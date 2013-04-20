using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class BagBuilder
    {
        public static Dictionary<string, int> BuildBagFromDocument(Document doc)
        {
            var bagOfWords = new Dictionary<string, int>();
            foreach (var word in doc)
            {
                if (!bagOfWords.ContainsKey(word))
                    bagOfWords.Add(word, 0);
                else
                    bagOfWords[word]++;
            }
			return bagOfWords;
        }

        public static List<Dictionary<string, int>> BuildBagsFromDocuments(List<Document> dox)
        {
            var bagsOfWords = new List<Dictionary<string, int>>();
            foreach (var doc in dox)
            {
                bagsOfWords.Add(BuildBagFromDocument(doc));
            }
            return bagsOfWords;
        }
    }
}
