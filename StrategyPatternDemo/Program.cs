using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPatternDemo
{
    //internal class Program
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize datasets
            var datasets = new List<DataSet>
            {
                new DataSet("DataSet1", new List<int>{ 5, 2, 9, 1 }),
                new DataSet("DataSet2", new List<int>{ 15, 25, 10, 5 }),
                new DataSet("DataSet3", new List<int>{ 34, 0, -5, 3 }),
                new DataSet("DataSet4", new List<int>{ 8, 22, 7, -4 }),
            };

            // Initialize strategies
            ISortStrategy bubbleSortStrategy = new BubbleSortStrategy();
            ISortStrategy quickSortStrategy = new QuickSortStrategy();
            IOutputStrategy horizontalOutputStrategy = new HorizontalOutputStrategy();
            IOutputStrategy verticalOutputStrategy = new VerticalOutputStrategy();

            // Apply strategies and demonstrate behavior
            foreach (var dataset in datasets)
            {
                // Explicit casting to resolve CS8957
                dataset.SetSortStrategy(dataset.Name == "DataSet1" || dataset.Name == "DataSet2" ? bubbleSortStrategy : quickSortStrategy);
                dataset.SetOutputStrategy(dataset.Name == "DataSet1" || dataset.Name == "DataSet3" ? horizontalOutputStrategy : verticalOutputStrategy);

                Console.WriteLine($"Processing {dataset.Name}:");
                dataset.SortData();
                dataset.OutputData();
                Console.WriteLine();
            }
        }

        // Sorting strategy interface
        public interface ISortStrategy
        {
            List<int> Sort(List<int> data);
        }

        // Concrete implementation of bubble sort
        public class BubbleSortStrategy : ISortStrategy
        {
            public List<int> Sort(List<int> data)
            {
                var sortedData = new List<int>(data);
                int temp;
                for (int i = 0; i < sortedData.Count - 1; i++)
                {
                    for (int j = 0; j < sortedData.Count - i - 1; j++)
                    {
                        if (sortedData[j] > sortedData[j + 1])
                        {
                            temp = sortedData[j];
                            sortedData[j] = sortedData[j + 1];
                            sortedData[j + 1] = temp;
                        }
                    }
                }
                return sortedData;
            }
        }

        // Concrete implementation of quick sort
        public class QuickSortStrategy : ISortStrategy
        {
            public List<int> Sort(List<int> data)
            {
                if (data.Count <= 1) return data;
                int pivot = data[0];
                var less = new List<int>();
                var greater = new List<int>();
                for (int i = 1; i < data.Count; i++)
                {
                    if (data[i] <= pivot) less.Add(data[i]);
                    else greater.Add(data[i]);
                }
                var sorted = Sort(less);
                sorted.Add(pivot);
                sorted.AddRange(Sort(greater));
                return sorted;
            }
        }

        // Output strategy interface
        public interface IOutputStrategy
        {
            void Output(List<int> data);
        }

        // Concrete implementation for horizontal output
        public class HorizontalOutputStrategy : IOutputStrategy
        {
            public void Output(List<int> data)
            {
                Console.WriteLine(string.Join(", ", data));
            }
        }

        // Concrete implementation for vertical output
        public class VerticalOutputStrategy : IOutputStrategy
        {
            public void Output(List<int> data)
            {
                foreach (int item in data)
                {
                    Console.WriteLine(item);
                }
            }
        }

        // DataSet class that uses sorting and output strategies
        public class DataSet
        {
            public string Name { get; private set; }
            public List<int> Data { get; private set; }
            private ISortStrategy sortStrategy;
            private IOutputStrategy outputStrategy;

            public DataSet(string name, List<int> data)
            {
                Name = name;
                Data = data;
            }

            public void SetSortStrategy(ISortStrategy sortStrategy)
            {
                this.sortStrategy = sortStrategy;
            }

            public void SetOutputStrategy(IOutputStrategy outputStrategy)
            {
                this.outputStrategy = outputStrategy;
            }

            public void SortData()
            {
                Data = sortStrategy.Sort(Data);
            }

            public void OutputData()
            {
                outputStrategy.Output(Data);
            }
        }

    }
}
