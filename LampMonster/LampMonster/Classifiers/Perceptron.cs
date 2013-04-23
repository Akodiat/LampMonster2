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
            List<Document> allDocuments = new List<Document>(docsOfCategory);
            allDocuments.AddRange(docsNotOfCat);
            var featureMap = new Dictionary<string, double>();
            foreach (var item in this.weightVector)
            {
                featureMap.Add(item, Document.IDF(item, allDocuments));
            }




            int trainingCount = docsOfCategory.Count + docsNotOfCat.Count;
            for (int i = 0; i < iterations; i++)
            {
                //Learn from training data
                foreach (var doc in docsOfCategory)
                {
                    this.LearnDocument(true, doc, learningRate, trainingCount, featureMap);
                }
                foreach (var doc in docsNotOfCat)
                {
                    this.LearnDocument(false, doc, learningRate, trainingCount, featureMap);
                }
            }
        }

        private void LearnDocument(bool p, Document document, double learningRate, int trainingCount, Dictionary<string, double> IDFMap)
        {
            if (p != (Classify(document) >= 0)) // if p and Classify don't agree on the category
            {
				double learningFactor = p?learningRate:-learningRate;
                foreach (var feature in document)
                {
					double idf;
					IDFMap.TryGetValue(feature.Word, out idf);
                    double l = learningFactor * feature.NormalizedFrequency * idf;
                    weightVector[feature.Word] += l;
                }
            }
        }
    }
}
