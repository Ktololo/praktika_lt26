using task01;
using Xunit;

namespace task01Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void IsPalindrome_SimplePalindrome_ReturnsTrue()
        {
            string input = "racecar";
            bool result = input.IsPalindrome();
            Assert.True(result);
        }
        [Fact]
        public void IsPalindrome_PalindromeWithSpacesAndUpperCase_ReturnsTrue()
        {
            string input = "A man a plan a canal Panama";
            bool result = input.IsPalindrome();
            Assert.True(result);
        }
        [Fact]
        public void IsPalindrome_PalindromeWithPunctuation_ReturnsTrue()
        {
            string input = "Madam, I'm Adam";
            bool result = input.IsPalindrome();
            Assert.True(result);
        }
        [Fact]
        public void IsPalindrome_NotPalindrome_ReturnsFalse()
        {
            string input = "hello";
            bool result = input.IsPalindrome();
            Assert.False(result);
        }
        [Fact]
        public void IsPalindrome_EmptyString_ReturnsFalse()
        {
            string input = "";
            bool result = input.IsPalindrome();
            Assert.False(result);
        }
        [Fact]
        public void IsPalindrome_NullString_ReturnsFalse()
        {
            string input = null;
            bool result = input.IsPalindrome();
            Assert.False(result);
        }
        [Fact]
        public void IsPalindrome_SingleCharacter_ReturnsTrue()
        {
            string input = "a";
            bool result = input.IsPalindrome();
            Assert.True(result);
        }
        [Fact]
        public void IsPalindrome_RussianPalindrome_ReturnsTrue()
        {
            string input = "А роза упала на лапу Азора";
            bool result = input.IsPalindrome();
            Assert.True(result);
        }
    }
}