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

namespace LampMonster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string root = "C:/Users/Lukas/Documents/GitHub/LampMonster/Documents/amazon-balanced-6cats";

            var categories = new List<string>();
            categories.Add("books");
            categories.Add("camera");
            categories.Add("DVD");
            categories.Add("music");
            categories.Add("health");
            categories.Add("software");

            var pathFinder = new OrderedPathFinder(400, 400);
            var fileParser = new FileParser();

            string mode = (string)comboBox1.SelectedItem;

            if (mode == "Indomain")
                Indomain(root, categories, pathFinder, fileParser);
            else if (mode  == "Categorize")
                OutOfDomain(root, categories, pathFinder, fileParser);
            else
                Crategorize(root, categories, pathFinder, fileParser);

        }


        private void Crategorize(string root, List<string> categories, OrderedPathFinder pathFinder, FileParser fileParser)
        {
            throw new NotImplementedException();
        }

        private void OutOfDomain(string root, List<string> categories, OrderedPathFinder pathFinder, FileParser fileParser)
        {
            var map = new Dictionary<string, List<string>>();
            foreach (var item in categories)
            {
                var posPath = root + "/" + item + "/pos";
                var negPath = root + "/" + item + "/neg";

                var pos = pathFinder.GetTrainingPaths(posPath);
                var neg = pathFinder.GetTrainingPaths(negPath);

                var categoryList = new List<string>();
                categoryList.AddRange(GetMegaDocument(pos, fileParser));
                categoryList.AddRange(GetMegaDocument(neg, fileParser));
                map.Add(item, categoryList);
            }


            var classifyer = new NaiveBayesClassifyer(map, 1);


            foreach (var item in categories)
            {
                var posPath = root + "/" + item + "/pos";
                var negPath = root + "/" + item + "/neg";

                var pos = pathFinder.GetProcessingPaths(posPath);
                var neg = pathFinder.GetProcessingPaths(negPath);

                var testSet  = new List<string>();
                testSet.AddRange(pos);
                testSet.AddRange(neg);

                int correctCount = 0;
                foreach (var file in testSet)
                {
                    var category = classifyer.Classify(fileParser.GetWordsInFile(file, (s) => true));
                    if (category == item)
                        correctCount++;
                }

                Console.WriteLine("Correct times on " + item + " is: " + correctCount);
            }


        }

        private void Indomain(string root, List<string> categories, OrderedPathFinder pathFinder, FileParser fileParser)
        {

            foreach (var item in categories)
            {
                var posPath = root + "/" + item + "/pos";
                var negPath = root + "/" + item + "/neg";

                var pos = pathFinder.GetTrainingPaths(posPath);
                var neg = pathFinder.GetTrainingPaths(negPath);

                var map = new Dictionary<string, List<string>>();
                map.Add("pos", GetMegaDocument(pos, fileParser));
                map.Add("neg", GetMegaDocument(neg, fileParser));

                var classifyer = new NaiveBayesClassifyer(map, 2);

                var pos2 = pathFinder.GetProcessingPaths(posPath);
                var neg2 = pathFinder.GetProcessingPaths(negPath);

                int correctPosCount = 0;

                foreach (var document in pos2)
                {
                    var category = classifyer.Classify(fileParser.GetWordsInFile(document, (s) => true));
                    if (category == "pos")
                        correctPosCount++;
                }

                Console.WriteLine("Correct pos for category " + item + " is: " + correctPosCount);
                int correctNegCount = 0;

                foreach (var document in neg2)
                {
                    var category = classifyer.Classify(fileParser.GetWordsInFile(document, (s) => true));
                    if (category == "neg")
                        correctNegCount++;
                }

                Console.WriteLine("Correct neg for category " + item + " is: " + correctNegCount);
                Console.WriteLine("Total correct for " + item + " is: " + (correctPosCount + correctNegCount));
            }
        }

        private List<string> GetMegaDocument(List<string> files, FileParser parser)
        {
            var words = new List<string>();
            foreach (var file in files)
                words.AddRange(parser.GetWordsInFile(file, (s) => true));

            return words;
        }
    }
}
