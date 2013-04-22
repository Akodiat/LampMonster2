using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class FailLog
    {
        private Dictionary<string, int> documentFail; 
        private object _lock;
        
        public FailLog()
        {
            documentFail = new Dictionary<string, int>();
            _lock = new object();
        }


        public void LogClassificationFailure(Document document)
        {
            lock (_lock)
            {
                if (!documentFail.ContainsKey(document.ID))
                    documentFail.Add(document.ID, 1);
                else
                    documentFail[document.ID]++;
            }
        }

        public void PrintWorst(int count, StreamWriter writer)
        {
            var list = new List<string>(documentFail.Keys);
            list.Sort((x, y) =>
            {
                double xW = documentFail[x];
                double yW = documentFail[y];

                if (xW < yW) return 1;
                else if (xW > yW) return -1;
                else return 0;
            });

            for (int i = 0; i < count; i++)
			{
                writer.WriteLine(list[i] + "\t" + documentFail[list[i]]);
            }
        }

    }
}
