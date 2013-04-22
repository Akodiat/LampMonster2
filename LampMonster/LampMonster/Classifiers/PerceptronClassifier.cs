using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
	
    class PerceptronClassifier : Classifyer
    {
        List<PerceptronBase> perceptrons;

        public PerceptronClassifier(List<PerceptronBase> perceptrons)
        {
            this.perceptrons = perceptrons;
        }


        public string Classify(Document document)
        {
            string category = "failure";
            double highest = double.NegativeInfinity;
            foreach (var perceptron in perceptrons)
            {
                double perceptronOutput = perceptron.Classify(document);
                if (perceptronOutput > highest)
                {
                    highest = perceptronOutput;
                    category = perceptron.Category;
                }
            }
            return category;
        }
    }
}
