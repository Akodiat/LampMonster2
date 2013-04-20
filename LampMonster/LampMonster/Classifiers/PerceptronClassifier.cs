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

			//Build the megadoc
            var categoryToBags = new Dictionary<string, List<Dictionary<string, int>>>();
            foreach (var category in categoryData)
            {
                categoryToBags.Add(
                    category.ID,
                    BagBuilder.BuildBagsFromDocuments(category.TrainingDocuments)
                    );
            }

			//Construct the vocabulary
            var vocabulary = new HashSet<string>();
            foreach (var documentType in categoryToBags.Values)
            {
                foreach (var document in documentType)
                {
                    foreach (var word in document.Keys)
                    {
                        vocabulary.Add(word);
                    }
                }
            }
      
			foreach(var category in categoryData)
			{
                var docsOfCategory = new List<Dictionary<string, int>>(categoryToBags[category.ID]);

                var docsNotOfCategory = new List<Dictionary<string, int>>();
                foreach (var cat in categoryToBags.Keys)
                {
                    if (cat != category.ID)
                        docsNotOfCategory.AddRange(categoryToBags[cat]);
                }

				this.perceptrons.Add(
                    new Perceptron(
						category.ID,
						docsOfCategory,
						docsNotOfCategory,
						learningRate,
						iterations,
						bias,
                        vocabulary));
            }

        }
        public string Classify(Document document)
        {
            Dictionary<string, int> docBag = BagBuilder.BuildBagFromDocument(document);
            string category = "failure";
            double highest = double.NegativeInfinity;
            foreach (var perceptron in perceptrons)
            {
				double perceptronOutput = perceptron.Classify(docBag);
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
