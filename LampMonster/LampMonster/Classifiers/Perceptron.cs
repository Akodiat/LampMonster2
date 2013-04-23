using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class Perceptron : PerceptronBase
    {

        public Perceptron(string category, List<Document> docsOfCategory, List<Document> docsNotOfCat,
							 double learningRate, int iterations, double bias, List<string> vocabulary)
        : base(category, docsOfCategory, docsNotOfCat, learningRate, iterations, bias, vocabulary)
        { }

        protected override void Learn(List<Document> docsOfCategory, List<Document> docsNotOfCat, int iterations, double learningRate)
        {
            int trainingCount = docsOfCategory.Count + docsNotOfCat.Count;
            for (int i = 0; i < iterations; i++)
            {
                //Learn from training data
                foreach (var doc in docsOfCategory)
                {
                    this.LearnDocument(true, doc, learningRate, trainingCount);
                }
                foreach (var doc in docsNotOfCat)
                {
                    this.LearnDocument(false, doc, learningRate, trainingCount);
                }
            }
        }
    
        private void LearnDocument(bool p, Document document, double learningRate, int trainingCount)
        {
            if (p != (Classify(document) >= 0)) // if p and Classify don't agree on the category
            {
				double learningFactor = p?learningRate:-learningRate;
                foreach (var feature in document)
                {
                    double l = learningFactor * Math.Log(trainingCount / feature.Frequency);
                    weightVector[feature.Word] += l * l; 
                }
            }
        }
    }
}
