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

        private void button1_Click(object sender, EventArgs e)
        {
            string root = "C:/Users/Lukas/Documents/GitHub/LampMonster/Documents/amazon-balanced-6cats";

            var categories = new List<string>();
            categories.Add("books");
            categories.Add("camera");
            categories.Add("DVD");
            categories.Add("music");
            categories.Add("health");
            categories.Add("software");

            var pathFinder = new OrderedPathFinder(500, 400);
            var fileParser = new FileParser();

            foreach (var item in categories)
            {
                var posPath = root + "/" + item + "/pos";
                var negPath = root + "/" + item + "/neg";

                var pos = pathFinder.GetTrainingPaths(posPath);
                var neg = pathFinder.GetTrainingPaths(negPath);

                var map = new Dictionary<string, List<string>>();
                map.Add("pos", GetMegaDocument(pos, fileParser));
                map.Add("neg", GetMegaDocument(neg, fileParser));

                var classifyer = new NaiveBayesClassifyer(map, 1);

                var pos2 = pathFinder.GetProcessingPaths(posPath);
                var neg2 = pathFinder.GetProcessingPaths(negPath);

                int correctPosCount = 0;

                foreach (var document in pos2)
                {
                    var category = classifyer.Classify(fileParser.GetWordsInFile(document, (s) => true));
                    if (category == "pos")
                    {
                        correctPosCount++;
                    }
                }
                
                Console.WriteLine("Correct pos for category " + item + " is: " + correctPosCount);
                int correctNegCount = 0;

                foreach (var document in neg2)
                {
                    var category = classifyer.Classify(fileParser.GetWordsInFile(document, (s) => true));
                    if (category == "neg")
                        correctNegCount++;
                }

                Console.WriteLine("Correct pos neg category " + item + " is: " + correctNegCount);
                Console.WriteLine("Total correct for " + item + " is: " + (correctPosCount + correctNegCount));           
            }
       }

        private List<string> GetMegaDocument(List<string> files, FileParser parser)
        {
            double dummy;
            var words = new List<string>();
            foreach (var file in files)
                words.AddRange(parser.GetWordsInFile(file, (s) => !double.TryParse(s, out dummy)));

            return words;
        }
        
    }
}
