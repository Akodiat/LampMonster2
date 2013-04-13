using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LampMonster
{
    interface IPathFinder
    {
        List<string> GetTrainingPaths(string category);
        List<string> GetProcessingPaths(string category);
    }
}
