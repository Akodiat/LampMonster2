using Quadruple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class Main
    {
        public static void Start()
        {
            var parser = new FileParser();
            var classesData = FileManager.ExctractClassData("../../../../Documents/amazon-balanced-6cats", parser, (s) => true);


            var testData = new List<List<ClassData>>(classesData.Count);
            var trainingData = new List<List<ClassData>>(classesData.Count);
            NfoldSplit(5, testData, trainingData, classesData);

            var domainResults = Nfold(trainingData, testData, Domain);
            var categorizationResults = Nfold(trainingData, testData, Categorize);

            var recall = CalculateRecall(categorizationResults);
            var precision = CalculatePrecision(categorizationResults);
            var accuracy = CalculateAccuracy(categorizationResults);
         }

        private static double CalculateAccuracy(double[,] confusionMatrix)
        {
            double diagonal = 0;
            double total = 0;
            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                diagonal += confusionMatrix[i, i];
                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                {
                    total += confusionMatrix[i, j];
                }
            }
            return diagonal / total;
        }

        private static double[] CalculatePrecision(double[,] confusionMatrix)
        {
            var precision = new double[confusionMatrix.GetLength(0)];
            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                double cii = confusionMatrix[i, i];
                double columnValue = 0;
                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                {
                    columnValue += confusionMatrix[j, i];
                }
                precision[i] = cii / columnValue;
            }

            return precision;

        }

        private static double[] CalculateRecall(double[,] confusionMatrix)
        {
            var recall = new double[confusionMatrix.GetLength(0)];
            for (int i = 0; i < confusionMatrix.GetLength(0); i++)
            {
                double cii = confusionMatrix[i, i];
                double rowValue = 0;
                for (int j = 0; j < confusionMatrix.GetLength(1); j++)
                {
                    rowValue += confusionMatrix[i,j];
                }
                recall[i] = cii / rowValue;
            }

            return recall;
        }
        
        private static double[,] Nfold(List<List<ClassData>> trainingData, List<List<ClassData>> testData,
                                       Action<List<ClassData>, List<ClassData>, double[,]> func)
        {
            int foldTimes = trainingData.Count;
            int classCount = trainingData[0].Count;
            var result = new double[classCount, classCount];
            for (int i = 0; i < foldTimes; i++)
            {
                func(testData[i], trainingData[i], result);
            }

            for (int i = 0; i < classCount; i++)
            {
                for (int j = 0; j < classCount; j++)
                {
                    result[i, j] /= foldTimes;
                }
            }
            
            return result;
        }


        
        private static void Categorize(List<ClassData> testData, List<ClassData> trainingData, double[,] result)
        {
            var indexMap = new Dictionary<string, int>();
            for (int i = 0; i < testData.Count; i++)
            {
                indexMap[testData[i].ClassID] = i;
            }

            
            var classifyer = CreateCategorizeClassifyer(testData, trainingData);
            for (int i = 0; i < testData.Count; i++)
            {
                foreach (var document in testData[i].JoinedDocuments)
                {
                    string category = classifyer.Classify(document);
                    int index = indexMap[category];
                    result[i, index]++;
                }
            }
        }



        #region Indomain
        private static void Domain(List<ClassData> testData, List<ClassData> trainingData, double[,] result)
        {
            for (int i = 0; i < testData.Count; i++)
            {
                var classifyer = CreateDomainClassifier(trainingData[i], testData[i]);
                for (int j = 0; j < testData.Count; j++)
                {
                    result[i, j] += RunTests(classifyer, testData[j].PosetiveDocuments, "pos");
                    result[i, j] += RunTests(classifyer, testData[j].NegativeDocuments, "neg");
                }
            }
        }

        private static Classifyer CreateDomainClassifier(ClassData training, ClassData test)
        {

            Quad negProb = (Quad)test.NegativeDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);
            Quad posProb = (Quad)test.PosetiveDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);

            var negCatData = new CategoryData("neg", negProb, training.NegativeDocuments);
            var posCatData = new CategoryData("pos", posProb, training.PosetiveDocuments);

            //////////////////////////

           return CreateClassifyer(negCatData, posCatData);
        }

        private static void NfoldSplit(int n, List<List<ClassData>> testData, List<List<ClassData>> trainingData, List<ClassData> classesData)
        {
            for (int i = 0; i < n; i++)
            {
                testData.Add(new List<ClassData>());
                trainingData.Add(new List<ClassData>());

                for (int j = 0; j < classesData.Count; j++)
                {
                    ClassData c = classesData[j];

                    int negChunkSize = c.NegativeDocuments.Count / n;
                    int posChunkSize = c.PosetiveDocuments.Count / n;

                    int negStart = negChunkSize * i;
                    int posStart = posChunkSize * i;

                    var posTrainingDocs = new List<Document>();
                    var negTrainingDocs = new List<Document>();

                    var posTestDocs = new List<Document>();
                    var negTestDocs = new List<Document>();

                    SplitIntoTestAndTraining(negChunkSize, negStart, negTrainingDocs,
                                             negTestDocs, c.NegativeDocuments);
                    SplitIntoTestAndTraining(posChunkSize, posStart, posTrainingDocs, 
                                             posTestDocs, c.PosetiveDocuments);


                    trainingData[i].Add(new ClassData(c.ClassID, posTrainingDocs, negTrainingDocs));
                    testData[i].Add(new ClassData(c.ClassID, posTestDocs, negTestDocs));
                }           
            }
        }

        private static void SplitIntoTestAndTraining(int chunkSize, int start, List<Document> trainingDocs, List<Document> testDocs, List<Document> toSplit)
        {
            for (int i = 0; i < toSplit.Count; i++)
            {
                if (start <= i && start + chunkSize > i)
                    testDocs.Add(toSplit[i]);
                else
                    trainingDocs.Add(toSplit[i]);
            }
        }

        #endregion

        #region Categorize
        private static Classifyer CreateCategorizeClassifyer(List<ClassData> testData, List<ClassData> trainingData)
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

            return CreateClassifyer(list.ToArray());
        }

        #endregion

        private static int RunTests(Classifyer classifyer, List<Document> testDocuments, string correctClass)
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

        private static Classifyer CreateClassifyer(params CategoryData[] data)
        {
            return new NaiveBayesClassifyer(data.ToList(), 1);
        }
    }
}