using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class CategorizeClassificationManager
    {
        private List<List<ClassData>> testData;
        private List<List<ClassData>> trainingData;

        private int CategoryCount
        {
            get
            {
                return testData[0].Count;
            }
        }

        private int NfoldCount
        {
            get
            {
                return testData.Count;
            }
        }

        public CategorizeClassificationManager(List<List<ClassData>> testData, 
                                               List<List<ClassData>> trainingData)
        {
            this.testData = testData;
            this.trainingData = trainingData;
        }

        public double[,] RunTests(IClassificationFactory factory)
        {
            var result = new double[NfoldCount][,];
            var tasks = new Task[NfoldCount];
            for (int i = 0; i < NfoldCount; i++)
            {
                int taskIndex = i;
                tasks[i] = new Task(() =>
                {
                    result[taskIndex] = Categorize(testData[taskIndex], trainingData[taskIndex], factory);
                });
                tasks[i].Start();
            }

            Task.WaitAll(tasks);

            var totalResult = new double[CategoryCount, CategoryCount];
            for (int k = 0; k < NfoldCount; k++)
            {
                double[,] partialResult = result[k];
                for (int i = 0; i < CategoryCount; i++)
                {
                    for (int j = 0; j < CategoryCount; j++)
                    {
                        totalResult[i, j] += partialResult[i, j] / NfoldCount;
                    }
                }
            }

            return totalResult;
        }


        private double[,] Categorize(List<ClassData> testData, List<ClassData> trainingData, IClassificationFactory factory)
        {
            var result = new double[CategoryCount, CategoryCount];
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
                                         (Quadruple.Quad)item.JoinedCount / totalDocs,
                                         item.JoinedDocuments));
            }

            return list;
        }
    }
}