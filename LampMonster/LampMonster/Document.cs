using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class Document : IEnumerable<string>
    {
        public readonly string ID;
        private List<string> words;
        private Dictionary<string, int> bagOfWords;

        public Document(string id,
                        List<string> words)
        {
            this.ID = id;
            this.words = words;
            this.bagOfWords = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!bagOfWords.ContainsKey(word))
                    bagOfWords.Add(word, 1);
                else
                    bagOfWords[word]++;
            }
        }

        public int WordCount
        {
            get { return this.words.Count; }
        }

        public IEnumerable<string> GetUniqueWords()
        {
            return bagOfWords.Keys;
        }
        
        public IEnumerator<string> GetEnumerator()
        {
            return words.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
