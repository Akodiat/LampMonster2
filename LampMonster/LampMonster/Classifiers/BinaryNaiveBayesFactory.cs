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
                foreach (var word in document.GetUniqueWords())
                {
                    wordCount++;
                    if (!bag.ContainsKey(word))
                        bag[word] = 1;
                    else
                        bag[word]++;
                }
            }
            return new NaiveBayes(categoryData.ID, wordCount, categoryData.CategoryProb, bag);
        }


        public string ClassifyerDesc()
        {
            return string.Format("Binary Naive Bayes Prior {0}", prior);
        }
    }
}
