using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
using Accord.Math;

namespace LampMonster
{
    class KNNClassifier:Classifyer
    {
        private readonly KNearestNeighbors knn;
        private readonly Dictionary<int, string> classDict;
        private readonly Dictionary<string, int> indexDict;
        public KNNClassifier(List<CategoryData> categories, double[][] inputVectors, int[] output, int k, 
								Dictionary<string, int> indexDict)
        {
            this.knn = new KNearestNeighbors(k, categories.Count, inputVectors, output, Distance.Chebyshev);
            this.classDict = new Dictionary<int, string>();
            this.indexDict = indexDict;
            for (int i = 0; i < categories.Count; i++)
            {
                classDict.Add(i, categories[i].ID);
            }
        }
        public string Classify(Document document)
        {
            return classDict[this.knn.Compute(Utils.GenerateInputVector(
                indexDict.Count, document, this.indexDict))];
        }
    }
}
