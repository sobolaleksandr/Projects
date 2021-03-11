using System;

namespace BracketBalancer
{
    public static class Brackets
    {
        public static string CheckParenthesisBalance(string line)
        {
            if (line == "")
                throw new ArgumentException("String must not be Empty!");

            int openingParsCount = 0;
            int closingParsCount = 0;
            int lineLength = line.Length;

            for (int i = 0; i < lineLength; i++)
            {
                if (line[i] == '(')
                {
                    openingParsCount++;
                }
                if (line[i] == ')')
                {
                    closingParsCount++;
                }

                if (openingParsCount < closingParsCount)
                {
                    return "Лишняя закрывающая скобка";
                }
            }

            if (openingParsCount > closingParsCount)
            {
                return "Не хватает закр. скобок: " + (openingParsCount - closingParsCount);
            }

            return "OK";
        }

    }
}
