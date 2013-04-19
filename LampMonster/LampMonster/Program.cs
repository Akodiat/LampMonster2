using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LampMonster
{
    static class Program
    {
        static void Main()
        {
            var parser = new FileParser();
            var classesData = FileManager.ExctractClassData("../../../../Documents/amazon-balanced-6cats", parser, (s) => true);

            var testData = new List<List<ClassData>>(classesData.Count);
            var trainingData = new List<List<ClassData>>(classesData.Count);
            var splitter = new NSplitter(5, 1.0);

            splitter.NfoldSplit(testData, trainingData, classesData);

            var factory = new NaiveBayesFactory(1);

            var sentimentManager = new SentimentClassificationManager(factory);
            var categorizeManager = new CategorizeClassificationManager(factory);

            var domainResults = NfoldTest(sentimentManager, trainingData, testData);
            var categorizationResults = NfoldTest(categorizeManager, trainingData, testData);
            var mcNemarResults = McNemar.test(domainResults);

            var recall = CalculateRecall(categorizationResults);
            var precision = CalculatePrecision(categorizationResults);
            var accuracy = CalculateAccuracy(categorizationResults);

            PrintResult(classesData, domainResults, categorizationResults, mcNemarResults);
        }

        private static Result NfoldTest<Result>(TestManager<Result> testManager,
                                                List<List<ClassData>> trainingData,
                                                List<List<ClassData>> testData)
        {
            int nfoldCount = trainingData.Count;
            var result = new Result[nfoldCount];
            var tasks = new Task[nfoldCount];
            for (int i = 0; i < nfoldCount; i++)
            {
                int taskIndex = i;
                tasks[i] = new Task(() =>
                {
                    result[taskIndex] = testManager.RunPartialTests(trainingData[taskIndex], testData[taskIndex]);
                    Console.WriteLine(".");
                });
                tasks[i].Start();
            }

            Task.WaitAll(tasks);
            return testManager.MergeTests(result);
        }

        private static void PrintResult(List<ClassData> classesData, TruthTable[,] domainResults, double[,] categorizationResults, double[,] mcNemarResults)
        {
            using (StreamWriter file = new System.IO.StreamWriter("../../../../Documents/Resultstats/stats" + DateTime.Now.ToLongDateString() + ".txt"))
            {
                file.WriteLine("Statistic Results - " + DateTime.Now + "\n");
                file.WriteLine("SENTIMENT ANALISYS\n\n\n\n");

                for (int i = 0; i < domainResults.GetLength(0); i++)
                {
                    for (int j = 0; j < domainResults.GetLength(1); j++)
                    {
                       file.WriteLine(classesData[i].ClassID + " train " + classesData[j].ClassID + " test");
                       file.WriteLine(domainResults[i, j]);
                       file.WriteLine("Accuracy " + domainResults[i, j].GetAccuracy());
                       file.WriteLine("Recall: Pos " + domainResults[i, j].GetRecall()[0] + 
                                      ", Neg " + domainResults[i, j].GetRecall()[1]);
                       file.WriteLine("Precision: Pos " + domainResults[i, j].GetPercision()[0] +
                                      " Neg " + domainResults[i, j].GetPercision()[1] + " \n\n");
                    }
                }

                file.WriteLine("CATEGORIZATION\n\n");

                for (int i = 0; i < classesData.Count; i++)
                {
                    file.Write(classesData[i].ClassID + "\t");
                }

                file.WriteLine();

                for (int i = 0; i < categorizationResults.GetLength(0); i++)
                {
                    file.Write(classesData[i].ClassID + "\t\t");
                    for (int j = 0; j < categorizationResults.GetLength(1); j++)
                    {
                        file.Write(categorizationResults[i, j] + "\t");
                    }
                    file.WriteLine();
                }
                file.WriteLine("\nnMcNEMAR");
                file.Write("\t\t");

                for (int i = 0; i < classesData.Count; i++)
                {
                    file.Write(classesData[i].ClassID + "\t");
                }
                file.WriteLine();

                for (int i = 0; i < mcNemarResults.GetLength(0); i++)
                {
                    file.WriteLine(classesData[i].ClassID + "\t\t");
                    for (int j = 0; j < mcNemarResults.GetLength(1); j++)
                    {
                       file.Write(Math.Round(mcNemarResults[i, j], 3) + "\t");
                    }
                    file.Write("\n");
                }
            }
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
                    rowValue += confusionMatrix[i, j];
                }
                recall[i] = cii / rowValue;
            }

            return recall;
        }
    }
}
