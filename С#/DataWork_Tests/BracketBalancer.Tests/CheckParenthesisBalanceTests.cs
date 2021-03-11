using System;
using Xunit;

namespace BracketBalancer.Tests
{
    public class CheckParenthesisBalanceTests
    {
        [Fact]
        public void CheckParenthesisBalance_WithNotEnoughBrackets()
        {
            string line1 = "((1+3)((((4+(3-5)))";

            Assert.Equal("Не хватает закр. скобок: 3", Brackets.CheckParenthesisBalance(line1));
        }

        [Fact]
        public void CheckParenthesisBalance_WithEnoughBrackets()
        {
            string line2 = "((1+3)()(4+(3-5)))";

            Assert.Equal("OK", Brackets.CheckParenthesisBalance(line2));
        }

        [Fact]
        public void CheckParenthesisBalance_WithTooMuchBrackets()
        {
            string line3 = "((1+3)())(4+(3-5)))";

            Assert.Equal("Лишняя закрывающая скобка", Brackets.CheckParenthesisBalance(line3));
        }

        [Fact]
        public void CheckParenthesisBalance_WithEmptyString()
        {
            string line3 = "";

            Assert.Throws<ArgumentException>(() => Brackets.CheckParenthesisBalance(line3));
        }
    }
}
