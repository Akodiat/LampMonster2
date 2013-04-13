using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W3b.Sine;


namespace LampMonster
{
    class NaiveBayesClassifyer : Classifyer
    {
        private class Category
        {
            public readonly string id;
            public readonly int vocabalarySize;
            public readonly Dictionary<string, double> bagOfWords;

            public Category(string id, int vocabalarySize, Dictionary<string, double> bagOfWords)
            {
                this.id = id;
                this.vocabalarySize = vocabalarySize;
                this.bagOfWords = bagOfWords;
            }
        }


        List<Category> categories;
        double prior;

        public NaiveBayesClassifyer(Dictionary<string, List<string>> trainingSet,
                                     double prior)
        {
            this.categories = CreateCategorys(trainingSet);
            this.prior = prior;
        }


        private static List<Category> CreateCategorys(Dictionary<string, List<string>> trainingSet)
        {
            var categories = new List<Category>();
            foreach (var item in trainingSet)
            {
                var bag = CreateMegaDoc(item.Value); 
                categories.Add(new Category(item.Key, item.Value.Count, bag));
            }

            return categories;
        }

        private static Dictionary<string, double> CreateMegaDoc(List<string> words)
        {
            var bag = new Dictionary<string, double>();
            foreach (var word in words)
            {
                if (!bag.ContainsKey(word))
                    bag[word] = 1.0d;
                else
                    bag[word]++;
            }



            return bag;
        }


        private BigNum CalculateCategoryProbability(List<string> document, Category category) 
        {
            BigNum product = 1;
            BigNum nom = new BigNumDec(category.vocabalarySize + category.bagOfWords.Count * this.prior);

            foreach (var word in document)
            {
                double denom;
                category.bagOfWords.TryGetValue(word, out denom);
                denom += prior;
                product = new BigNumDec(denom) / nom;
            }

            return product;
        }

        public string Classify(List<string> document)
        {
            BigNum max = 0.0d;
            Category selectedCategory = null;
            foreach (var category in this.categories)
            {
                BigNum probability = CalculateCategoryProbability(document, category);
                if (probability.CompareTo(max) < 0)
                {
                    max = probability;
                    selectedCategory = category;
                }
            }

            return selectedCategory.id;
        }
    }
}
