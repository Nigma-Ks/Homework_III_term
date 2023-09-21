using System.Text;

if (args.Length != 2)
{
    Console.WriteLine("Для работы этой программы нужно ввести пути до двух файлов, введено меньше или больше");
    return;
}

string firstMatrixFilePath = args[0];
string secondMatrixFilePath = args[1];

if (!File.Exists(firstMatrixFilePath))
{
    throw new ArgumentException("Путь к первому файлу некорректен!");
}

if (!File.Exists(secondMatrixFilePath))
{
    throw new ArgumentException("Путь ко второму файлу некорректен!");
}

Matrix matrix1 = new(firstMatrixFilePath);
Matrix matrix2 = new(secondMatrixFilePath);
Console.Write("First matrix:\n" + matrix1.PrintedMatrix());
Console.Write("Second matrix:\n" + matrix2.PrintedMatrix());

Matrix matrix3 = Matrix.MultiThreadedMultiplyingMatrixes(matrix1, matrix2);
Console.Write("Result of multiplication:\n" + matrix2.PrintedMatrix());
string matrix3FilePath = "..//..//..//ResultMatrix.txt";

using (FileStream fs = File.Create(matrix3FilePath))
{
    if (!File.Exists(matrix3FilePath))
    {
        throw new ArgumentException("Файл не был создан");
    }
    byte[] info = new UTF8Encoding(true).GetBytes(matrix3.PrintedMatrix());
    fs.Write(info, 0, info.Length);
}
