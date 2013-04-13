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
        private class Category
        {
            public readonly string id;
            public readonly int vocabalarySize;
            public readonly Dictionary<string, int> bagOfWords;

            public Category(string id, int vocabalarySize, Dictionary<string, int> bagOfWords)
            {
                this.id = id;
                this.vocabalarySize = vocabalarySize;
                this.bagOfWords = bagOfWords;
            }
        }


        List<Category> categories;
        int prior;

        public NaiveBayesClassifyer(Dictionary<string, List<string>> trainingSet,
                                     int prior)
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

        private static Dictionary<string, int> CreateMegaDoc(List<string> words)
        {
            var bag = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!bag.ContainsKey(word))
                    bag[word] = 1;
                else
                    bag[word]++;
            }



            return bag;
        }


        private Quad CalculateCategoryProbability(List<string> document, Category category) 
        {
            Quad product = 1;
            Quad nom = category.vocabalarySize + category.bagOfWords.Count * prior;

            foreach (var word in document)
            {
                int denom;
                category.bagOfWords.TryGetValue(word, out denom);
                denom += prior;
                product *= denom / nom;
            }

            return product;
        }

        public string Classify(List<string> document)
        {
            Quad max = 0.0d;
            Category selectedCategory = null;
            foreach (var category in this.categories)
            {
                Quad probability = CalculateCategoryProbability(document, category);
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