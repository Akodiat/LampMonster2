using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class AlgoStats
    {
        public readonly string algorithmName;
        public readonly double[,] categorizationData;
        public readonly SentimentTable[,] sentimentData;
        public readonly double[][,] rawCategorizationData;
        public readonly SentimentTable[][,] rawSentimentData;

        public AlgoStats(string algorithmName,
                            double[,] categorizationData,
                            SentimentTable[,] sentimentData,
                            double[][,] rawCategorizationData,
                            SentimentTable[][,] rawSentimentData)
        {
            this.algorithmName = algorithmName;
            this.categorizationData = categorizationData;
            this.sentimentData = sentimentData;
            this.rawCategorizationData = rawCategorizationData;
            this.rawSentimentData = rawSentimentData;
        }
    }

    class StatisticsManager
    {
        List<AlgoStats> algoStats;
        Dictionary<int, string> categoryIndexMap;

        public StatisticsManager(List<string> categoryNames)
        {

            this.algoStats = new List<AlgoStats>();
            this.categoryIndexMap = new Dictionary<int, string>();
            for (int i = 0; i < categoryNames.Count; i++)
            {
                categoryIndexMap.Add(i, categoryNames[i]);
            }
        }

        public void AddAlgoStats(string algorithmName,
                            double[,] categorizationData,
                            SentimentTable[,] sentimentData,
                            double[][,] rawCategorizationData,
                            SentimentTable[][,] rawSentimentData)
        {
            algoStats.Add(new AlgoStats(algorithmName, categorizationData, sentimentData, 
                                            rawCategorizationData, rawSentimentData));
        }

        public void PrintResults(StreamWriter fileStream)
        {
            PrintSummary(fileStream);
            fileStream.Write("\r\n\r\n");
            PrintClassification(fileStream);
            fileStream.Write("\r\n\r\n");
            PrintSentiment(fileStream);
            fileStream.Write("\r\n\r\n");
            //PrintMcNemar(fileStream);
        }

        private void PrintMcNemar(StreamWriter fileStream)
        {
            throw new NotImplementedException();
        }

        private void PrintSentiment(StreamWriter fileStream)
        {
            fileStream.WriteLine("SENTIMENT ANALISYS\r\n\r\n");
            foreach (var algo in this.algoStats)
            {
                fileStream.WriteLine("ALGORITHM: " + algo.algorithmName + "\r\n");

                fileStream.WriteLine("Indomain - sentiment analisys\r\n");
                for (int i = 0; i < algo.sentimentData.GetLength(0); i++)
                {
                    string classID = categoryIndexMap[i];
                    PrintSentiment(classID, classID, algo.sentimentData[i, i], fileStream);
                }

                fileStream.WriteLine("\r\nOut of domain - sentiment analysis");
                for (int i = 0; i < algo.sentimentData.GetLength(0); i++)
                {
                    for (int j = 0; j < algo.sentimentData.GetLength(1); j++)
                    {
                        if (i == j) continue;

                        string trainID = categoryIndexMap[i];
                        string testID = categoryIndexMap[j];
                        PrintSentiment(trainID, testID, algo.sentimentData[i, j], fileStream);
                    }
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

        private void PrintClassification(StreamWriter file)
        {
            file.WriteLine("CATEGORIZATION\r\n");
            foreach (var algo in this.algoStats)
            {
                file.WriteLine("ALGORITHM: " + algo.algorithmName + "\r\n");
                file.WriteLine("\r\nTotal Accuracy: " + CalculateAccuracy(algo.categorizationData));

                file.Write("\t\t");


                for (int i = 0; i < categoryIndexMap.Count; i++)
                {
                    file.Write(categoryIndexMap[i] + "\t");
                }

                file.WriteLine();

                for (int i = 0; i < algo.categorizationData.GetLength(0); i++)
                {
                    file.Write(categoryIndexMap[i] + "\t\t");
                    for (int j = 0; j < algo.categorizationData.GetLength(1); j++)
                    {
                        file.Write(algo.categorizationData[i, j] + "\t");
                    }
                    file.WriteLine();
                }
                file.WriteLine();

                var recall = CalculateRecall(algo.categorizationData);
                var precision = CalculatePrecision(algo.categorizationData);

                for (int i = 0; i < categoryIndexMap.Count; i++)
                {
                    file.Write(categoryIndexMap[i] + "\t\t");
                    file.Write(" Precision " + precision[i]);
                    file.WriteLine(" Recall " + recall[i]);
                }
            }
            
        }

        private void PrintSummary(StreamWriter fileStream)
        {
            fileStream.WriteLine("----------------------------SUMMARY------------------------------\r\n");
            foreach (var algo in this.algoStats)
            {
                fileStream.WriteLine("ALGORITHM: " + algo.algorithmName + "\r\n");
                fileStream.WriteLine("Categorisation accuracy: " + CalculateAccuracy(algo.categorizationData));
                fileStream.WriteLine("Sentiment accuracy:");
                for (int i = 0; i < algo.sentimentData.GetLength(0); i++)
                {
                    fileStream.WriteLine(categoryIndexMap[i] + ": " + algo.sentimentData[i, i].GetAccuracy());
                }
                fileStream.WriteLine("\r\n");
            }
            fileStream.WriteLine("----------------------------end of summary------------------------------\r\n");
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
