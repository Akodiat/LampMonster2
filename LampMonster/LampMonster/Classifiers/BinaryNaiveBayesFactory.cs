using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class BinaryNaiveBayesFactory : IClassificationFactory
    {
        private int prior;

        public BinaryNaiveBayesFactory(int prior)
        {
            this.prior = prior;
        }

        public Classifyer GetClassifyer(List<CategoryData> categories)
        {
            var nbClassifiers = CreateCategorys(categories);
            return new NaiveBayesClassifyer(nbClassifiers, prior);
        }

        private static List<NaiveBayes> CreateCategorys(List<CategoryData> trainingData)
        {
            var categories = new List<NaiveBayes>();
            foreach (var item in trainingData)
            {
                categories.Add(CreateCategory(item));
            }

            return categories;
        }


        private static NaiveBayes CreateCategory(CategoryData categoryData)
        {
            var bag = new Dictionary<string, int>();
            int wordCount = 0;
            foreach (var document in categoryData.TrainingDocuments)
            {
                foreach (var feature in document)
                {
                    wordCount++;
                    if (!bag.ContainsKey(feature.Word))
                        bag[feature.Word] = 1;
                    else
                        bag[feature.Word]++;
                }
            }


            foreach (var item in bag.Keys.ToList())
            {
                if (bag[item] < 0)
                    bag.Remove(item);
            }

            return new NaiveBayes(categoryData.ID, wordCount, categoryData.CategoryProb, bag);
        }


        private class Comp : Comparer<KeyValuePair<string, int>>
        {

            public override int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
            {
                return y.Value - x.Value;
            }
        }


        public string ClassifyerDesc()
        {
            return string.Format("Binary Naive Bayes Prior {0}", prior);
        }
    }
}
