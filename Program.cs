using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SortingAlgorithmEnvironment
{
    internal class Program
    {
        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////

        int shellIndex = 0;
        int heapIndex = 0;

        double[] shellSetAvg = new double[5];
        double[] heapSetAvg = new double[5];
        static void Heapify(long[] array, long size, long index)
        {
            var largestIndex = index;
            var leftChild = 2 * index + 1;
            var rightChild = 2 * index + 2;

            if (leftChild < size && array[leftChild] > array[largestIndex])
            {
                largestIndex = leftChild;
            }
            if (rightChild < size && array[rightChild] > array[largestIndex])
            {
                largestIndex = rightChild;
            }
            if (largestIndex != index)
            {
                var tempVar = array[index];
                array[index] = array[largestIndex];
                array[largestIndex] = tempVar;
                Heapify(array, size, largestIndex);
            }
        }

        public long[] heapSort(long[] array)
        {   
            long size = array.Length;
            if (size <= 1)
                return array;
            for (long i = size / 2 - 1; i >= 0; i--)
            {
                Heapify(array, size, i);
            }
            for (long i = size - 1; i >= 0; i--)
            {
                var tempVar = array[0];
                array[0] = array[i];
                array[i] = tempVar;
                Heapify(array, i, 0);
            }
            return array;
        }

        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////
        public long[] shellSort(long[] array)
        {
            long size = array.Length;
            if (size <= 1)
                return array;
            for (long interval = size / 2; interval > 0; interval /= 2)
            {
                for (long i = interval; i < size; i++)
                {
                    var currentKey = array[i];
                    var k = i;
                    while (k >= interval && array[k - interval] > currentKey)
                    {
                        array[k] = array[k - interval];
                        k -= interval;
                    }
                    array[k] = currentKey;
                }
            }
            return array;
        }

        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////

        static public long[][] generateRandomDataSet(long arrayLength, bool reverseOrder = false)
        {
            var randomInt = new System.Random(0);

            if (!reverseOrder)
            {
                long[][] testData = new long[5][];
                for (long i = 0; i < 5; i++)
                {
                    testData[i] = new long[arrayLength];
                    for (long j = 0; j < arrayLength; j++)
                    {
                        testData[i][j] = randomInt.NextInt64();
                        //testData[i][j] = RandomNumberGenerator.GetInt32();
                    }
                }

                return testData;

            } else
            {
                long[][] testData = new long[1][];
                testData[0] = new long[arrayLength];
                testData[0][0] = 2147483646;

                long temp = 9223372036854775807;
                long decr = randomInt.NextInt64(0, 1000);

                for (long i = 1; i < arrayLength; i++)
                {
                    temp -= decr;
                    testData[0][i] = temp;
                }

                return testData;
            }
        }

        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////

        public void sortDataSet(long[][] dataSet, int arrayAmt, int repeatAmt, string sortingAlgorithm, int setNumber)
        {
            Stopwatch sortTimer;

            double dataSetAvg = 0;
            double[] eachSetAvg = new double[5];

            for (int i = 0; i < arrayAmt; i++)
            {
                double elapsedSum = 0;
                Console.WriteLine("| "+ sortingAlgorithm + " / Amount: " + dataSet[0].Length +  "\t| \tData set " + setNumber + " / Array "  + (i + 1) + "\t");
                Console.WriteLine("|\t\t\t\t|\t\t\t\t");

                shellSort(dataSet[i]);
                
                if(sortingAlgorithm == "shellSort")
                {
                    for (int j = 1; j <= repeatAmt; j++)
                    {
                        sortTimer = Stopwatch.StartNew();
                        shellSort(dataSet[i]);
                        sortTimer.Stop();

                        double elapsedTime = sortTimer.Elapsed.TotalMilliseconds;
                        elapsedSum += elapsedTime;
                        Console.WriteLine("|\tTest number: " + j + "\t\t|\tElapsed time: " + ((float)Math.Round(elapsedTime * 100f) / 100f) + " \tms\t");
                    }
                } // SHELL SORT
                else
                {
                    for (int j = 1; j <= repeatAmt; j++)
                    {
                        sortTimer = Stopwatch.StartNew();
                        heapSort(dataSet[i]);
                        sortTimer.Stop();

                        double elapsedTime = sortTimer.Elapsed.TotalMilliseconds;
                        elapsedSum += elapsedTime;
                        Console.WriteLine("|\tTest number: " + j + "\t\t|\tElapsed time: " + ((float)Math.Round(elapsedTime * 100f) / 100f) + " \tms\t");
                    }
                }  // HEAP SORT

                Console.WriteLine("|\t\t\t\t|\t\t\t\t");
                Console.WriteLine("|\t\t\t\t|\tAverage elapsed time: "+ ((float)Math.Round((elapsedSum / repeatAmt) * 100f) / 100f) + " ms\t");
                Console.WriteLine("\n");

                elapsedSum /= 10;
                dataSetAvg += (elapsedSum);
                eachSetAvg[i] = (elapsedSum);

            }

            Console.WriteLine("\t\t\t\t| Data set: " + setNumber +  " / Data amount: " + dataSet[0].Length + "\n");

            for (int i = 0; i < arrayAmt; i++)
            {
                    Console.WriteLine("\t\t\t\t| Array " + (i+1) + " average: " + ((float)Math.Round(eachSetAvg[i] * 100f) / 100f) + " ms");
            }

            Console.WriteLine("\n");
            Console.WriteLine("\t\t\t\t| Total average: " + ((float)Math.Round((dataSetAvg/arrayAmt) * 100f) / 100f) + " ms |\n\n\n\n");

            if (sortingAlgorithm == "shellSort")
            {
                shellSetAvg[shellIndex] = (dataSetAvg / arrayAmt);
                shellIndex++;
            }
            else
            {
                heapSetAvg[heapIndex] = (dataSetAvg / arrayAmt);
                heapIndex++;
            }

        }

        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////
        /// ///////////////////////////////////////////////////

        static void Main()
        {
            Program test = new Program();

            int testAmount = 10;
            int setDepth;
            string shell = "shellSort";
            string heap = "heapSort";

            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////

            // FIRST TEST STAGE

            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////

            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("First Test Stage / Randomly Generated Data");
            Console.WriteLine("------------------------------------------------------\n");

            Console.WriteLine("Creating data sets. Please wait...\n");

            setDepth = 5;

            long[][] firstDataSet = generateRandomDataSet(500000);
            long[][] secondDataSet = generateRandomDataSet(1000000);
            long[][] thirdDataSet = generateRandomDataSet(5000000);
            long[][] fourthDataSet = generateRandomDataSet(10000000);
            long[][] fifthDataSet = generateRandomDataSet(25000000);

            Console.WriteLine("Finished.\n");
            Console.WriteLine("------------------------------------------------------\n");


            Console.WriteLine("Press any key to start...");
            Console.ReadKey();

            //

            test.sortDataSet(firstDataSet, setDepth, testAmount, shell, 1);
            test.sortDataSet(secondDataSet, setDepth, testAmount, shell, 2);
            test.sortDataSet(thirdDataSet, setDepth, testAmount, shell, 3);
            test.sortDataSet(fourthDataSet, setDepth, testAmount, shell, 4);
            test.sortDataSet(fifthDataSet, setDepth, testAmount, shell, 5);

            //

            test.sortDataSet(firstDataSet, setDepth, testAmount, heap, 1);
            test.sortDataSet(secondDataSet, setDepth, testAmount, heap, 2);
            test.sortDataSet(thirdDataSet, setDepth, testAmount, heap, 3);
            test.sortDataSet(fourthDataSet, setDepth, testAmount, heap, 4);
            test.sortDataSet(fifthDataSet, setDepth, testAmount, heap, 5);

            //
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("     TEST RESULTS");
            Console.WriteLine("------------------------------------------------------\n");

            Console.WriteLine("--------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|\tShell Sort \t\t\t\t|\tHeap Sort");
            Console.WriteLine("|\t\t\t\t\t\t|\t");

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("|\tData Set " + (i + 1) + "\tAverage:  " + ((float)Math.Round(test.shellSetAvg[i] * 100f) / 100f) + "    ms \t|\t" + "Data Set " + (i + 1) + "\tAverage:  " + ((float)Math.Round(test.heapSetAvg[i] * 100f) / 100f) + "    ms");
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------------------\n");

            Console.WriteLine("\n\n\n\n\nPress any key to continue...");
            Console.ReadKey();


            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////

            // SECOND TEST STAGE

            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////
            /// ///////////////////////////////////////////////////

            Console.WriteLine("\n\n------------------------------------------------------");
            Console.WriteLine("Second Test Stage / Descending Order Data");
            Console.WriteLine("------------------------------------------------------\n");

            Console.WriteLine("Creating data sets. Please wait...\n");

            setDepth = 1;

            test.shellIndex = 0;
            test.heapIndex = 0;

            firstDataSet = generateRandomDataSet(500000, true);
            secondDataSet = generateRandomDataSet(1000000, true);
            thirdDataSet = generateRandomDataSet(5000000, true);
            fourthDataSet = generateRandomDataSet(10000000, true);
            fifthDataSet = generateRandomDataSet(25000000, true);


            Console.WriteLine("Finished.\n");
            Console.WriteLine("------------------------------------------------------\n");


            Console.WriteLine("Press any key to start...\n\n\n\n\n\n\n");
            Console.ReadKey();

            //

            test.sortDataSet(firstDataSet, setDepth, testAmount, shell, 1);
            test.sortDataSet(secondDataSet, setDepth, testAmount, shell, 2);
            test.sortDataSet(thirdDataSet, setDepth, testAmount, shell, 3);
            test.sortDataSet(fourthDataSet, setDepth, testAmount, shell, 4);
            test.sortDataSet(fifthDataSet, setDepth, testAmount, shell, 5);

            //

            test.sortDataSet(firstDataSet, setDepth, testAmount, heap, 1);
            test.sortDataSet(secondDataSet, setDepth, testAmount, heap, 2);
            test.sortDataSet(thirdDataSet, setDepth, testAmount, heap, 3);
            test.sortDataSet(fourthDataSet, setDepth, testAmount, heap, 4);
            test.sortDataSet(fifthDataSet, setDepth, testAmount, heap, 5);

            //

            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("     TEST RESULTS");
            Console.WriteLine("------------------------------------------------------\n");

            Console.WriteLine("--------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|\tShell Sort \t\t\t\t|\tHeap Sort");
            Console.WriteLine("|\t\t\t\t\t\t|\t");

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("|\tData Set " + (i + 1) + "  Average:  " + ((float)Math.Round(test.shellSetAvg[i] * 100f) / 100f) + "    ms \t|\t" + "Data Set " + (i + 1) + "  Average:  " + ((float)Math.Round(test.heapSetAvg[i] * 100f) / 100f) + "   ms");
            }

            Console.WriteLine("--------------------------------------------------------------------------------------------------------\n\n\n\n\n\n");
            //

        }

    }

}