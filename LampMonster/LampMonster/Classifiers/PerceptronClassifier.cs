using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
	
    class PerceptronClassifier : Classifyer
    {
        List<Perceptron> perceptrons;

        public PerceptronClassifier(List<CategoryData> categoryData, 
										double learningRate, int iterations, double bias)
        {
			
			this.perceptrons = new List<Perceptron>();
                  
			foreach(var category in categoryData)
			{
                var vocabulary = new HashSet<string>();
                foreach (var document in category.TrainingDocuments)
                {
                    foreach (var feture in document)
                    {
                        vocabulary.Add(feture.Word);
                    }
                }


                var docsNotOfCategory = new List<Document>();
                foreach (var cat in categoryData)
                {
                    if (cat.ID != category.ID)
                        docsNotOfCategory.AddRange(cat.TrainingDocuments);
                }

				this.perceptrons.Add(
                    new Perceptron(
						category.ID,
						category.TrainingDocuments,
						docsNotOfCategory,
						learningRate,
						iterations,
						bias,
                        vocabulary));
            }
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
