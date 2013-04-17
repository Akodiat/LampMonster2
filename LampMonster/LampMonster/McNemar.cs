using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class McNemar
    {
        private static double calcTestStatistic(double pos_neg, double neg_pos)
        {
            return (Math.Pow((pos_neg-neg_pos),2) / (pos_neg + neg_pos));
        }
        public static double[,] test(TruthTable[,] truthTables)
        {
            int numberOfCategories = (int) Math.Sqrt(truthTables.Length);
            Console.WriteLine("Weeeeee: McNemar");
            var result = new double[numberOfCategories, numberOfCategories];
            for(int i=0; i<numberOfCategories; i++){
                for (int j = 0; j < numberOfCategories; j++){
                    result[i,j] = calcTestStatistic(truthTables[i, j].FalsePosetive, truthTables[i, j].FalseNegative);
                    Console.WriteLine("({0},{1}) {2}\t", i, j, result[i, j]);
                }
                Console.WriteLine("\n");
            }

            return result;
        }
    }
}
