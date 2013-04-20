using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class AveragedPerceptron
    {
        private Dictionary<string, double> weightVector; //Implemented as a map since it is simpler
        private Dictionary<string, double> averageWeightVector; //Implemented as a map since it is simpler
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
        public AveragedPerceptron(string category, List<Document> docsOfCategory, List<Document> docsNotOfCat,
							 double learningRate, int iterations, double bias, ISet<string> vocabulary)
        {
            if (learningRate <= 0 || learningRate >= 1)
                throw new ArgumentOutOfRangeException("0 < learningRate < 1 must be satisfied");
           
            this.category = category;
            this.learningRate = learningRate;
            this.bias = bias;
            int trainingSize = docsNotOfCat.Count + docsOfCategory.Count;

			//Initialize the weightvectors
            weightVector = new Dictionary<string, double>();
			foreach (var word in vocabulary)
            {
				weightVector.Add(word, 0);
            }
            averageWeightVector = new Dictionary<string, double>(weightVector);

            for (int i = 0; i < iterations; i++)
            {
                //Learn from training data
                foreach (var doc in docsOfCategory)
                {
                    this.Learn(true, doc, i, iterations, trainingSize);
                }
                foreach (var doc in docsNotOfCat)
                {
                    this.Learn(false, doc, i, iterations, trainingSize);
                }
            }
            List<KeyValuePair<string, double>> weightList = weightVector.ToList();
            weightList.Sort((firstPair, nextPair) => { return firstPair.Value.CompareTo(nextPair.Value); });

        }

        private void Learn(bool p, Document doc, int counter, int iterations, int trainingSize)
        {
			bool guess = Classify(doc)>=0;
			double learningFactor = p ? learningRate : -learningRate;
            if (p ^ guess) // if p and Classify don't agree on the category
            {
				
                foreach (var word in doc)
                {
                    weightVector[word.Word] += learningFactor * word.Count;
                }

                double average_weight = (trainingSize * iterations - counter) / (trainingSize * iterations);
                foreach (var word in doc)
                {
                    averageWeightVector[word.Word] += average_weight * learningFactor * word.Count;
                }
            }
        }

        public double Classify(Document doc)
        {
            double dotProduct = bias;
            foreach (var word in doc)
            {
				double weightOfWord;
                if (averageWeightVector.TryGetValue(word.Word, out weightOfWord))
                    dotProduct += weightOfWord * word.Count;
            }
            return dotProduct;
        }
    }
}