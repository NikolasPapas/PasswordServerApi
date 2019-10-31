using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace PasswordServerApi.Extensions
{
	public static class ExportHelper
	{

		#region Basic Pdf Export Helper

		public static HttpResponseMessage ExportPdf(this ByteArrayContent buffer, string fileName)
		{
			var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
			response.Headers.Clear();
			response.Content = buffer;
			SetFileSettings(fileName, response, "application/pdf");
			return response;
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

		public static double GetDocumentContentWidthInMillimeters(Document doc, Section section)
		{
			return GetDocumentPageWidthInMillimeters(doc, section) - GetSectionLeftMarginInMillimeters(section) - GetSectionRightMarginInMillimeters(section);
		}

		public static double GetDocumentPageWidthInMillimeters(Document doc, Section section)
		{
			return section.PageSetup.Orientation == Orientation.Portrait ? doc.DefaultPageSetup.PageWidth.Millimeter : doc.DefaultPageSetup.PageHeight.Millimeter;
		}

		public static double GetSectionLeftMarginInMillimeters(Section section)
		{
			return section.PageSetup.LeftMargin.Millimeter;
		}

		public static double GetSectionRightMarginInMillimeters(Section section)
		{
			return section.PageSetup.RightMargin.Millimeter;
		}

		#endregion
	}
}
