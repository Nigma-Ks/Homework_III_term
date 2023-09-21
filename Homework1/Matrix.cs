
/// <summary>
/// the matrix class implements methods:
/// matrix representation as a row, matrix multiplication by threads and sequential
/// </summary>
public class Matrix
{
    public int[,] matrixOfNumbers { get; private set; }
    public int Height { get; private set; } = 0;
    public int Width { get; private set; } = 0;
    public Matrix(string filePath)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        string[] matrixInLines = File.ReadAllLines(filePath);
        Height = matrixInLines.Length;
        if (Height < 1)
        {
            throw new ArgumentException("There is no numbers in file");
        }
        string[] parsedFirstLine = matrixInLines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        Width = parsedFirstLine.Length;
        if (Width < 1)
        {
            throw new ArgumentException("There is no numbers in file");
        }
        matrixOfNumbers = new int[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            string line = matrixInLines[i];
            string[] stringOfNumbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (stringOfNumbers.Length != Width)
            {
                throw new ArgumentException("Amount of numbers in line isn't equal");
            }
            if (i == 0)
            {
                Width = stringOfNumbers.Length;
            }
            for (int j = 0; j < Width; j++)
            {
                if (int.TryParse(stringOfNumbers[i], out int currentValue))
                {
                    matrixOfNumbers[i, j] = currentValue;
                }
                else
                {
                    throw new ArgumentException("Not inly numbers in file");
                }
            }
        }
    }

    public Matrix(int[,] matrix, int height, int width)
    {
        matrixOfNumbers = matrix;
        Height = height;
        Width = width;
    }

    /// <summary>
    /// return matrix in string
    /// </summary>
    public string PrintedMatrix()
    {
        string matrixInString = "";
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                matrixInString += matrixOfNumbers[i, j] + ' ';
            }
            matrixInString += '\n';
        }
        return matrixInString;
    }

    /// <summary>
    /// Multiplies matrixes sequential
    /// </summary>
    /// <param name="matrix1"></param>
    /// <param name="matrix2"></param>
    public static Matrix SequentialMultiplyingMatrixes(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1 == null)
        {
            throw new ArgumentNullException(nameof(matrix1));
        }

        if (matrix2 == null)
        {
            throw new ArgumentNullException(nameof(matrix2));
        }

        if (matrix1.Width != matrix2.Height)
        {
            throw new ArgumentException("Matrix sizes are inconsistent");
        }
        var matrix3Numbers = new int[matrix1.Height, matrix2.Width];
        for (int i = 0; i < matrix1.Height; i++)
        {
            for (int j = 0; j < matrix2.Width; j++)
            {
                matrix3Numbers[i, j] = 0;
                for (int k = 0; k < matrix1.Width; k++)
                {
                    matrix3Numbers[i, j] += matrix1.matrixOfNumbers[i, k] * matrix2.matrixOfNumbers[k, j];
                }
            }
        }

        return new Matrix(matrix3Numbers, matrix1.Height, matrix2.Width);
    }

    /// <summary>
    /// Multiplies matrixes by threads
    /// </summary>
    /// <param name="matrix1"></param>
    /// <param name="matrix2"></param>
    public static Matrix MultiThreadedMultiplyingMatrixes(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1 == null)
        {
            throw new ArgumentNullException(nameof(matrix1));
        }

        if (matrix2 == null)
        {
            throw new ArgumentNullException(nameof(matrix2));
        }

        if (matrix1.Width != matrix2.Height)
        {
            throw new ArgumentException("Matrix sizes are inconsistent");
        }
        var matrix3Numbers = new int[matrix1.Height, matrix2.Width];

        var matrixMyltiplyingThreads = new Thread[4];
        for (byte i = 0; i < matrixMyltiplyingThreads.Length; i++)
        {
            int numberOfStartElement = i;
            uint amountOfElementsInMatrix = (uint)matrix1.Height * (uint)matrix2.Width;
            matrixMyltiplyingThreads[i] = new Thread(() =>
            {
                uint currentElement = (uint)numberOfStartElement;
                while (currentElement < amountOfElementsInMatrix)
                {
                    int currentLine = (int)currentElement / matrix2.Width;
                    int currentColumn = (int)currentElement % matrix2.Width;
                    matrix3Numbers[currentLine, currentColumn] = 0;
                    for (int k = 0; k < matrix1.Width; k++)
                    {
                        matrix3Numbers[currentLine, currentColumn] += matrix1.matrixOfNumbers[currentLine, k] * matrix2.matrixOfNumbers[k, currentColumn];
                    }
                    currentElement += (uint)matrixMyltiplyingThreads.Length;
                }
            });
        }

        foreach (var tread in matrixMyltiplyingThreads)
        {
            tread.Start();
        }
        foreach (var tread in matrixMyltiplyingThreads)
        {
            tread.Join();
        }
        return new Matrix(matrix3Numbers, matrix1.Height, matrix2.Width);
    }
}


