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

        public PerceptronClassifier(List<CategoryData> trainingDocs, 
										double learningRate, int iterations, double bias)
        {
			//Construct vocabulary
			ISet<string> vocabulary = new HashSet<string>();
            foreach (var category in trainingDocs)
            {
                foreach (var doc in category.TrainingDocuments)
                {
                    foreach (var word in doc)
                    {
                        vocabulary.Add(word.Word);
                    }
                }
            }
			
			this.perceptrons = new List<Perceptron>();

			foreach(var cat in trainingDocs)
			{
				List<Document> docsNotInCategory = new List<Document>();
				foreach (var cat2 in trainingDocs)
				{
						if(cat2.ID != cat.ID)
							docsNotInCategory.AddRange(cat2.TrainingDocuments);
				}
				this.perceptrons.Add(
                    new Perceptron(
						cat.ID,
						cat.TrainingDocuments,
						docsNotInCategory,
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
            //Console.WriteLine("Couldn't classify document times: " + ++fails);
            if (category == "failure")
                throw new ArgumentException("WTF");
            return category;
            //throw new ArgumentOutOfRangeException("Document was not able to be classified");
        }
    }
}
