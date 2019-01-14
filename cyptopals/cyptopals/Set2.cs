using Xunit;

namespace cryptopals
{
    public class Set2
    {
        [Fact]
        public void Exercise1()
        {
            var block = "YELLOW SUBMARINE";
            var actualResult = block.PadRight(20, (char)4);

            var expectedResult = "YELLOW SUBMARINE\x04\x04\x04\x04";

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
