﻿using System;
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
            public readonly int wordCount;
            public readonly Quad categoryProb;
            public readonly Dictionary<string, int> bagOfWords;

            public Category(string id, int vocabalarySize,
                            Quad categoryProb, Dictionary<string, int> bagOfWords)
            {
                this.id = id;
                this.wordCount = vocabalarySize;
                this.categoryProb = categoryProb;
                this.bagOfWords = bagOfWords;
            }
        }


        private List<Category> categories;
        private int prior;

        public NaiveBayesClassifyer(List<CategoryData> categories,
                                     int prior)
        {
            this.categories = CreateCategorys(categories);
            this.prior = prior;
        }


        private static List<Category> CreateCategorys(List<CategoryData> trainingData)
        {
            var categories = new List<Category>();
            foreach (var item in trainingData)
            {
                categories.Add(CreateCategory(item));
            }

            return categories;
        }

        private static Category CreateCategory (CategoryData categoryData)
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
            return new Category(categoryData.ID, wordCount, categoryData.CategoryProb, bag); 
        }


        private double CalculateCategoryProbability(Document document, Category category) 
        {
            double product = 0;
            double nom = category.wordCount + category.bagOfWords.Count * prior;

            foreach (var word in document)
            {
                int denom;
                category.bagOfWords.TryGetValue(word, out denom);
                denom += prior;

                product += Math.Log(denom / nom);
            }

            return product + Math.Log((double)category.categoryProb);
        }

        public string Classify(Document document)
        {
            double max = double.NegativeInfinity;
            Category selectedCategory = null;
            foreach (var category in this.categories)
            {
                double probability = CalculateCategoryProbability(document, category);
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