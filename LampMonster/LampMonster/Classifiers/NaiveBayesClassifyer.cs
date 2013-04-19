using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quadruple; 


namespace LampMonster
{
    class NaiveBayesClassifyer : Classifyer
    {
        private List<NaiveBayes> categories;
        private int prior;

        public NaiveBayesClassifyer(List<NaiveBayes> categories,
                                     int prior)
        {
            this.categories = categories;
            this.prior = prior;
        }

        public string Classify(Document document)
        {
            double max = double.NegativeInfinity;
            NaiveBayes selectedCategory = null;
            foreach (var category in this.categories)
            {
                double probability = category.ClassProbobility(document, prior);
                if (probability > max)
                {
                    max = probability;
                    selectedCategory = category;
                }
            }

            return selectedCategory.id;
        }
    }

}