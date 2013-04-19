using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class CategorizeClassificationManager : TestManager<double[,]>
    {
        private IClassificationFactory factory;

        public CategorizeClassificationManager(IClassificationFactory factory)
        {
            this.factory = factory;
        }


        private static List<CategoryData> CreateCategories(List<ClassData> testData, List<ClassData> trainingData)
        {
            var list = new List<CategoryData>();

            //We need this to calculate P(C) 
            int totalDocs = 0;
            foreach (var item in testData)
                totalDocs += item.JoinedCount;

            foreach (var item in trainingData)
            {
                list.Add(new CategoryData(item.ClassID,
                                         (double)item.JoinedCount / totalDocs,
                                         item.JoinedDocuments));
            }

            return list;
        }

        public double[,] RunPartialTests(List<ClassData> trainingData, List<ClassData> testData)
        {
            int categoryCount = trainingData.Count;
            var result = new double[categoryCount, categoryCount];
            var indexMap = new Dictionary<string, int>();
            for (int i = 0; i < testData.Count; i++)
            {
                indexMap[testData[i].ClassID] = i;
            }

            var classifyer = factory.GetClassifyer(CreateCategories(testData, trainingData));
            for (int i = 0; i < testData.Count; i++)
            {
                foreach (var document in testData[i].JoinedDocuments)
                {
                    string category = classifyer.Classify(document);
                    int index = indexMap[category];
                    result[i, index]++;
                }
            }

            return result;
        }

        public double[,] MergeTests(double[][,] testResults)
        {
            return Utils.MergeMatricies(testResults,
                   (first, second, size) => first + (second / size));
        }
    }
}