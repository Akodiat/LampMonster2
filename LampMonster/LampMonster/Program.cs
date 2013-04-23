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
        private static readonly IClassificationFactory[] factories = {
                                                 //new KNNFactory(2000,20),
                                                 //new PerceptronFactory(8000, 100, 0.05, -1, PerceptronType.Normal),
                                                 //new PerceptronFactory(8000, 100, 0.05, -1, PerceptronType.Awereged),
                                                 new BinaryNaiveBayesFactory(1),
                                                 new NaiveBayesFactory(1)
                                             };
        static void Main()
        {
            var parser = new FileParser("stopwords.txt", '.', ' ', ',', '\n', '\r', '"', '(', ')','[', ']', '$', '!');
            var classesData = FileManager.ExctractClassData("../../../../Documents/amazon-balanced-6cats", parser);

            var trainingCoverage = 1;
            var failLog = new FailLog();
            var validator = new CrossValidation(10, trainingCoverage, classesData);
            var statistics = new StatisticsManager(GetCategoryNames(classesData));
            foreach (var factory in factories)
            {
                var sentimentTests = new SentimentClassificationManager(factory, failLog);
                var catTests = new CategorizeClassificationManager(factory);

                var sentimentResults = validator.Compute(sentimentTests);
                var categorizationResults = validator.Compute(catTests);

                statistics.AddAlgoStats(factory.ClassifyerDesc(), catTests.MergeTests(categorizationResults), 
                                sentimentTests.MergeTests(sentimentResults), categorizationResults, sentimentResults);
            }
            using (var fileStream = OpenFileStream())
            {
                statistics.PrintResults(fileStream);
            }
        }

        private static List<string> GetCategoryNames(List<ClassData> classesData)
        {
            var catNames = new List<string>();
            foreach (var name in classesData)
            {
                catNames.Add(name.ClassID);
            }
            return catNames;
        }

        private static StreamWriter OpenFileStream()
        {
            StreamWriter file = new System.IO.StreamWriter("../../../../Documents/Resultstats/" +
                  DateTime.Now.ToShortDateString() + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + ".txt");
            return file;
        }
    }
}
