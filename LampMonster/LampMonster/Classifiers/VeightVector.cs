using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class VeightVector
    {
        private Dictionary<string, double> representation;

        public VeightVector(List<string> fetures)
        {
            this.representation = new Dictionary<string, double>();
            foreach (var feture in fetures)
            {
                this.representation.Add(feture, 0);
            }
        }

        public VeightVector(VeightVector other)
            : this(other.representation.Keys.ToList())
        {
        }

        public double this[string feture] 
        {
            get
            {
                double value;
                representation.TryGetValue(feture, out value);
                return value;
            }
            set
            {
                if (representation.ContainsKey(feture))
                    representation[feture] = value;
            }
        }


        public double DotProduct(Document document)
        {
            double dotProduct = 0;
            foreach (var feture in document)
            {
                double weightOfWord;
                if (representation.TryGetValue(feture.Word, out weightOfWord))
                    dotProduct += weightOfWord * feture.Frequency;
            }
            return dotProduct;
        }


    }
}
