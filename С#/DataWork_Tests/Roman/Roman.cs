using System;

namespace Roman
{
    public static class Roman
    {
        private static int DecodeSingle(char letter) => letter switch
        {
            'I' => 1,
            'V' => 5,
            'X' => 10,
            'L' => 50,
            'C' => 100,
            'D' => 500,
            'M' => 1000,
            _ => throw new ArgumentException("String must be Roman!"),
        };

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
                if (DecodeSingle(uRoman[i]) < DecodeSingle(uRoman[i + 1]))
                    result -= DecodeSingle(uRoman[i]);
                else
                    result += DecodeSingle(uRoman[i]);
            }

            result += DecodeSingle(uRoman[stringLength - 1]);
            return result;
        }
    }
}
