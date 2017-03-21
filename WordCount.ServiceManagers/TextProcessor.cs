using System.Collections.Generic;
using System.Linq;

namespace WordCount.ServiceManagers
{
    public class TextProcessor : ITextProcessor
    {
        public Dictionary<string, int> CountWords(string text)
        {
            Dictionary<string, int> wordOccurence = new Dictionary<string, int>();

            char[] trimChars = new[] { '.', '\'', ',', '-', '?', '"', '/', ';', '!', ':', '(', ')', '_' };
            
            IList<char> charList = new List<char>();
            
            for (int index = 0; index < text.Length; index++)
            {
                while (index < text.Length && (char.IsLetterOrDigit(text[index]) || text[index] == '\''))
                {
                    charList.Add(text[index]);
                    index++;
                }
                if (charList.Any())
                {
                    string word = string.Join(string.Empty, charList).ToUpper().TrimEnd(trimChars).TrimStart(trimChars);
                    if (wordOccurence.ContainsKey(word))
                    {
                        wordOccurence[word]++;
                    }
                    else
                    {
                        wordOccurence.Add(word, 1);
                    }

                    charList.Clear();
                }
            }

            return wordOccurence;
        }

        public IList<string> BreakIntoChunks(string text)
        {
            int originalSectionLength = text.Length / 8; // i have a quand core processor with hyper threading hence 8.
            int sectionLength = originalSectionLength;
            int startIndex = 0;

            IList<string> sectionStrings = new List<string>();

            while (string.Join(string.Empty, sectionStrings).Length + 1 < text.Length)
            {
                sectionLength = startIndex + sectionLength < text.Length
                    ? sectionLength
                    : text.Length - startIndex - 1;

                char criticalLetter = text[startIndex + sectionLength];

                while (char.IsLetter(criticalLetter) || char.IsDigit(criticalLetter))
                {
                    sectionLength++;
                    criticalLetter = text[startIndex + sectionLength];
                }

                string sectionString = text.Substring(startIndex, sectionLength);
                sectionStrings.Add(sectionString);
                startIndex += sectionString.Length;
                sectionLength = originalSectionLength;
            }

            return sectionStrings;
        }
    }
}