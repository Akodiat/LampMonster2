using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class Perceptron
    {
        private Dictionary<string, double> weightVector; //Implemented as a map since it is simpler
        private readonly string category;
        public string Category
        {
            get { return category; }
        }
        private readonly double learningRate;
        public double LearningRate
        {
            get { return learningRate; }
        }
        private readonly double bias;
        public double Bias
        {
            get { return bias; }
        }
        public Perceptron(string category, List<Dictionary<string, int>> docsOfCategory, List<Dictionary<string, int>> docsNotOfCat,
							 double learningRate, int iterations, double bias, ISet<string> vocabulary)
        {
            if (learningRate <= 0 || learningRate >= 1)
                throw new ArgumentOutOfRangeException("0 < learningRate < 1 must be satisfied");
           
            this.category = category;
            this.learningRate = learningRate;
            this.bias = bias;

			//Initialize the weightvector
            weightVector = new Dictionary<string, double>();
			foreach (var word in vocabulary)
            {
				weightVector.Add(word, 0);
            }

            for (int i = 0; i < iterations; i++)
            {
                //Learn from training data
                foreach (var doc in docsOfCategory)
                {
                    this.Learn(true, doc);
                }
                foreach (var doc in docsNotOfCat)
                {
                    this.Learn(false, doc);
                }
            }
        }



        private void Learn(bool p, Dictionary<string, int> docBag)
        {
            if (p != (Classify(docBag) >= 0)) // if p and Classify don't agree on the category
            {
				double learningFactor = p?learningRate:-learningRate;
                foreach (var keyValPair in docBag)
                {
                    weightVector[keyValPair.Key] += learningFactor * keyValPair.Value;
                }
            }
        }

        public double Classify(Dictionary<string, int> bagOfDoc)
        {
            double dotProduct = bias;
            foreach (var keyValPair in bagOfDoc)
            {
				double weightOfWord;
                if (weightVector.TryGetValue(keyValPair.Key, out weightOfWord))
                    dotProduct += weightOfWord * keyValPair.Value;
            }
            return dotProduct;
        }
    }
}
