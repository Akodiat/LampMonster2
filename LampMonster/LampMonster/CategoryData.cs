using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quadruple;

namespace LampMonster
{
    sealed class CategoryData
    {
        public string ID { get; private set; }
        public Quad CategoryProb { get; private set; }
        public List<Document> TrainingDocuments { get; private set; }

        public CategoryData(string id,
                            Quad categoryProb,
                            List<Document> trainingData)
        {
            this.ID = id;
            this.CategoryProb = categoryProb;
            this.TrainingDocuments = trainingData;
        }
    }
}
