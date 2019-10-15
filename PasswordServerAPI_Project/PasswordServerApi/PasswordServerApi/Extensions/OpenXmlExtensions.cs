using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PasswordServerApi.Models.Enums;

namespace PasswordServerApi.Service
{

	public static class OpenXmlExtensions
	{
		public static string GetCellValue(this IEnumerable<Cell> row, SpreadsheetDocument doc, string address)
		{
			Cell cell = row.FirstOrDefault(c => c.CellReference == address);
			CellValue rawValue = cell?.CellValue;
			if (rawValue == null)
				return null;

			if (cell.DataType == null) // number & dates
				return GetNumberOrDateCellValue(doc, cell);
			else // Shared string or boolean
			{
				switch (cell.DataType.Value)
				{
					case CellValues.SharedString:
						return GetStringCellValue(doc, cell);
					case CellValues.Boolean:
						return GetBooleanCellValue(doc, cell);
					default:
						return GetUnformattedCellValue(doc, cell);
				}
			}
		}

		private static string GetNumberOrDateCellValue(SpreadsheetDocument doc, Cell cell)
		{
			int styleIndex = (int)cell.StyleIndex.Value;
			CellFormat cellFormat = (CellFormat)doc.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ElementAt(styleIndex);
			uint formatId = cellFormat.NumberFormatId.Value;

			if (formatId == (uint)OpenXmlCellFormats.PercentageDecimal || formatId == (uint)OpenXmlCellFormats.Percentage)
			{
				decimal percentage = decimal.Parse(cell.InnerText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture) * 100m;
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0.###}", percentage);
			}
			else if (formatId == (uint)OpenXmlCellFormats.DateShort || formatId == (uint)OpenXmlCellFormats.DateLong || (
				formatId != (uint)OpenXmlCellFormats.General &&
				formatId != (uint)OpenXmlCellFormats.Text &&
				formatId != (uint)OpenXmlCellFormats.DecimalWithThousandsSeparator &&
				formatId != (uint)OpenXmlCellFormats.NumberWithThousandsSeparator)
			)
			{
				return GetDateNumberFromCell(doc, cell, formatId);
			}
			return cell.InnerText;
		}

		private static string GetDateNumberFromCell(SpreadsheetDocument doc, Cell cell, uint formatId)
		{
			var numberingFormat = doc.WorkbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats?.Cast<NumberingFormat>().SingleOrDefault(f => f.NumberFormatId.Value == formatId);
			bool isNumber = numberingFormat != null;
			if (isNumber)
			{
				string formatString = numberingFormat.FormatCode.Value;
				double number = double.Parse(cell.InnerText);
				return number.ToString(formatString, CultureInfo.InvariantCulture);
			}
			else
			{
				double oaDate;
				if (double.TryParse(cell.InnerText, out oaDate))
					return DateTime.FromOADate(oaDate).ToString("o");
			}
			return cell.InnerText;
		}

		private static string GetStringCellValue(SpreadsheetDocument doc, Cell cell)
		{
			SharedStringItem ssi = doc.WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(int.Parse(cell.CellValue.InnerText));
			return ssi.Text.Text.Trim();
		}

		private static string GetBooleanCellValue(SpreadsheetDocument doc, Cell cell)
		{
			return cell.CellValue.InnerText == "0" ? "false" : "true";
		}

		private static string GetUnformattedCellValue(SpreadsheetDocument doc, Cell cell)
		{
			return cell.CellValue.InnerText.Trim();
		}
	}
}
