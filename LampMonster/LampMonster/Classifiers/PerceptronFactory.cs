using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    enum PerceptronType
    {
        Normal,
        Awereged,
        Voted
    }


    class PerceptronFactory : IClassificationFactory
    {
        private PerceptronType type;

        protected int fetureCount;
        protected int iterations;
        protected double learningRate;
        protected double bias;



        public PerceptronFactory(int fetureCount, int iterations, double learningRate, double bias, PerceptronType type)
        {
            this.fetureCount = fetureCount;
            this.iterations = iterations;
            this.learningRate = learningRate;
            this.bias = bias;
            this.type = type;
        }

        public virtual Classifyer GetClassifyer(List<CategoryData> categories)
        {
            var perceptrons = new List<IPerceptron>();
            foreach (var category in categories)
            {
                var list = Utils.ExtractVocabulary(this.fetureCount, category.TrainingDocuments);

                var docsNotOfCategory = new List<Document>();
                foreach (var cat in categories)
                {
                    if (cat.ID != category.ID)
                        docsNotOfCategory.AddRange(cat.TrainingDocuments);
                }

                perceptrons.Add(CreatePerceptron(category, list, docsNotOfCategory));
            }


            return new PerceptronClassifier(perceptrons);
        }

        private IPerceptron CreatePerceptron(CategoryData category, List<string> list, List<Document> docsNotOfCategory)
        {
            if (type == PerceptronType.Awereged)
                return new AveragedPerceptron(category.ID,
                                    category.TrainingDocuments,
                                    docsNotOfCategory,
                                    learningRate,
                                    iterations,
                                    bias,
                                    list);
            else if (type == PerceptronType.Normal)
                return new Perceptron(category.ID,
                                    category.TrainingDocuments,
                                    docsNotOfCategory,
                                    learningRate,
                                    iterations,
                                    bias,
                                    list);
            else
                return new VotedPerceptron(category.ID,
                                           category.TrainingDocuments,
                                           docsNotOfCategory,
                                           learningRate,
                                           iterations,
                                           list);
        }

        public string ClassifyerDesc()
        {
            return string.Format("Perceptron: Iterations={0} LearningRate={1} Bias={2} FetureCount={3}",
                                 iterations, learningRate, bias, fetureCount);
        }
    }
}
