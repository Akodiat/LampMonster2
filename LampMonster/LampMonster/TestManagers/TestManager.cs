using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LampMonster
{
    interface TestManager<Result>
    {
        Result RunPartialTests(List<ClassData> trainingData,
                                 List<ClassData> testData);

        Result MergeTests(Result[] testResults);        
    }
}
