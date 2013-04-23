using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class Utils
    {
        private const int SEED = 9876;
        private static Random random = new Random(SEED);

        public static List<T> CopyShuffle<T>(List<T> list)
        {
            var copy = new List<T>(list);
            int n = copy.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = copy[k];
                copy[k] = copy[n];
                copy[n] = value;
            }

            return copy;
        }

        public static T[,] MergeMatricies<T>(T[][,] matricies, Func<T, T, int, T> merger)
        {
            int dimX = matricies[0].GetLength(0),
                dimY = matricies[0].GetLength(1);

            var totalResult = new T[dimX, dimY];
            for (int k = 0; k < matricies.Length; k++)
            {
                T[,] partialResult = matricies[k];
                for (int i = 0; i < dimX; i++)
                {
                    for (int j = 0; j < dimY; j++)
                    {
                        totalResult[i, j] = merger(totalResult[i, j],
                                                   partialResult[i, j],
                                                   matricies.Length);
                    }
                }
            }
            return totalResult;
        }

        public static List<String> ExtractVocabulary(int featuresCount, List<Document> documents)
        {
            var bag = new Dictionary<string, double>();
            foreach (var document in documents)
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

            for (int i = list.Count - 1; i >= featuresCount; i--)
            {
                list.RemoveAt(i);
            }
            return list;
        }
    }
}