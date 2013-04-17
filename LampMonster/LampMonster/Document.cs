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


        public Document(string id,
                        List<string> words)
        {
            this.ID = id;
            this.words = words;
        }
             

        public int WordCount
        {
            get { return this.words.Count; }
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
