using System;
using System.Globalization;

namespace Leauge_Auto_Accept
{
    internal class Functions
    {
        public static bool IsEnglishLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z' || c == ' ');
        }

        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}