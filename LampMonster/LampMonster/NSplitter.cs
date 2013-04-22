using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class CrossValidation
    {
        private int foldCount;
        private double percentTrain;
        private List<ClassData> classesData;
        private List<List<ClassData>> trainingData;
        private List<List<ClassData>> testData;

        public CrossValidation(int foldCount, double percentTrain, List<ClassData> classesData)
        {
            if (foldCount < 1)
                throw new ArgumentException("Must fold more then once!");
            if (percentTrain <= 0 || percentTrain > 1)
                throw new ArgumentException("Percent train must be below 0 and 1 exclusive");
            if (classesData == null)
                throw new NullReferenceException("classesData");

            this.foldCount = foldCount;
            this.percentTrain = percentTrain;
            this.classesData = classesData;

            this.trainingData = new List<List<ClassData>>();
            this.testData = new List<List<ClassData>>();
            this.NfoldSplit();
        }

        private void NfoldSplit()
        {
            for (int i = 0; i < foldCount; i++)
            {
                testData.Add(new List<ClassData>());
                trainingData.Add(new List<ClassData>());

                for (int j = 0; j < classesData.Count; j++)
                {
                    ClassData c = classesData[j];

                    int negChunkSize = c.NegativeDocuments.Count / foldCount;
                    int posChunkSize = c.PosetiveDocuments.Count / foldCount;

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

        private void SplitIntoTestAndTraining(int chunkSize, int start, 
                                              List<Document> trainingDocs,
                                              List<Document> testDocs, 
                                              List<Document> toSplit)
        {
            int trainingDocMax = (int)((toSplit.Count - chunkSize) * percentTrain);
            int trainingDocCount = 0;
            for (int i = 0; i < toSplit.Count; i++)
            {
                if (start <= i && start + chunkSize > i)
                    testDocs.Add(toSplit[i]);
                else if(trainingDocCount++ < trainingDocMax)
                    trainingDocs.Add(toSplit[i]);
            }
        }

        public Result Compute<Result>(TestManager<Result> testManager)
        {
            var result = new Result[foldCount];
            var tasks = new Task[foldCount];
            for (int i = 0; i < foldCount; i++)
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

    }
}
