using Quadruple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class SentimentClassificationManager
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

        public SentimentClassificationManager(List<List<ClassData>> testData,
                                              List<List<ClassData>> trainingData)
        {
            this.testData = testData;
            this.trainingData = trainingData;
        }


        public TruthTable[,] RunTests(IClassificationFactory classificationFactory)
        {
            var result = new TruthTable[CategoryCount, CategoryCount];
            for (int i = 0; i < NfoldCount; i++)
            {
                Classify(testData[i], trainingData[i], result, classificationFactory);
            }

            for (int i = 0; i < CategoryCount; i++)
            {
                for (int j = 0; j < CategoryCount; j++)
                {
                    result[i, j] /= NfoldCount;
                }
            }

            return result;
        }

        private void Classify(List<ClassData> testData, List<ClassData> trainingData,
                                   TruthTable[,] result, IClassificationFactory factory)
        {
            for (int i = 0; i < testData.Count; i++)
            {
                var classifyer = factory.GetClassifyer(CreateCategories(trainingData[i], testData[i]));
                for (int j = 0; j < testData.Count; j++)
                {
                    result[i, j] += SentimentClassification(classifyer, testData[j]);
                }
            }
        }
        
        private List<CategoryData> CreateCategories(ClassData training, ClassData test)
        {
            Quad negProb = (Quad)test.NegativeDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);
            Quad posProb = (Quad)test.PosetiveDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);

            var list = new List<CategoryData>();
            list.Add(new CategoryData("neg", negProb, training.NegativeDocuments));
            list.Add(new CategoryData("pos", posProb, training.PosetiveDocuments));

            return list;
        }

        private TruthTable SentimentClassification(Classifyer classifyer, ClassData testData)
        {
            var table = new TruthTable();

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