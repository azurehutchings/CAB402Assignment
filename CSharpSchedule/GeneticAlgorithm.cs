using System;
using System.Collections.Generic;
namespace CAB402.CSharp
{
    public class GeneticAlgorithm
    { 
        public static readonly Random random = new(0);

        // TODO: add other methods and classes are required (following Object-Oriented Design Principles)
        public class Individual : IComparable<Individual>
        { 
            // default constructor
            public Individual() {}
            public Individual(Microsoft.FSharp.Core.FSharpFunc<int[], double> fitnessFunction, int[] genes)
            {
                this.Genes = genes;
                this.Fitness = fitnessFunction.Invoke(Genes);
            }

            int IComparable<Individual>.CompareTo(Individual objI)
            {
                Individual iToCompare = (Individual)objI;
                if (Fitness < iToCompare.Fitness)
                {
                    return -1; //if I am less fit than iCompare return -1
                }
                else if (Fitness > iToCompare.Fitness)
                {
                    return 1; //if I am fitter than iCompare return 1
                }
                return 0; // if we are equally return 0
            }
            public int[] Genes { get; set; }
            public double Fitness { get; set; }
        }

        public class Population
        {
            public Population() { }
            public Population(int numberOfGenes, int numberOfIndividuals, Microsoft.FSharp.Core.FSharpFunc<int[], double> fitnessFunction)
            {
                List <Individual> individuals = new List<Individual>();
                int[] genes = new int[numberOfGenes];
                for (int k = 0; k < numberOfGenes; k++)
                {
                    genes[k] = k;
                }
                for (int i = 0; i < numberOfIndividuals; i++)
                {
                    for (int j = numberOfGenes - 1; j > 0; j--)
                    {
                        int randomIndex = random.Next(0, j);

                        int temp = genes[j];
                        genes[j] = genes[randomIndex];
                        genes[randomIndex] = temp;
                    }
                    Individual individual = new Individual(fitnessFunction, genes);
                    individuals.Add(individual);
                }
                Individuals = individuals;
            }

            private Individual Tournament()
            {
                int index1 = random.Next(0, Individuals.Count);
                int index2 = random.Next(0, Individuals.Count);
                if (Individuals[index1].Fitness > Individuals[index2].Fitness)
                {
                    return Individuals[index1];
                }
                else
                {
                    return Individuals[index2];
                }
            }
            private readonly double MutateProbability = 15;

            public int[] Mutate(int[]genes)
            {
                if ((random.Next(0, 100) > MutateProbability) && (genes.Length > 2))
                {
                    int firstIndex = random.Next(0, genes.Length - 1);
                    int secondIndex = random.Next(firstIndex + 1, genes.Length);
                    int length = secondIndex - firstIndex;
                    Array.Reverse(genes, firstIndex, length);
                }
                return genes;
            }

            private static bool CheckContainsGene(int splitPoint, Individual parent, int[] genes, int index)
            {
                for (int i = 0; i < splitPoint; i++)
                {
                    if (parent.Genes[index] == genes[i])
                    {
                        return true;
                    }
                }
                return false;
            }

            private Individual Procreate(Microsoft.FSharp.Core.FSharpFunc<int[], double> fitnessFunction)
            {
                Individual parent1 = Tournament();
                Individual parent2 = Tournament();
                int max;
                if (parent1.Genes.Length > 1)
                {
                    max = parent1.Genes.Length - 1; 
                }
                else
                {
                    max = parent1.Genes.Length;
                }
                int splitPoint = random.Next(1, max);
                int[] genes = new int[parent1.Genes.Length];

                for (int i = 0; i < splitPoint; i++)
                {
                    genes[i] = parent1.Genes[i];
                }

                int index = splitPoint;
                for (int i = 0; i < parent2.Genes.Length; i++)
                {
                    if (CheckContainsGene(splitPoint, parent2, genes, i) == false)
                    {
                        genes[index] = parent2.Genes[i];
                        index++;
                    }
                }
                genes = Mutate(genes);
                Individual child = new(fitnessFunction, genes);
                return child;
            }

            public void Evolve(Microsoft.FSharp.Core.FSharpFunc<int[], double> fitnessFunction, int numberOfChildren)
            {
                List<Individual> children = new List<Individual>();
                for (int i = 0; i < numberOfChildren; i++)
                {
                    children.Add(Procreate(fitnessFunction));
                }
                Individuals.Sort();
                Individuals.Reverse();
                int range = Individuals.Count - 10;
                Individuals.RemoveRange(10, range);
                Individuals.AddRange(children);
            }

            public Individual Fitest()
            {
                Individuals.Sort();
                Individuals.Reverse();
                return Individuals[0];
            }
            public List<Individual> Individuals { get; set; }
        }

        public static IEnumerable<System.Tuple<int[],double>> Optimize(Microsoft.FSharp.Core.FSharpFunc<int[], double> fitnessFunction, int numberOfGenes, int numberOfIndividuals)
        {
            // TODO: add correct implementation here 
            Population population = new(numberOfGenes, numberOfIndividuals, fitnessFunction);
            while (true)
            {
                population.Evolve(fitnessFunction, numberOfIndividuals);
                Individual fitest = population.Fitest();
                yield return new Tuple<int[], double>(fitest.Genes, fitest.Fitness);
            }
        }
    }
}
