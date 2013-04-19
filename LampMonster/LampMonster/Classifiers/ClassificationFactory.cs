using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    interface IClassificationFactory 
    {
        Classifyer GetClassifyer(List<CategoryData> categories);
        string ClassifyerDesc();
    }


    class PerceptronFactory : IClassificationFactory
    {
        protected int iterations;
        protected double learningRate;
        protected double bias;

        public PerceptronFactory(int iterations, double learningRate, double bias)
        {
            this.iterations = iterations;
            this.learningRate = learningRate;
            this.bias = bias;
        }

        public virtual Classifyer GetClassifyer(List<CategoryData> categories)
        {
            return new PerceptronClassifier(categories, learningRate, iterations, bias);
        }


        public string ClassifyerDesc()
        {
            return string.Format("Perceptron: Iterations={0} LearningRate={1} Bias={2}",
                                 iterations, learningRate, bias);
        }
    }

    class AveragedPerceptronFactory : PerceptronFactory
    {

        public AveragedPerceptronFactory(int iterations, double learningRate, double bias)
			:base(iterations, learningRate, bias)
        {
            
        }

        public override Classifyer GetClassifyer(List<CategoryData> categories)
        {
            return new AveragedPerceptronClassifier(categories, learningRate, iterations, bias);
        }
    }
}
