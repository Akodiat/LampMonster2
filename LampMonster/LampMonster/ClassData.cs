using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class ClassData
    {
        public string ClassID { get; private set; }
        public List<List<string>> PosetiveDocuments { get; private set; }
        public List<List<string>> NegativeDocuments { get; private set; }

        public ClassData(string id,
                          List<List<string>> PosetiveDocuments,
                          List<List<string>> NegativeDocuments)
        {
            this.ClassID = id;
            this.PosetiveDocuments = PosetiveDocuments;
            this.NegativeDocuments = NegativeDocuments;
        }

    }
}
