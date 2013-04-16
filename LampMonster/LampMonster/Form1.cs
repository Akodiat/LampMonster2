using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace LampMonster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private bool WordFilter(string toFilter)
        {
            return true; //Add aditional word filtering here.
        }


        private void button1_Click(object sender, EventArgs e)
        {

            Main.Start();


           /* string root = "../../../../Documents/amazon-balanced-6cats";

            var categories = new List<string>();
            categories.Add("books");
            categories.Add("camera");
            categories.Add("DVD");
            categories.Add("music");
            categories.Add("health");
            categories.Add("software");

            var fileParser = new FileParser();

            string mode = (string)comboBox1.SelectedItem;


            var watch = Stopwatch.StartNew();

            var pathFinder = new OrderedPathFinder(600, 100);
            Indomain(root, categories, pathFinder, fileParser);
            Crategorize(root, categories, pathFinder, fileParser);
            OutOfDomain(root, categories, pathFinder, fileParser);


            watch.Stop();

            Console.WriteLine("Running time was: " + watch.Elapsed.Seconds + " seconds.");*/
        }


        private void Crategorize(string root, List<string> categories, OrderedPathFinder finder, FileParser fileParser)
        {
            var trainingMap = new Dictionary<string, List<string>>();
            var testMap = new Dictionary<string, List<string>>();

            foreach (var category in categories)
            {
                var trainingList = new List<string>();
                trainingList.AddRange(GetTrainingPaths(root, category, "pos", finder));
                trainingList.AddRange(GetTrainingPaths(root, category, "neg", finder));
                trainingMap.Add(category, trainingList);

                var testList = new List<string>();
                testList.AddRange(GetTestPaths(root, category, "pos", finder));
                testList.AddRange(GetTestPaths(root, category, "neg", finder));
                testMap.Add(category, testList);
            }

            var trainingData = GetCategoryData(trainingMap, fileParser);
            var classifyer = GetClassifyer(trainingData);

            foreach (var test in testMap)
            {
                int correctCount = RunTests(classifyer, test.Value, test.Key, fileParser);
                Console.WriteLine("Correct times on " + test.Key + " is: " + correctCount);
            }
        }

        private void OutOfDomain(string root, List<string> categories, IPathFinder pathFinder, FileParser fileParser)
        {
            foreach (var category in categories)
            {
                var trainingMap = GetPosNegTrainingMap(root, category, pathFinder);

                var trainingData = GetCategoryData(trainingMap, fileParser);
                var classifyer = GetClassifyer(trainingData);

                foreach (var otherCategory in categories)
                {
                    if (otherCategory == category)
                        continue;

                    Console.WriteLine("Starting out of domain testing on {0} with classifyer from class {1}", otherCategory, category);

                    int correctPosCount = RunTests(classifyer, GetTestPaths(root, otherCategory, "pos", pathFinder), "pos", fileParser);
                    int correctNegCount = RunTests(classifyer, GetTestPaths(root, otherCategory, "neg", pathFinder), "neg", fileParser);

                    Console.WriteLine("Correct pos for {0} is: {1}", otherCategory, correctPosCount);
                    Console.WriteLine("Correct neg for {0} is: {1}", otherCategory, correctNegCount);
                    Console.WriteLine("Correct total for {0} is: {1}", otherCategory, correctPosCount + correctNegCount);
                }                

            }
        }

        private void Indomain(string root, List<string> categories, IPathFinder pathFinder, FileParser fileParser)
        {
            foreach (var category in categories)
            {
                var trainingMap = GetPosNegTrainingMap(root, category, pathFinder);
                
                var posTests = GetTestPaths(root, category, "pos", pathFinder);
                var negTests = GetTestPaths(root, category, "neg", pathFinder);

                var trainingData = GetCategoryData(trainingMap, fileParser);
                var classifyer = GetClassifyer(trainingData);


                int correctPosCount = RunTests(classifyer, posTests, "pos", fileParser);
                int correctNegCount = RunTests(classifyer, negTests, "neg", fileParser);

                Console.WriteLine("Correct pos for {0} is: {1}", category, correctPosCount);
                Console.WriteLine("Correct neg for {0} is: {1}", category, correctNegCount);
                Console.WriteLine("Correct total for {0} is: {1}", category, correctPosCount + correctNegCount);
            }
        }

        private List<CategoryData> GetCategoryData(Dictionary<string, List<string>> trainingMap, FileParser parser)
        {
            var list = new List<CategoryData>();
            
            //We need this to calculate P(C) 
            int totalFiles = 0;
            foreach (var item in trainingMap)
                totalFiles += item.Value.Count;
                    
            foreach (var item in trainingMap)
            {
                var trainingDocuments = GetFilesAsWords(item.Value, parser);
                list.Add(new CategoryData(item.Key,
                                          (Quadruple.Quad)item.Value.Count / totalFiles,
                                          trainingDocuments));
            }

            return list;
        }

        private int RunTests(Classifyer classifyer, List<string> testPaths, string correctCategory, FileParser parser)
        {
            int correctCount = 0;
            foreach (var file in testPaths)
            {
                var cat = classifyer.Classify(parser.GetWordsInFile(file, WordFilter));
                if (cat == correctCategory)
                    correctCount++;
            }
            return correctCount;
        }

        private Dictionary<string, List<string>> GetPosNegTrainingMap(string root, string category, IPathFinder finder)
        {
            var posTrainingPaths = GetTrainingPaths(root, category, "pos", finder);
            var negTrainingPaths = GetTrainingPaths(root, category, "neg", finder);
            var trainingMap = new Dictionary<string, List<string>>();
            trainingMap.Add("pos", posTrainingPaths);
            trainingMap.Add("neg", negTrainingPaths);

            return trainingMap;
        }
        
        private List<string> GetTestPaths(string root, string category, string posNeg, IPathFinder finder)
        {
            var path = root + "/" + category + "/" + posNeg;
            return finder.GetProcessingPaths(path);
        }

        private List<string> GetTrainingPaths(string root, string category, string posNeg, IPathFinder finder)
        {
            var path = root + "/" + category + "/" + posNeg;
            return finder.GetTrainingPaths(path);
        }

        private List<List<string>> GetFilesAsWords(List<string> files, FileParser parser)
        {
            var words = new List<List<string>>();
            foreach (var file in files)
                words.Add(parser.GetWordsInFile(file, WordFilter));

            return words;
        }

        private Classifyer GetClassifyer(List<CategoryData> trainingData)
        {
            return new NaiveBayesClassifyer(trainingData, 1);
        }
    }
}
