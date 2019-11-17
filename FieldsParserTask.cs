using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("a \"a", new[] { "a", "a" })]
        [TestCase("\'a\' b \"c\"", new[] { "a", "b", "c" })]
        [TestCase("''", new[] { "" })]
        [TestCase(@"'\\'", new[] { "\\" })]
        [TestCase("a     b", new[] { "a", "b" })]
        [TestCase("'\"a\"'", new[] { "\"a\"" })]
        [TestCase("\"'a'\"", new[] { "'a'" })]
        [TestCase("a 'b  '", new[] { "a", "b  ", })]
        [TestCase("\"\\\\\"", new[] { "\\" })]
        [TestCase(@"""abc ", new[] { "abc " })]
        [TestCase("   b", new[] { "b" })]
        [TestCase("\"\"''", new[] { "", "" })]
        [TestCase("'\\'a\\''", new[] { "'a'" })]
        [TestCase("", new string[] { })]
        [TestCase("\"\\\"a\\\"\"", new[] { "\"a\"" })]
        [TestCase("a'b c d'f", new[] { "a", "b c d", "f"})]
        public static
    void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var parsedTokens = new List<Token>();
            int i = 0;
            while (i<line.Length - 1)
            {
                if (char.IsWhiteSpace(line[i]))
                    i++;
                else if (line[i] == '\'' || line[i] == '\"')
                {
                    parsedTokens.Add(ReadQuotedField(line, i));
                    i = parsedTokens[parsedTokens.Count -1].GetIndexNextToToken();
                }
                else if (line[i] == '\\' || char.IsLetterOrDigit(line[i]))
                {
                    parsedTokens.Add(ReadField(line, i));
                    i = parsedTokens[parsedTokens.Count - 1].GetIndexNextToToken();
                }
            }
            return parsedTokens;
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            var i = startIndex;
            var valueOfToken = new StringBuilder();
            while (!char.IsWhiteSpace(line[i]) && i < line.Length - 1 && line[i] != '\'' && line[i] != '\"')
            { 
                    valueOfToken.Append(line[i]);
                    i++;
            }
            return new Token(valueOfToken.ToString(), startIndex, i - startIndex);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            int i = startIndex + 1;
            var valueOfToken = new StringBuilder();
            while (line[startIndex] != line[i] && i<line.Length - 1)
            {
                if (line[i] == '\\')
                {
                    valueOfToken.Append(line[i + 1]);
                    i += 2;
                }
                else
                {
                    valueOfToken.Append(line[i]);
                    i++;
                }
            }
            return new Token(valueOfToken.ToString(), startIndex, i + 1 - startIndex);
        }
    }
}