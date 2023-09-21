namespace Homework1.Tests;

public class Tests
{
    Matrix matrix1;
    Matrix matrix2;
    [SetUp]
    public void Setup()
    {
        matrix1 = new(new int[2, 3] { { 1, 2, 3 }, { 3, 5, 6 } }, 2, 3);
        matrix2 = new(new int[3, 2] { { 1, 2 }, { 3, 5 }, { 3, 0 } }, 3, 2);
    }

    [Test]
    public void ExceptionsTest()
    {
        Assert.Throws<ArgumentNullException>(() => new Matrix(null));
        Assert.Throws<ArgumentNullException>(() => Matrix.MultiThreadedMultiplyingMatrixes(null, matrix1));
        Assert.Throws<ArgumentNullException>(() => Matrix.MultiThreadedMultiplyingMatrixes(matrix1, null));
        Assert.Throws<ArgumentException>(() => Matrix.MultiThreadedMultiplyingMatrixes(matrix1, matrix1));
        Assert.Throws<ArgumentNullException>(() => Matrix.SequentialMultiplyingMatrixes(null, matrix1));
        Assert.Throws<ArgumentNullException>(() => Matrix.SequentialMultiplyingMatrixes(matrix1, null));
        Assert.Throws<ArgumentException>(() => Matrix.SequentialMultiplyingMatrixes(matrix1, matrix1));
    }

    [Test]
    public void EqualCalculationTest()
    {
        Matrix matrix3 = Matrix.SequentialMultiplyingMatrixes(matrix1, matrix2);
        Matrix matrix4 = Matrix.MultiThreadedMultiplyingMatrixes(matrix1, matrix2);
        Assert.That(matrix4.Height, Is.EqualTo(matrix3.Height));
        Assert.That(matrix4.Width, Is.EqualTo(matrix3.Width));
        for (int i = 0; i < matrix4.Height; i++)
        {
            for (int j = 0; j < matrix3.Width; j++)
            {
                Assert.That(matrix4.matrixOfNumbers[i, j], Is.EqualTo(matrix3.matrixOfNumbers[i, j]));
            }
        }
    }

    [Test]
    public void RightCalculationTest()
    {
        Matrix matrix3 = Matrix.SequentialMultiplyingMatrixes(matrix1, matrix2);
        Matrix matrix4 = new(new int[2, 2] { { 16, 12 }, { 36, 31} }, 2, 2);
        Assert.That(matrix4.Height, Is.EqualTo(matrix3.Height));
        Assert.That(matrix4.Width, Is.EqualTo(matrix3.Width));
        for (int i = 0; i < matrix4.Height; i++)
        {
            for (int j = 0; j < matrix3.Width; j++)
            {
                Assert.That(matrix4.matrixOfNumbers[i, j], Is.EqualTo(matrix3.matrixOfNumbers[i, j]));
            }
        }
    }
}
