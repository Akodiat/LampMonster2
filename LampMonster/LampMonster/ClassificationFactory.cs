using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    interface IClassificationFactory 
    {
        Classifyer GetClassifyer(List<CategoryData> categories);
    }

    class NaiveBayesFactory : IClassificationFactory
    {
        private int prior;

        public NaiveBayesFactory(int prior)
        {
            this.prior = prior;
        }

        public Classifyer GetClassifyer(List<CategoryData> categories)
        {
            return new NaiveBayesClassifyer(categories, prior);
        }
    }
}
