using System;
using System.Collections.Generic;
using System.IO;

namespace LampMonster
{
    class OrderedPathFinder : IPathFinder
    {
        private int trainingSize;
        private int processSize;

        public OrderedPathFinder(int trainingSize, int processSize)
        {
            this.trainingSize = trainingSize;
            this.processSize = processSize;
        }

        private static List<string> GetPaths(string categoryDir, int start, int count)
        {
            if (!Directory.Exists(categoryDir))
                throw new ArgumentException("Directory no found!");

            var list = new List<string>(count);
            string[] files = Directory.GetFiles(categoryDir);

            if (files.Length < count + start)
                throw new ArgumentException("To few files.");

            for (int i = start; i < start + count; i++)
                list.Add(files[i - start]);

            return list;
        }


        public List<string> GetTrainingPaths(string categoryDir)
        {
            return GetPaths(categoryDir, 0, trainingSize);
        }

        public List<string> GetProcessingPaths(string categoryDir)
        {
            return GetPaths(categoryDir, trainingSize, processSize); 
        }
    }
}
