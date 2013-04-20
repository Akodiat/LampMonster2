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

        public DocumentFeature(string word, int count, double frequency)
        {
            this.Word = word;
            this.Count = count;
            this.Frequency = frequency;
        }
    }


    class Document : IEnumerable<DocumentFeature>
    {
        public readonly string ID;
        public readonly int WordCount;
        private List<DocumentFeature> words;

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
           
            this.words = new List<DocumentFeature>(bag.Count);
            foreach (var item in bag)
            {
                this.words.Add(new DocumentFeature(item.Key, item.Value, (double)item.Value / wordCount));
            }
        }


        public IEnumerator<DocumentFeature> GetEnumerator()
        {
            return words.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
