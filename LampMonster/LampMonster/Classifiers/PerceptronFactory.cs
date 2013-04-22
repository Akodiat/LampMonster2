using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class PerceptronFactory : IClassificationFactory
    {
        protected int fetureCount;
        protected int iterations;
        protected double learningRate;
        protected double bias;

        public PerceptronFactory(int fetureCount, int iterations, double learningRate, double bias)
        {
            this.fetureCount = fetureCount;
            this.iterations = iterations;
            this.learningRate = learningRate;
            this.bias = bias;
        }

        public virtual Classifyer GetClassifyer(List<CategoryData> categories)
        {
            return new PerceptronClassifier(fetureCount, categories, learningRate, iterations, bias);
        }


        public string ClassifyerDesc()
        {
            return string.Format("Perceptron: Iterations={0} LearningRate={1} Bias={2}",
                                 iterations, learningRate, bias);
        }
    }

    class AveragedPerceptronFactory : PerceptronFactory
    {
        public AveragedPerceptronFactory(int fetureCount, int iterations, double learningRate, double bias)
            : base(fetureCount, iterations, learningRate, bias)
        {

        }

        public override Classifyer GetClassifyer(List<CategoryData> categories)
        {
            return new AveragedPerceptronClassifier(categories, learningRate, iterations, bias);
        }
    }
}
