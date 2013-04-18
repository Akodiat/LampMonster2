using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class Perceptron
    {
        private Dictionary<string, float> weightVector; //Implemented as a map since it is simpler
        private readonly string category;
        public string Category
        {
            get { return category; }
        }
        private readonly float learningRate;
        public float LearningRate
        {
            get { return learningRate; }
        }
        private readonly float bias;
        public float Bias
        {
            get { return bias; }
        }
        public Perceptron(string category, List<Document> docsOfCategory, List<Document> docsNotOfCat,
							 float learningRate, int iterations, float bias, ISet<string> vocabulary)
        {
            if (learningRate <= 0 || learningRate >= 1)
                throw new ArgumentOutOfRangeException("0 < learningRate < 1 must be satisfied");
           
            this.category = category;
            this.learningRate = learningRate;
            this.bias = bias;

			//Initialize the weightvector
            weightVector = new Dictionary<string, float>();
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
            List<KeyValuePair<string, float>> weightList = weightVector.ToList();
            weightList.Sort((firstPair, nextPair) => { return firstPair.Value.CompareTo(nextPair.Value); });

        }

        private void Learn(bool p, Document doc)
        {
            if (p ^ (Classify(doc)>=0)) // if p and Classify don't agree on the category
            {
                foreach (var word in doc)
                {
                    weightVector[word] += p?learningRate:-learningRate;
                }
            }
        }

        public float Classify(Document doc)
        {
            float dotProduct = bias;
            foreach (var word in doc)
            {
				float weightOfWord;
                if (weightVector.TryGetValue(word, out weightOfWord))
                    dotProduct += weightOfWord;
            }
            return dotProduct;
        }
    }
}
