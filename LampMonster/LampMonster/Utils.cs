using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    class Utils
    {
        private const int SEED = 1;
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
    }
}
