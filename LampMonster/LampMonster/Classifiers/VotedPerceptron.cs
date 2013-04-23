using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class VotedPerceptron : IPerceptron
    {
        public string Category { get; private set; }

        List<VeightVector> vectors;
        List<int> aliveCounts;


        public VotedPerceptron(string category, List<Document> docsOfCategory, List<Document> docsNotOfCat,
							 double learningRate, int iterations, List<string> vocabulary)
        {
            this.Category = category;
            this.vectors = new List<VeightVector>();
            Learn(vocabulary, docsOfCategory, docsNotOfCat, iterations, learningRate);
        }

        protected void Learn(List<string> vocabulary, List<Document> docsOfCategory, List<Document> docsNotOfCat, int iterations, double learningRate)
        {
            vectors = new List<VeightVector>();
            aliveCounts = new List<int>();

            var activeVector = new VeightVector(vocabulary);

            int trainingCount = docsNotOfCat.Count + docsOfCategory.Count;
            int count = 0;
            for (int i = 0; i < iterations; i++)
            {
                //Learn from training data
                foreach (var doc in docsOfCategory)
                {
                    if (Classify(doc, activeVector) < 0)
                    {
                        vectors.Add(activeVector);
                        aliveCounts.Add(count);
                        count = 0;

                        activeVector = new VeightVector(activeVector);
                        Learn(activeVector, doc, true, learningRate, trainingCount);
                    }
                    count++;
                }

                foreach (var doc in docsNotOfCat)
                {
                    if (Classify(doc, activeVector) >= 0)
                    {
                        vectors.Add(activeVector);
                        aliveCounts.Add(count);
                        count = 0;

                        activeVector = new VeightVector(activeVector);
                        Learn(activeVector, doc, false, learningRate, trainingCount);

                    }
                    count++;
                }
            }

            vectors.Add(activeVector);
            aliveCounts.Add(count);

        }

        private void Learn(VeightVector activeVector, Document doc, bool p, double learningRate, int trainingCOunt)
        {
            double learningFactor = p ? learningRate : -learningRate;
            foreach (var feature in doc)
            {
                activeVector[feature.Word] += learningFactor * Math.Log(trainingCOunt / feature.Frequency);
            }
        }

        private double Classify(Document doc, VeightVector activeVector)
        {
            return activeVector.DotProduct(doc);
        }

        public double Classify(Document document)
        {
            double sum = 0;
            for (int i = 0; i < vectors.Count; i++)
            {
                sum += aliveCounts[i] * Math.Sign(vectors[i].DotProduct(document));
            }

            return sum;
        }
    }
}
