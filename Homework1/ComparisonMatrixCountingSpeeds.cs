using System.Diagnostics;

public static class ComparisonMatrixCountingSpeeds
{
    public static (double, double)[] Experiments(int sizeOfData, int amountOfTestsInSample) //size of data, mat ozgidanie, dispersiia
    {
        Random random = new Random();
        List<long> timeByThreads = new();
        List<long> timeSequential = new();
        for (int testNumber = 0; testNumber < amountOfTestsInSample; testNumber++)
        {
            int[,] numbersInMatrix1 = createSquareArrayWithRandomValues(sizeOfData);
            int[,] numbersInMatrix2 = createSquareArrayWithRandomValues(sizeOfData);
            Matrix matrix1 = new(numbersInMatrix1, sizeOfData, sizeOfData);
            Matrix matrix2 = new(numbersInMatrix2, sizeOfData, sizeOfData);
            timeByThreads.Add(timeOfMultiThreadedMultiplying(matrix1, matrix2));
            timeSequential.Add(timeOfSequentialMultiplying(matrix1, matrix2));
        }
        double? mathematicalExpectationByThreads = timeByThreads.Average();
        double? mathematicalExpectationSequential = timeSequential.Average();
        double dispersionByThreads = dispersionCalculator(timeByThreads, mathematicalExpectationByThreads.Value);
        double dispersionSequential = dispersionCalculator(timeSequential, mathematicalExpectationSequential.Value);
        return new (double, double)[] { (mathematicalExpectationByThreads.Value, dispersionByThreads), (mathematicalExpectationSequential.Value, dispersionSequential)};
    }

    private static double dispersionCalculator(List<long> time, double mathematicalExpectation)
    {
        double sumOfSquareDifference = 0;
        for (int i = 0; i < time.Count; i++)
        {
            double difference = time[i] - mathematicalExpectation;
            sumOfSquareDifference += difference * difference;
        }
        return sumOfSquareDifference / time.Count;
    }
    private static int[,] createSquareArrayWithRandomValues(int size)
    {
        Random random = new Random();
        var array = new int[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                array[i, j] = random.Next();
            }
        }
        return array;
    }

    private static long timeOfMultiThreadedMultiplying(Matrix matrix1, Matrix matrix2)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Matrix matrix3 = Matrix.MultiThreadedMultiplyingMatrixes(matrix1, matrix2);
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }

    private static long timeOfSequentialMultiplying(Matrix matrix1, Matrix matrix2)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Matrix matrix3 = Matrix.SequentialMultiplyingMatrixes(matrix1, matrix2);
        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }
}

