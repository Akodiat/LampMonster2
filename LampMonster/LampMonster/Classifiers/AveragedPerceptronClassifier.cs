using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{

    class AveragedPerceptronClassifier : Classifyer
    {
        static int positivesMinusNegatives = 0;
        List<AveragedPerceptron> perceptrons;

        public AveragedPerceptronClassifier(List<CategoryData> trainingDocs,
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


            this.perceptrons = new List<AveragedPerceptron>();

            foreach (var cat in trainingDocs)
            {
                List<Document> docsNotInCategory = new List<Document>();
                foreach (var cat2 in trainingDocs)
                {
                    if (cat2.ID != cat.ID)
                        docsNotInCategory.AddRange(cat2.TrainingDocuments);
                }
                this.perceptrons.Add(
                    new AveragedPerceptron(
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
            if (category == "failure")
                throw new ArgumentException("WTF");
            if (category == "pos")
                positivesMinusNegatives++;
			else if (category == "neg")
				positivesMinusNegatives--;
            Console.WriteLine(positivesMinusNegatives);
            return category;
        }
    }
}
