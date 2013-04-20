using Quadruple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class SentimentClassificationManager : TestManager<SentimentTable[,]>
    {
        private IClassificationFactory factory;

        public SentimentClassificationManager(IClassificationFactory factory)
        {
            this.factory = factory;
        }
        public SentimentTable[,] RunPartialTests(List<ClassData> trainingData, List<ClassData> testData)
        {
            int categoryCount = testData.Count;
            var result = new SentimentTable[categoryCount, categoryCount];

            for (int i = 0; i < categoryCount; i++)
            {
                var classifyer = factory.GetClassifyer(CreateCategories(trainingData[i], testData[i]));
                for (int j = 0; j < categoryCount; j++)
                {
                    result[i, j] = SentimentClassification(classifyer, testData[j]);
                }
            }

            return result;
        }

        public SentimentTable[,] MergeTests(SentimentTable[][,] testResults)
        {
            return Utils.MergeMatricies(testResults,
                   (first, second, size) => first + (second / size));
        }

        private List<CategoryData> CreateCategories(ClassData training, ClassData test)
        {
            double negProb = (double)test.NegativeDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);
            double posProb = (double)test.PosetiveDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);

            var list = new List<CategoryData>();
            list.Add(new CategoryData("neg", negProb, training.NegativeDocuments));
            list.Add(new CategoryData("pos", posProb, training.PosetiveDocuments));

            return list;
        }

        private SentimentTable SentimentClassification(Classifyer classifyer, ClassData testData)
        {
            var table = new SentimentTable();

            int posCount = RunTests(classifyer, testData.PosetiveDocuments, "pos");
            int negCount = RunTests(classifyer, testData.NegativeDocuments, "neg");

            table.TruePosetive = posCount;
            table.FalseNegative = testData.PosetiveDocuments.Count - posCount;
            table.TrueNegative = negCount;
            table.FalsePosetive = testData.NegativeDocuments.Count - negCount;

            return table;
        }

        private int RunTests(Classifyer classifyer, List<Document> testDocuments, string correctClass)
        {
            int correctCount = 0;
            foreach (var doc in testDocuments)
            {
                var c = classifyer.Classify(doc);
                if (c == correctClass)
                    correctCount++;
            }
            return correctCount;
        }
    }
}