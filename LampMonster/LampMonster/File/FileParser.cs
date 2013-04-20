using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LampMonster
{
    class FileParser
    {

        public List<string> GetWordsInFile(string filePath, Predicate<string> wordFilter)
        {
            string[] words;
            using (TextReader reader = new StreamReader(filePath))
            {
                string fileString = reader.ReadToEnd();
                words = fileString.Split('.', ' ', ',', '\n', '\r', '"', '(', ')');
            }



            string[] stopWords = { "it", "is", "and", "or", "this", "can", "be", "you", "i" }; //Think about this.

            var list = new List<string>(words.Length);
            foreach (var word in words)
            {
                if(IsUpper(word)) {
                    list.Add(word);
                }

                if (word != "" && word != "," && !stopWords.Contains(word) &&  wordFilter(word))
                    list.Add(word.ToLower());
            }

            return list;
        }

        private bool IsUpper(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsLower(s[i]))
                    return false;
            }
            return true;
        }
    }
}
