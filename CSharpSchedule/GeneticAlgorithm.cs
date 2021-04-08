using System.Collections.Generic;

namespace CAB402.CSharp
{
    public class GeneticAlgorithm
    {
        // TODO: add other methods and classes are required (following Object-Oriented Design Principles)
        public class Individual
        {
            public int[] Genes { get; set; }
            public int Fitness { get; set; }
        }

        public class Parent : Individual
        {
            public List<Child> Children { get; set; }
        }

        public class Child : Individual
        {
            public List<Parent> Parents { get; set; }
        }
        public static IEnumerable<System.Tuple<int[],double>> Optimize(Microsoft.FSharp.Core.FSharpFunc<int[], double> fitnessFunction, int numberOfGenes, int numerOfIndividuals)
        {
            // TODO: add correct implementation here 
            yield break;
        }
    }
}
