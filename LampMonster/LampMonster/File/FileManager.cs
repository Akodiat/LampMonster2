using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LampMonster
{
    class FileManager
    {

        public static List<ClassData> ExctractClassData(string root,
                                                        FileParser parser)
        {
            var classData = new List<ClassData>();

            foreach (var subdir in Directory.EnumerateDirectories(root))
            {
                string category = Path.GetFileName(subdir);

                var posFiles = Directory.GetFiles(Path.Combine(subdir, "pos"));
                var negFiles = Directory.GetFiles(Path.Combine(subdir, "neg"));

                classData.Add(CreateClassData(category, posFiles, negFiles, parser));
            }

            return classData;            
        }

        private static ClassData CreateClassData(string category, string[] posFiles, 
                                                 string[] negFiles, FileParser parser)
        {
            var posDoc = new List<Document>();
            var negDoc = new List<Document>();

            foreach (var file in posFiles)
                posDoc.Add(new Document(file, parser.GetWordsInFile(file)));

            foreach (var file in negFiles)
                negDoc.Add(new Document(file, parser.GetWordsInFile(file)));
           
            return new ClassData(category, posDoc, negDoc);
        }
    }
}