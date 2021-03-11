using System;

namespace Roman
{
    public static class Roman
    {
        private static int decodeSingle(char letter)
        {
            switch (letter)
            {
                case 'M':
                    return 1000;
                case 'D':
                    return 500;
                case 'C':
                    return 100;
                case 'L':
                    return 50;
                case 'X':
                    return 10;
                case 'V':
                    return 5;
                case 'I':
                    return 1;
                default:
                    throw new ArgumentException("String must be Roman!");
            }
        }

        public static int RomanToInt(string roman)
        {
            if (roman == "")
                throw new ArgumentException("String must not be Empty!");

            int result = 0;
            string uRoman = roman.ToUpper(); //case-insensitive
            int stringLength = uRoman.Length;

            for (int i = 0; i < stringLength - 1; i++)
            {
                //loop over all but the last character
                if (decodeSingle(uRoman[i]) < decodeSingle(uRoman[i + 1]))
                {
                    result -= decodeSingle(uRoman[i]);
                }
                else
                {
                    result += decodeSingle(uRoman[i]);
                }
            }
            result += decodeSingle(uRoman[uRoman.Length- 1]);
            return result;
        }
    }
}
