using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    struct DocumentFeature
    {
        public readonly string Word;
        public readonly int Count;
        public readonly double Frequency;
        public readonly double NormalizedFrequency;

        public DocumentFeature(string word, int count, double frequency, double normalizedFrequency)
        {
            this.Word = word;
            this.Count = count;
            this.Frequency = frequency;
            this.NormalizedFrequency = normalizedFrequency;
        }
    }

    class Document : IEnumerable<DocumentFeature>
    {
        public readonly string ID;
        public readonly int WordCount;
        private Dictionary<string, DocumentFeature> words;

        public Document(string id,
                        List<string> words)
        {
            this.ID = id;

            int wordCount = 0;
            var bag = new Dictionary<string, int>();
            foreach (var word in words)
            {
                wordCount++;
                if (!bag.ContainsKey(word))
                    bag.Add(word, 1);
                else
                    bag[word]++;
            }

            this.WordCount = wordCount;
            double max = bag.Max(x => x.Value / (double)wordCount);
           
            this.words = new Dictionary<string, DocumentFeature>(bag.Count);
            foreach (var item in bag)
            {
				double frequency = (double)item.Value / wordCount;
				double normalizedFrequency = frequency/max;
                this.words.Add(item.Key, new DocumentFeature(item.Key, item.Value, frequency, normalizedFrequency));
            }
        }


        public IEnumerator<DocumentFeature> GetEnumerator()
        {
            return words.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

		/// <summary>
		/// Inverse document frequency
		/// </summary>
		/// <param name="?"></param>
		public static double IDF(string featureID, List<Document> documents)
        {
            int documentCount = 0;
			foreach (var document in documents)
			{
                if (document.words.ContainsKey(featureID))
                    documentCount++;
			}
			double quota = (double)documents.Count/documentCount;
			
			return Math.Log(quota);
        }
    }
}
