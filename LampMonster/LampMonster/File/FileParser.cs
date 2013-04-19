using System;
using System.Collections.Generic;
using System.IO;

namespace LampMonster
{
    class FileParser
    {
        Dictionary<string, List<string>> cache = new Dictionary<string, List<string>>();

        public List<string> GetWordsInFile(string filePath, Predicate<string> wordFilter)
        {
            if (cache.ContainsKey(filePath))
                return cache[filePath];


            string[] words;
            using (TextReader reader = new StreamReader(filePath))
            {
                string fileString = reader.ReadToEnd();
                words = fileString.Split('.', ' ', ',', '\n', '\r', '"', '(', ')');
            }


            var list = new List<string>(words.Length);
            foreach (var word in words)
            {
                if (word != "" && word != "," && wordFilter(word))
                    list.Add(word.ToLower());
            }

            this.cache.Add(filePath, list);
            return list;
        }
    }
}
