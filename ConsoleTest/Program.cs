using OpenNLP.Tools.NameFind;
using OpenNLP.Tools.SentenceDetect;
using SharpEntropy.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            string str = ".NET Framework (pronounced dot net) is a software framework developed by Mr. Smith (Microsoft) that runs primarily on Microsoft Windows. It includes a large class library known as Framework Class Library (FCL) and James Blunt provides language interoperability (each language can use code written in other languages) across several programming languages. Programs written for .NET Framework execute SYSTEK in a software environment (as contrasted to hardware environment) known as Common Language Runtime (CLR), an application virtual machine that provides services such as security, DISCO DAD 320 memory management, and exception handling. (As such, computer code written using .NET Framework is called 'managed code'.) FCL and CLR together constitute .NET Framework.";


            //// Training new name-finder model.
            //TrainModel(trainingFile: "en-ner-make-02.train", modelName: "make.nbin");

            var entities = ExtractEntities(str, "make");

            Console.WriteLine("Recognized Entities:");
            foreach(var entity in entities)
            {
                Console.WriteLine($"Name: {entity.Name} (Prob: {Math.Round(entity.Probability, 4)})");
            }

            Console.ReadLine();
        }

        static public IEnumerable<(string Name, double Probability)> ExtractEntities(string inputData, string model)
        {
            var path = Directory.GetCurrentDirectory() + "\\Libs\\";

            var sentenceDetector = new EnglishMaximumEntropySentenceDetector(path + "en-sent.nbin");
            var sentences = sentenceDetector.SentenceDetect(inputData);

            var results = new List<(string, double)>();
            foreach (string sentence in sentences)
            {
                var nameFinder = new EnglishNameFinder(path);

                var res = nameFinder.GetNamesWithProbabilities(model, sentence);
                results.AddRange(res);
            }

            return results;
        }

        static public void TrainModel(string trainingFile, string modelName)
        {
            var modelTrained = MaximumEntropyNameFinder.TrainModel(trainingFile);
            var modelWriter = new BinaryGisModelWriter();

            modelWriter.Persist(modelTrained, modelName);
        }
    }
}