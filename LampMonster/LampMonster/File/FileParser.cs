using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LampMonster
{
    class FileParser
    {
        ISet<string> stopwords;
        char[] splitTokens;

        public FileParser(string stopWordsPath, params char[] tokens)
        {
            this.splitTokens = tokens;
            this.stopwords = new HashSet<string>();
            using (StreamReader reader = new StreamReader("stopwords.txt"))
            {
                while (!reader.EndOfStream)
                    stopwords.Add(reader.ReadLine());
            }
        }



        public List<string> GetWordsInFile(string filePath, Predicate<string> wordFilter = null)
        {
            if (wordFilter == null)
                wordFilter = (s) => true;


            string[] words;
            using (TextReader reader = new StreamReader(filePath))
            {
                string fileString = reader.ReadToEnd();
                words = fileString.Split(splitTokens);
            }

            var list = new List<string>(words.Length);
            foreach (var word in words)
            {
                string w = word.ToLower();
                if (w != "" && w != "," && !stopwords.Contains(w) && !wordFilter(w))
                    list.Add(w);
            }

            return list;
        }
    }
}
