using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class NaiveBayes
    {
        public readonly string id;
        private readonly int wordCount;
        private readonly double categoryProb;
        private readonly Dictionary<string, int> bagOfWords;

        public NaiveBayes(string id, int vocabalarySize,
                        double categoryProb, Dictionary<string, int> bagOfWords)
        {
            this.id = id;
            this.wordCount = vocabalarySize;
            this.categoryProb = categoryProb;
            this.bagOfWords = bagOfWords;
        }

        public double ClassProbobility(Document document, int prior)
        {
            double product = 0;
            double nom = this.wordCount + this.bagOfWords.Count * prior;

            foreach (var word in document)
            {
                int denom;
                this.bagOfWords.TryGetValue(word, out denom);
                denom += prior;

                product += Math.Log(denom / nom);
            }

            return product + Math.Log(this.categoryProb);
        }
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
                foreach (var word in document)
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




    }

 

    
}
