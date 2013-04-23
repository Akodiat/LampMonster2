using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    abstract class PerceptronBase : IPerceptron
    {
        public string Category { get; private set; }
        public readonly double Bias;
        protected VeightVector weightVector;

        public PerceptronBase(string category, List<Document> docsOfCategory, List<Document> docsNotOfCat,
							 double learningRate, int iterations, double bias, List<string> vocabulary)
        {
            if (learningRate <= 0 || learningRate >= 1)
                throw new ArgumentOutOfRangeException("0 < learningRate < 1 must be satisfied");
           
            this.Category = category;
            this.Bias = bias;
			
            //Initialize the weightvector
            weightVector = new VeightVector(vocabulary);
            Learn(docsOfCategory, docsNotOfCat, iterations, learningRate);
        }

        protected abstract void Learn(List<Document> docsOfCategory, List<Document> docsNotOfCat, int iterations, double learningRate);


        public double Classify(Document document)
        {
            return Bias + weightVector.DotProduct(document);
        }

    }
}
