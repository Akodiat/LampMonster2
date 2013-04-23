using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using Accord.Math;


namespace LampMonster
{
    class KNNFactory:IClassificationFactory
    {
        private readonly int nearestNeighborsCount;
		private readonly int featureCount;
        public KNNFactory(int featureCount, int nearestNeighborsCount)
        {
            this.featureCount = featureCount;
            this.nearestNeighborsCount = nearestNeighborsCount;
        }

        public Classifyer GetClassifyer(List<CategoryData> categories)
        {
			var trainingDox = new List<Document>();
			foreach (var category in categories)
			{
				trainingDox.AddRange(category.TrainingDocuments);
			}
            List<string> vocabulary = Utils.ExtractVocabulary(featureCount, trainingDox);

            var indexMap = new Dictionary<string, int>();
            for (int i = 0; i < vocabulary.Count; i++)
            {
                indexMap.Add(vocabulary[i], i);
            }

            double[][] inputVectors = GenerateInputVectors(vocabulary, indexMap, trainingDox);
            int[] output = GenerateOutput(categories, trainingDox.Count);


            return new KNNClassifier(categories, inputVectors, output, this.nearestNeighborsCount, indexMap);
        }

        private double[][] GenerateInputVectors(List<string> vocabulary, Dictionary<string, int> indexMap, List<Document> trainingDox)
        {
			double[][] input = new double[trainingDox.Count][];
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Utils.GenerateInputVector(vocabulary.Count, trainingDox[i], indexMap);
            }
            return input;
        }



        private int[] GenerateOutput(List<CategoryData> categories, int documentsCount)
        {
            List<int> output = new List<int>();
            int classCounter = 0;
            foreach (var category in categories)
            {
                foreach (var doc in category.TrainingDocuments)
                {
                    output.Add(classCounter);
                }
                classCounter++;
            }
            return output.ToArray();
        }


		//private double[] ConvertToFeatureVector(

        public string ClassifyerDesc()
        {
            return string.Format("KNearestNeighbors: k = {0}, featurecount = {1}",
                                 this.nearestNeighborsCount, this.featureCount);
        }
    }
}
