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
        public List<Document> PosetiveDocuments { get; private set; }
        public List<Document> NegativeDocuments { get; private set; }

        public ClassData(string id,
                          List<Document> PosetiveDocuments,
                          List<Document> NegativeDocuments)
        {
            this.ClassID = id;
            this.PosetiveDocuments = PosetiveDocuments;
            this.NegativeDocuments = NegativeDocuments;
        }


        public List<Document> JoinedDocuments
        {
            get
            {
                var list = new List<Document>();
                list.AddRange(PosetiveDocuments);
                list.AddRange(NegativeDocuments);
                return list;
            }
        }

        public int JoinedCount
        {
            get { return this.PosetiveDocuments.Count + this.NegativeDocuments.Count; }
        }
    }
}
