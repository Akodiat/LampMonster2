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

            NfoldCategorize(10, 100, classesData); 
            NfoldIndomain(10, 100, classesData);
         }

        #region Indomain
        private static void NfoldIndomain(int n, int trainSize, List<ClassData> classesData)
        {

            var accuracies = new double[classesData.Count];

            for (int i = 0; i < n; i++)
            {
                List<ClassData> testData = new List<ClassData>();
                List<ClassData> trainingData = new List<ClassData>();

                Split(classesData, testData, trainingData, trainSize);
                Indomain(trainingData, testData, accuracies);
            }

            for (int i = 0; i < classesData.Count; i++)
            {
                Console.WriteLine("Accuracy indomain: algorithm={0}, trainingSize={1}, \n"
                    + " category={2} is: {3}", "NaiveBayes", trainSize, classesData[i].ClassID, accuracies[i] / n);             
            }
        }

        private static void Indomain(List<ClassData> trainingData, List<ClassData> testData, double[] accuracies)
        {
            //Change this code since it's wierd.
            for (int i = 0; i < trainingData.Count; i++)
            {
                accuracies[i] += IndomainClass(trainingData[i], testData[i]);
            }
        }

        private static double IndomainClass(ClassData training, ClassData test)
        {
            /////This step is will be fixed moved what ever

            Quad negProb = (Quad)test.NegativeDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);
            Quad posProb = (Quad)test.PosetiveDocuments.Count /
                           (test.NegativeDocuments.Count + test.PosetiveDocuments.Count);

            var negCatData = new CategoryData("neg", negProb, training.NegativeDocuments);
            var posCatData = new CategoryData("pos", posProb, training.PosetiveDocuments);

            //////////////////////////

            var classifyer = CreateClassifyer(negCatData, posCatData);
            int correctPos = RunTests(classifyer, test.PosetiveDocuments, "pos");
            int correctNeg = RunTests(classifyer, test.NegativeDocuments, "neg");

            return (double)(correctNeg + correctPos) /
                       (test.PosetiveDocuments.Count + test.NegativeDocuments.Count);
        }

        #endregion

        #region Categorize



        public static void NfoldCategorize(int n, int trainSize, List<ClassData> classesData)
        {
            var accuracies = new double[classesData.Count];
            for (int i = 0; i < n; i++)
            {
                List<ClassData> testData = new List<ClassData>();
                List<ClassData> trainingData = new List<ClassData>();

                Split(classesData, testData, trainingData, trainSize);
                Categorize(testData, trainingData, accuracies);
            }

            for (int i = 0; i < classesData.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Red;  
                Console.WriteLine("Accuracy categorize: algorithm={0}, trainingSize={1}, \n"
                    + " category={2} is: {3}", "NaiveBayes", trainSize, classesData[i].ClassID, accuracies[i] / n);
            }
        }

        private static void Categorize(List<ClassData> testData, List<ClassData> trainingData, double[] accuracies)
        {
            var classifyer = CreateCategorizeClassifyer(testData, trainingData);

            for (int i = 0; i < testData.Count; i++)
            {
                int correctCount = RunTests(classifyer, testData[i].PosetiveDocuments, testData[i].ClassID);
                correctCount += RunTests(classifyer, testData[i].NegativeDocuments, testData[i].ClassID);

                accuracies[i] += (double)(correctCount) / (testData[i].PosetiveDocuments.Count + testData[i].NegativeDocuments.Count);
            }
        }



        private static Classifyer CreateCategorizeClassifyer(List<ClassData> testData, List<ClassData> trainingData)
        {
            var list = new List<CategoryData>();

            //We need this to calculate P(C) 
            int totalDocs = 0;
            foreach (var item in testData)
                totalDocs += item.PosetiveDocuments.Count + item.NegativeDocuments.Count;

            foreach (var item in trainingData)
            {
                var trainingDocs = new List<List<string>>();
                trainingDocs.AddRange(item.PosetiveDocuments);
                trainingDocs.AddRange(item.NegativeDocuments);

                list.Add(new CategoryData(item.ClassID,
                                          (Quadruple.Quad)trainingDocs.Count / totalDocs,
                                          trainingDocs));
            }

            return CreateClassifyer(list.ToArray());
        }


        #endregion

        private static void Split(List<ClassData> classesData, List<ClassData> testData, List<ClassData> trainingData, int sizeOfTrainData)
        {
            foreach (var classData in classesData)
            {
                var posDocs = Utils.CopyShuffle(classData.PosetiveDocuments);
                var negDocs = Utils.CopyShuffle(classData.NegativeDocuments);

                var trainingPosData = new List<List<string>>();
                var trainingNegData = new List<List<string>>();
                var testPosData = new List<List<string>>();
                var testNegData = new List<List<string>>();

                for (int i = 0; i < sizeOfTrainData; i++)
                {
                    trainingPosData.Add(posDocs[i]);
                    trainingNegData.Add(negDocs[i]);
                }

                for (int i = sizeOfTrainData; i < posDocs.Count; i++)
                    testPosData.Add(posDocs[i]);

                for (int i = sizeOfTrainData; i < negDocs.Count; i++)
                    testNegData.Add(negDocs[i]);

                testData.Add(new ClassData(classData.ClassID, testPosData, testNegData));
                trainingData.Add(new ClassData(classData.ClassID, trainingPosData, trainingNegData));
            }
        }


        private static int RunTests(Classifyer classifyer, List<List<string>> testDocuments, string correctClass)
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
