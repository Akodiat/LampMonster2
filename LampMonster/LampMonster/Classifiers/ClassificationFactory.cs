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
}
