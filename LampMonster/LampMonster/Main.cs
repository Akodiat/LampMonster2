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

            var naiveBayesFactory = new PerceptronFactory(200, 0.1f, 0.1f);

            var sentimentManager = new SentimentClassificationManager(testData, trainingData);
            var categorizeManager = new CategorizeClassificationManager(testData, trainingData);

            var domainResults = sentimentManager.RunTests(naiveBayesFactory);
            var categorizationResults = categorizeManager.RunTests(naiveBayesFactory);
            var mcNemarResults = McNemar.test(domainResults);

            var recall = CalculateRecall(categorizationResults);
            var precision = CalculatePrecision(categorizationResults);
            var accuracy = CalculateAccuracy(categorizationResults);

            PrintResult(classesData, domainResults, categorizationResults, mcNemarResults);

         }

        private static void PrintResult(List<ClassData> classesData, TruthTable[,] domainResults, double[,] categorizationResults, double[,] mcNemarResults)
        {
            String result = "Statistic results - "+System.DateTime.Now+"\n\n";
            result +="SENTIMENT ANALISYS\n\n\n\n";
            
            for (int i = 0; i < domainResults.GetLength(0); i++)
            {
                for (int j = 0; j < domainResults.GetLength(1); j++)
                {
                    result +=classesData[i].ClassID + "train "+classesData[j].ClassID+" test \nresult:    \t"+ domainResults[i, j];
                    result += "Accuracy "+domainResults[i, j].GetAccuracy()+
                        "\nRecall: Pos "+domainResults[i, j].GetRecall()[0]+
                        ", Neg "+domainResults[i, j].GetRecall()[1]+
                        ",\nPrecision: Pos "+domainResults[i, j].GetPercision()[0]+
                        " Neg "+domainResults[i, j].GetPercision()[1]+" \n\n";
                }
            }

            result +="CATEGORIZATION\n\n";
            
            for (int i = 0; i < classesData.Count; i++)
            {
                result += classesData[i].ClassID + "\t";
            }
            result += "\n";

            for (int i = 0; i < categorizationResults.GetLength(0); i++)
            {
                result += classesData[i].ClassID + "\t\t";
                for (int j = 0; j < categorizationResults.GetLength(1); j++)
                {
                    result += categorizationResults[i, j] +"\t";
                }
                result += "\n";
            }
            result += "\nnMcNEMAR";
            result += "\t\t";

            for (int i = 0; i < classesData.Count; i++)
            {
                result += classesData[i].ClassID + "\t";
            }
            result += "\n";

            for (int i = 0; i < mcNemarResults.GetLength(0); i++)
            {
                result += classesData[i].ClassID + "\t\t";
                for (int j = 0; j < mcNemarResults.GetLength(1); j++)
                {
                    result +=Math.Round(mcNemarResults[i, j], 3) +"\t";
                }
                result += "\n";
            }
            Form1 form = (Form1) Form1.ActiveForm;
            form.sendToDiagram(mcNemarResults, "McNemar");

            System.IO.StreamWriter file = new System.IO.StreamWriter("../../../../Documents/Resultstats/stats"+System.DateTime.Now+".txt");
            file.WriteLine(result);

            file.Close();
            
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
    }
}