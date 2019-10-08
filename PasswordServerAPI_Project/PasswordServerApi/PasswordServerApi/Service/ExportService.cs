using OfficeOpenXml;
using OfficeOpenXml.Style;
using PasswordServerApi.DTO;
using PasswordServerApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PasswordServerApi.Service
{
	public class ExportService : IExportService
	{
		private IBaseService _baseService;
		private ILoggingService _logger;

		public ExportService(IBaseService baseService, ILoggingService logger)
		{
			_baseService = baseService;
			_logger = logger;
		}

		public HttpResponseMessage Export()
		{
			List<AccountDto> accounts = _baseService.GetAccounts(new Models.Account.Requests.AccountActionRequest() { }).ToList();

			using (var package = new ExcelPackage())
			{
				ExcelWorksheet FirstFirstWorksheet = package.Workbook.Worksheets.Add("Λίστα λογαριασμόν");
				FirstFirstWorksheet.PrinterSettings.Orientation = eOrientation.Landscape;

				FirstRowData(FirstFirstWorksheet);
				RestData(FirstFirstWorksheet, accounts);
				SetExcelStyles(FirstFirstWorksheet);

				accounts.ForEach(account =>
				{
					ExcelWorksheet data = (package.Workbook.Worksheets.Add(account.UserName));
					AddWorksheetData(data, account);
				});

				var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
				response.Headers.Clear();
				response.Content = new ByteArrayContent(package.GetAsByteArray());
				SetFileSettings("Report_" + DateTime.Now.Date.ToShortDateString(), response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
				package.SaveAs(new FileInfo("C:\\exporPasswordServerApi.xlsx"));
				return response;
			}
		}

		internal static void SetFileSettings(string fileName, HttpResponseMessage response, String contentType)
		{
			var mt = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
			var con = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
			{
				FileName = fileName
			};
			response.Content.Headers.ContentType = mt;
			response.Content.Headers.ContentDisposition = con;
		}

		private void FirstRowData(ExcelWorksheet Worksheet)
		{
			AddCell(Worksheet, 1, 1, "UserName", true);
			AddCell(Worksheet, 1, 2, "Sheet", true);
		}

		private void RestData(ExcelWorksheet Worksheet, List<AccountDto> accounts)
		{
			int rowIndex = 2;
			foreach (var app in accounts)
			{
				SedDataRow(Worksheet, app, rowIndex);
				rowIndex++;
			}
			Worksheet.Cells[1, 1, accounts.Count(), 2].AutoFilter = true;
		}


		private void SedDataRow(ExcelWorksheet Worksheet, AccountDto account, int rowIndex)
		{
			AddCell(Worksheet, rowIndex, 1, account.UserName, false);
			AddCell(Worksheet, rowIndex, 2, $"{rowIndex}", false);
			Worksheet.Cells[rowIndex, 2].Hyperlink = new ExcelHyperLink($"{account.UserName}!A1", $"Sheet: {rowIndex}");
			Worksheet.Cells[rowIndex, 2].AutoFitColumns();
		}


		private void SetExcelStyles(ExcelWorksheet Worksheet)
		{
			Worksheet.Cells.Style.Font.Name = "Calibri";
			Worksheet.Cells.Style.Font.Size = 14;
			Worksheet.PrinterSettings.Orientation = eOrientation.Landscape;
			Worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
			Worksheet.PrinterSettings.PageOrder = ePageOrder.OverThenDown;
			Worksheet.PrinterSettings.Scale = 100;
			Worksheet.PrinterSettings.FitToHeight = 0;
			Worksheet.PrinterSettings.HorizontalCentered = true;
		}


		private void AddWorksheetData(ExcelWorksheet Worksheet, AccountDto account)
		{
			AddAccountData(Worksheet, account);
			AddPassworddata(Worksheet, account.Passwords);
		}

		private void AddAccountData(ExcelWorksheet Worksheet, AccountDto account)
		{
			int rowIndexHeder = 1;
			int rowIndexData = 2;

			AddCell(Worksheet, rowIndexHeder, 1, "UserName", true);
			AddCell(Worksheet, rowIndexData, 1, account.UserName, false);

			AddCell(Worksheet, rowIndexHeder, 2, "FirstName", true);
			AddCell(Worksheet, rowIndexData, 2, account?.FirstName, false);

			AddCell(Worksheet, rowIndexHeder, 3, "LastName", true);
			AddCell(Worksheet, rowIndexData, 3, account?.LastName, false);

			AddCell(Worksheet, rowIndexHeder, 4, "Email", true);
			AddCell(Worksheet, rowIndexData, 4, account?.Email, false);

			AddCell(Worksheet, rowIndexHeder, 5, "Sex", true);
			AddCell(Worksheet, rowIndexData, 5, account?.Sex + "", false);

			AddCell(Worksheet, rowIndexHeder, 6, "LastLogIn", true);
			AddCell(Worksheet, rowIndexData, 6, account?.LastLogIn != null ? account?.LastLogIn.Value.ToShortDateString() : "", false);

			AddCell(Worksheet, rowIndexHeder, 7, "Password", true);
			AddCell(Worksheet, rowIndexData, 7, account?.Password, false);

			AddCell(Worksheet, rowIndexHeder, 8, "Role", true);
			AddCell(Worksheet, rowIndexData, 8, account?.Role, false);

			AddCell(Worksheet, rowIndexHeder, 9, "AccountId", true);
			AddCell(Worksheet, rowIndexData, 9, account?.AccountId.ToString(), false);
		}

		private void AddPassworddata(ExcelWorksheet Worksheet, List<PasswordDto> paswords)
		{

			AddCell(Worksheet, 5, 1, "UserName", true);
			AddCell(Worksheet, 6, 1, "name", true);
			AddCell(Worksheet, 7, 1, "Password", true);
			AddCell(Worksheet, 8, 1, "LogInLink", true);
			AddCell(Worksheet, 9, 1, "Sensitivity", true);
			AddCell(Worksheet, 10, 1, "Strength", true);
			AddCell(Worksheet, 11, 1, "PasswordId", true);

			int columnIndexData = 2;
			int index = 1;
			paswords.ForEach(pasword =>
			{
				pasword = _baseService.GetPassword(pasword.PasswordId);
				AddCell(Worksheet, 4, columnIndexData, $"Passsword {index}", true);
				AddCell(Worksheet, 5, columnIndexData, pasword.UserName, false);
				AddCell(Worksheet, 6, columnIndexData, pasword.Name, false);
				AddCell(Worksheet, 7, columnIndexData, pasword.Password, false);
				AddCell(Worksheet, 8, columnIndexData, pasword.LogInLink, false);
				AddCell(Worksheet, 9, columnIndexData, pasword.Sensitivity.ToString(), false);
				AddCell(Worksheet, 10, columnIndexData, pasword.Strength.ToString(), false);
				AddCell(Worksheet, 11, columnIndexData, pasword.PasswordId.ToString(), false);
				columnIndexData++;
				index++;
			});
		}

		private void AddCell(ExcelWorksheet Worksheet, int row, int column, string Value, bool Header)
		{
			Worksheet.Cells[row, column].Value = Value;
			Worksheet.Cells[row, column].Style.Font.Size = 12;
			if (Header)
			{
				Worksheet.Cells[row, column].Style.Font.Bold = true;
				Worksheet.Cells[row, column].Style.Font.Size = 14;
			}
			Worksheet.Cells[row, column].AutoFitColumns();
		}
	}
}
