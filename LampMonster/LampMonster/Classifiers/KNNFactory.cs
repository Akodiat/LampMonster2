using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using Accord.Math;


namespace LampMonster.Classifiers
{
    class KNNFactory:IClassificationFactory
    {
		private readonly int featureCount;
		public KNNFactory(int featureCount)
        {
            this.featureCount = featureCount;
        }

        public Classifyer GetClassifyer(List<CategoryData> categories)
        {
			List<Document> trainingDox = new List<Document>();
			foreach (var category in categories)
			{
				trainingDox.AddRange(category.TrainingDocuments);
			}
            List<string> vocabulary = Utils.ExtractVocabulary(featureCount, trainingDox);

            Dictionary<string, int> indexMap = new Dictionary<string, int>();
            for (int i = 0; i < vocabulary.Count; i++)
            {
                indexMap.Add(vocabulary[i], i);
            }

            throw new NotImplementedException();

        }

		//private double[] ConvertToFeatureVector(

        public string ClassifyerDesc()
        {
            throw new NotImplementedException();
        }
    }
}
