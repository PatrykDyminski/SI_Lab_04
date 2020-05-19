﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SI_Lab_04
{
    class Program
    {
        static void Main(string[] args)
        {
            //GenerateArffFile("test");

            //ile procent to train
            GenerateSets(0.9);

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

        static void GenerateSets(double ratio)
        {
            string[] files = Directory.GetFiles("E:/Projekty/SI/wiki_train_34_categories_data");
            var categories = new List<string>();

            foreach (var file in files)
            {
                categories.Add(file.Split("\\")[1].Split("_")[0]);
            }

            var categoriesDis = categories.Distinct();

            var dataLines = new List<string>();

            foreach (var file in files)
            {
                var content = File.ReadAllLines(file);
                var contentLine = string.Join(" ", content);
                var cat = file.Split("\\")[1].Split("_")[0];
                dataLines.Add(BuildArffLine(contentLine, cat));
            }

            Random rnd = new Random();
            dataLines = dataLines.OrderBy(a => rnd.Next()).ToList();
            List<string> firstPart = dataLines.GetRange(0, (int)(dataLines.Count()*ratio));
            List<string> secondPart = dataLines.Except(firstPart).ToList();

            Console.WriteLine(dataLines.Count);
            Console.WriteLine(firstPart.Count);
            Console.WriteLine(secondPart.Count);

            GenerateArffFile("train", firstPart, categoriesDis.ToList());
            GenerateArffFile("validate", secondPart, categoriesDis.ToList());
        }
    }
}