using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
	
    class PerceptronClassifier : Classifyer
    {
		static private int fails = 0;
        private class Document
        {
            public readonly string category;
            public readonly Dictionary<string, int> bagOfWords;

            public Document(string category, Dictionary<string, int> bagOfWords)
            {
                this.category = category;
                this.bagOfWords = bagOfWords;
            }
        }

        List<Perceptron> perceptrons;

        public PerceptronClassifier(List<CategoryData> trainingDocs, 
										float learningRate, int iterations, float bias)
        {
			//Construct vocabulary
			ISet<string> vocabulary = new HashSet<string>();
            foreach (var category in trainingDocs)
            {
                foreach (var doc in category.TrainingDocuments)
                {
                    foreach (var word in doc)
                    {
                        vocabulary.Add(word);
                    }
                }
            }
			
			this.perceptrons = new List<Perceptron>();

			foreach(var cat in trainingDocs)
			{
				List<List<string>> docsNotInCategory = new List<List<string>>();
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
        public string Classify(List<string> document)
        {
            foreach (var perceptron in perceptrons)
            {
                if (perceptron.Classify(document))
                    return perceptron.Category;
            }
            Console.WriteLine("Couldn't classify document times: " + ++fails);
            return "failure";
            //throw new ArgumentOutOfRangeException("Document was not able to be classified");
        }
    }
}
