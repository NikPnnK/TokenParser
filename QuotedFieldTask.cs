using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(actualToken, new Token(expectedValue, startIndex, expectedLength));
        }

        // Добавьте свои тесты
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var valueOfToken = new StringBuilder();
            var positionOfToken = -1;
            var startChar = '◙';
            int i = startIndex;
            for (; i < line.Length; i++)
            {
                if (line[i] != '"' && line[i] != '\'' && positionOfToken == -1)
                    continue;
                else if ((line[i] == '"' || line[i] == '\'') && positionOfToken == -1)
                {
                    positionOfToken = i;
                    startChar = line[i];
                }
                else if (line[i] == '\\' && positionOfToken != -1)
                {
                    valueOfToken.Append(line[i]);
                    i++;
                }
                else if (line[i] != startChar && line[i] != '\\' && positionOfToken != -1)
                {
                    valueOfToken.Append(line[i]);
                }
                else if (line[i] == startChar && positionOfToken != -1)
                    break;
            }
            return new Token(valueOfToken.ToString(), positionOfToken, i - positionOfToken + 1);
        }
    }
}
