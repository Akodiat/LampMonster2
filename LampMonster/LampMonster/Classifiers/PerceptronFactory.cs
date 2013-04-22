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
        Awereged
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
            var perceptrons = new List<PerceptronBase>();
            foreach (var category in categories)
            {
                var list = ExctractVocabulary(category);

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

        private PerceptronBase CreatePerceptron(CategoryData category, List<string> list, List<Document> docsNotOfCategory)
        {
            if (type == PerceptronType.Awereged)
                return new AveragedPerceptron(category.ID,
                                    category.TrainingDocuments,
                                    docsNotOfCategory,
                                    learningRate,
                                    iterations,
                                    bias,
                                    list);
            else
                return new Perceptron(category.ID,
                                    category.TrainingDocuments,
                                    docsNotOfCategory,
                                    learningRate,
                                    iterations,
                                    bias,
                                    list);
        }

        private List<string> ExctractVocabulary(CategoryData category)
        {
            var bag = new Dictionary<string, double>();
            foreach (var document in category.TrainingDocuments)
            {
                foreach (var feture in document)
                {
                    if (!bag.ContainsKey(feture.Word))
                        bag.Add(feture.Word, feture.Frequency);
                    else
                        bag[feture.Word] += feture.Frequency;
                }
            }

            var list = new List<string>(bag.Keys);
            list.Sort((x, y) =>
            {
                double xW = bag[x];
                double yW = bag[y];

                if (xW < yW) return 1;
                else if (xW > yW) return -1;
                else return 0;
            });

            for (int i = list.Count - 1; i >= fetureCount; i--)
            {
                list.RemoveAt(i);
            }
            return list;
        }

        public string ClassifyerDesc()
        {
            return string.Format("Perceptron: Iterations={0} LearningRate={1} Bias={2} FetureCount={3}",
                                 iterations, learningRate, bias, fetureCount);
        }
    }
}
