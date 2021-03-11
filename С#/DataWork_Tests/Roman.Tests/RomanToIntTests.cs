using System;
using System.Text;
using Xunit;

namespace Roman.Tests
{
    public class RomanToIntTests
    {
		[Fact]
		public void RomanToInt_WithEmptyString_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(()=>Roman.RomanToInt(""));
		}

		[Fact]
		public void RomanToInt_WithBadString_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => Roman.RomanToInt("asd"));
		}

		[Fact]
        public void RomanToInt_From1To3000()
        {
			for (int i=1; i <= 3000; i++)
            {
				Assert.Equal(i, Roman.RomanToInt(NumberToRoman(i)));
            }
        }

		private static string NumberToRoman(int number)
		{
			if (number < 0 || number > 3999)
				throw new ArgumentException("Value must be in the range 0 - 3,999.");

			int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
			string[] numerals = new string[]
			{ "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };


			StringBuilder result = new StringBuilder();


			for (int i = 0; i < 13; i++)
			{
				while (number >= values[i])
				{
					number -= values[i];
					result.Append(numerals[i]);
				}
			}

			return result.ToString();
		}
	}
}
