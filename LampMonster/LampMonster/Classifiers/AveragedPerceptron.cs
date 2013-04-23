using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class AveragedPerceptron : PerceptronBase
    {
        public AveragedPerceptron(string category, List<Document> docsOfCategory, List<Document> docsNotOfCat,
                             double learningRate, int iterations, double bias, List<string> vocabulary)
            : base (category, docsOfCategory, docsNotOfCat, learningRate, iterations, bias, vocabulary)
        { }

        protected override void Learn(List<Document> docsOfCategory, List<Document> docsNotOfCat, int iterations, double learningRate, Dictionary<string, double> IDFMap)
        {
            var w = new VeightVector(this.weightVector);

            int trainingSize = docsOfCategory.Count + docsNotOfCat.Count;
            for (int i = 0; i < iterations; i++)
            {
                //Learn from training data
                foreach (var doc in docsOfCategory)
                {
                    this.Learn(true, doc, w, i, iterations, trainingSize, learningRate, IDFMap);
                }
                foreach (var doc in docsNotOfCat)
                {
                    this.Learn(false, doc, w, i, iterations, trainingSize, learningRate, IDFMap);
                }
            }
        }

        private void Learn(bool p, Document doc, VeightVector w, int counter, int iterations, int trainingSize, double learningRate, Dictionary<string, double> IDFMap)
        {
			bool guess = Classify(doc, w)>=0;
			double learningFactor = p ? learningRate : -learningRate;
            if (p != guess) // if p and Classify don't agree on the category
            {
                double average_weight = (double)(trainingSize * iterations - counter) / (trainingSize * iterations);
                foreach (var feature in doc)
                {
                    double idf;
                    IDFMap.TryGetValue(feature.Word, out idf);
                    double learned = learningFactor * feature.NormalizedFrequency * idf;
                    w[feature.Word] += learned;
                    weightVector[feature.Word] += average_weight * learned;
                }
            }
        }

        public double Classify(Document doc, VeightVector w)
        {
            return Bias + w.DotProduct(doc);
        }
    }
}
