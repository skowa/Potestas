using System;
using System.Globalization;

namespace Potestas.WebPlugin.ExtensionMethods
{
	internal static class StringExtensionMethods
	{
		internal static DateTime ToDateTime(this string jsonFormatDateTime)
		{
			if (!TryParse(jsonFormatDateTime, "yyyy-MM-ddTHH:mm:ss", out var time))
			{
				if (!TryParse(jsonFormatDateTime, "yyyy-MM-ddTHH:mm:ss.fff", out time))
				{
					throw new InvalidOperationException($"Cannot convert string {jsonFormatDateTime} to DateTime.");
				}
			}

			return time;
		}

		private static bool TryParse(string json, string format, out DateTime dateTime)
		{
			return DateTime.TryParseExact(json.Replace("\"", ""), format,
				CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
		}
	}
}