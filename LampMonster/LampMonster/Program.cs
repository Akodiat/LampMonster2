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

            var trainingCoverage = 1;

            var testData = new List<List<ClassData>>(classesData.Count);
            var trainingData = new List<List<ClassData>>(classesData.Count);
            var splitter = new NSplitter(5, trainingCoverage);

            splitter.NfoldSplit(testData, trainingData, classesData);

            var factory = new BinaryNaiveBayesFactory(1);

            var sentimentManager = new SentimentClassificationManager(factory);
            var categorizeManager = new CategorizeClassificationManager(factory);

            var domainResults = NfoldTest(sentimentManager, trainingData, testData);
            var categorizationResults = NfoldTest(categorizeManager, trainingData, testData);

            var recall = CalculateRecall(categorizationResults);
            var precision = CalculatePrecision(categorizationResults);
            var accuracy = CalculateAccuracy(categorizationResults);

            PrintResult(classesData, domainResults, categorizationResults, factory.ClassifyerDesc(), trainingCoverage);
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

        private static void PrintResult(List<ClassData> classesData, SentimentTable[,] domainResults, double[,] categorizationResults, 
                                        string algorithm, double trainingSetCoverage)
        {
            using (StreamWriter file = new System.IO.StreamWriter("../../../../Documents/Resultstats/" +
                  DateTime.Now.ToShortDateString() + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + ".txt"))
            {
                file.WriteLine("Statistic Results - " + DateTime.Now + "\n");
                file.WriteLine("Algorithm {0} \r\nTrainingSetCoverage {1}", algorithm, trainingSetCoverage);
                file.WriteLine();

                PrintSentimentResults(classesData, domainResults, file);
                PrintCategorizationResults(classesData, categorizationResults, file);
            }
        }

        private static void PrintCategorizationResults(List<ClassData> classesData, double[,] categorizationResults, StreamWriter file)
        {
            file.WriteLine("CATEGORIZATION\r\n");
            file.WriteLine("\r\nTotal Accuracy: " + CalculateAccuracy(categorizationResults));
            
            file.Write("\t\t");


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
            file.WriteLine();

            var recall = CalculateRecall(categorizationResults);
            var precision = CalculatePrecision(categorizationResults);

            for (int i = 0; i < classesData.Count; i++)
            {
                file.Write(classesData[i].ClassID + "\t\t");
                file.Write(" Precision " + precision[i]);
                file.WriteLine(" Recall " + recall[i]);
            }
        }

        private static void PrintSentimentResults(List<ClassData> classesData, SentimentTable[,] domainResults, StreamWriter file)
        {

            file.WriteLine("SENTIMENT ANALISYS\r\n\r\n");


            file.WriteLine("Indomain - sentiment analisys\r\n");
            for (int i = 0; i < domainResults.GetLength(0); i++)
            {
                string classID = classesData[i].ClassID;
                PrintSentiment(classID, classID, domainResults[i, i], file);
            }

            file.WriteLine("\r\nOut of domain - sentiment analysis");
            for (int i = 0; i < domainResults.GetLength(0); i++)
            {
                for (int j = 0; j < domainResults.GetLength(1); j++)
                {
                    if (i == j) continue;

                    string trainID = classesData[i].ClassID;
                    string testID = classesData[j].ClassID;
                    PrintSentiment(trainID, testID, domainResults[i, j], file);
                }
            }
        }

        private static void PrintSentiment(string trainId, string testID, SentimentTable sentiment, StreamWriter file)
        {
            file.WriteLine(trainId + " train " + testID + " test");
            file.WriteLine(sentiment);
            file.WriteLine("Accuracy " + sentiment.GetAccuracy());
            file.WriteLine("Recall: Pos " + sentiment.GetRecall()[0] +
                           ", Neg " + sentiment.GetRecall()[1]);
            file.WriteLine("Precision: Pos " + sentiment.GetPercision()[0] +
                           " Neg " + sentiment.GetPercision()[1]);
            file.WriteLine("McNemar: " + sentiment.GetMcNemar());

            file.WriteLine();
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
