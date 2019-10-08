using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.Models.Enums
{
	public enum OpenXmlCellFormats : uint
	{
		General = 0,
		Number = 1,
		Decimal = 2,
		NumberWithThousandsSeparator = 3,
		DecimalWithThousandsSeparator = 4,
		Percentage = 9,
		PercentageDecimal = 10,
		Fraction = 12,
		Scientific = 11,
		DateShort = 14,
		Accounting = 44,
		Text = 49,
		Currency = 164,
		DateLong = 165,
		Time = 166,
	}
}
