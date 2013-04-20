using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    struct SentimentTable
    {
        public double FalsePosetive;
        public double FalseNegative;
        public double TruePosetive;
        public double TrueNegative;



        public double[] GetRecall()
        {
            return new double[] 
            {  
                TruePosetive / (TruePosetive + FalseNegative) ,
                TrueNegative / (TrueNegative + FalsePosetive)
            };
        }

        public double[] GetPercision()
        {
            return new double[]
            {
                TruePosetive / (TruePosetive + FalsePosetive),
                TrueNegative / (TrueNegative + FalseNegative)
            };
        }

        public double GetAccuracy()
        {
            double nom = TruePosetive + TrueNegative;
            double denom = nom + FalseNegative + FalsePosetive;

            return nom / denom;
        }

        public double GetMcNemar()
        {
            double fp = FalsePosetive;
            double fn = FalseNegative;
            return ((fp - fn) * (fp - fn)) / (fp * fn);
        }

        public static SentimentTable operator +(SentimentTable lhs, SentimentTable rhs)
        {
            SentimentTable table;
            table.TruePosetive = lhs.TruePosetive + rhs.TruePosetive;
            table.FalsePosetive = lhs.FalsePosetive + rhs.FalsePosetive;
            table.TrueNegative = lhs.TrueNegative + rhs.TrueNegative;
            table.FalseNegative = lhs.FalseNegative + rhs.FalseNegative;

            return table;
        }

        public static SentimentTable operator /(SentimentTable lhs, int rhs)
        {
            SentimentTable table;
            table.TruePosetive = lhs.TruePosetive / rhs;
            table.FalsePosetive = lhs.FalsePosetive / rhs;
            table.TrueNegative = lhs.TrueNegative / rhs;
            table.FalseNegative = lhs.FalseNegative / rhs;

            return table;
        }

        public override string ToString()
        {
            return string.Format("TP: {0} FP: {1} TN: {2} FN: {3}", TruePosetive, FalsePosetive, TrueNegative, FalseNegative);
        }
    }
}
