using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SI_Lab_04
{
    class Program
    {
        static void Main(string[] args)
        { 
            ////train i valid
            string path = "E:/Projekty/SI/wiki_train_34_categories_data";
            ////ile procent to train
            //GenerateSets(1.0, path);

            //test
            string path2 = "E:/Projekty/SI/wiki_test_34_categories_data";
            var categoriesDis = GetCategories(path2);
            var dataLines = GetLinesFromFiles(path2);
            var dataLines2 = GetLinesFromFiles(path);
            dataLines.AddRange(dataLines2);
            GenerateArffFile("train", dataLines, categoriesDis);
        }

        static void GenerateArffFile(string name, List<string> data, List<string> categories)
        {
            var arff = new StringBuilder();

            arff.AppendLine(string.Format("@relation {0}", name));
            arff.AppendLine();
            arff.AppendLine("@attribute Document string");
            string catLine = "@attribute class {";
            string joined = string.Join(",", categories);
            catLine += joined + "}";
            arff.AppendLine(catLine);
            arff.AppendLine();
            arff.AppendLine("@data");
            arff.AppendLine();

            foreach (var line in data)
            {
                arff.AppendLine(line);
            }

            string filename = string.Format("E:/Projekty/SI/{0}.arff", name);
            File.WriteAllText(filename, arff.ToString());
            Console.WriteLine("Done");
        }

        static string BuildArffLine(string content, string category)
        {
            return string.Format("\"{0}\", {1}", content, category);
        }

        static void GenerateSets(double ratio, string path)
        {
            var categoriesDis = GetCategories(path);
            var dataLines = GetLinesFromFiles(path);

            Random rnd = new Random();
            dataLines = dataLines.OrderBy(a => rnd.Next()).ToList();
            List<string> firstPart = dataLines.GetRange(0, (int)(dataLines.Count()*ratio));
            List<string> secondPart = dataLines.Except(firstPart).ToList();

            Console.WriteLine(dataLines.Count);
            Console.WriteLine(firstPart.Count);
            Console.WriteLine(secondPart.Count);

            GenerateArffFile("train", firstPart, categoriesDis);
            GenerateArffFile("validate", secondPart, categoriesDis);
        }

        static List<string> GetLinesFromFiles(string path)
        {
            string[] files = Directory.GetFiles(path);

            var dataLines = new List<string>();

            Regex rgx = new Regex("[^a-zA-ŻżźŹóÓśŚćĆńŃąĄęĘłŁ0-9 -]");

            foreach (var file in files)
            {
                var content = File.ReadAllLines(file);
                var contentLine = string.Join(" ", content);
                contentLine = rgx.Replace(contentLine, "");
                var cat = file.Split("\\")[1].Split("_")[0];
                dataLines.Add(BuildArffLine(contentLine, cat));
            }

            return dataLines;
        }

        static List<string> GetCategories(string path)
        {
            string[] files = Directory.GetFiles(path);
            var categories = new List<string>();

            foreach (var file in files)
            {
                categories.Add(file.Split("\\")[1].Split("_")[0]);
            }

            return categories.Distinct().ToList();
        }
    }
}
