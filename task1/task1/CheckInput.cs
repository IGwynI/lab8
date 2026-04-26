using System;
using System.IO;

public static class CheckInput
{
    /// <summary>
    /// Ввод целого числа.
    /// </summary>
    public static int ReadInt(string message)
    {
        int result = 0;
        bool valid = false;
        while (!valid)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            if (int.TryParse(input, out result))
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Ошибка: введите целое число.");
            }
        }
        return result;
    }

    /// <summary>
    /// Ввод положительного целого числа (>0).
    /// </summary>
    public static int ReadPositiveInt(string message)
    {
        int number;
        bool valid = false;
        do
        {
            number = ReadInt(message);
            if (number > 0)
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Ошибка: число должно быть больше 0.");
            }
        } while (!valid);
        return number;
    }

    /// <summary>
    /// Ввод неотрицательного целого числа (>=0).
    /// </summary>
    public static int ReadNonNegativeInt(string message)
    {
        int number;
        bool valid = false;
        do
        {
            number = ReadInt(message);
            if (number >= 0)
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Ошибка: число не может быть отрицательным.");
            }
        } while (!valid);
        return number;
    }

    /// <summary>
    /// Ввод числа с плавающей точкой (double).
    /// </summary>
    public static double ReadDouble(string message)
    {
        double result = 0;
        bool valid = false;
        while (!valid)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            if (double.TryParse(input, out result))
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Ошибка: введите число (можно с дробной частью).");
            }
        }
        return result;
    }

    /// <summary>
    /// Ввод положительного double (>0).
    /// </summary>
    public static double ReadPositiveDouble(string message)
    {
        double number;
        bool valid = false;
        do
        {
            number = ReadDouble(message);
            if (number > 0)
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Ошибка: число должно быть больше 0.");
            }
        } while (!valid);
        return number;
    }

    /// <summary>
    /// Ввод непустой строки.
    /// </summary>
    public static string ReadNonEmptyString(string message)
    {
        string input;
        bool valid = false;
        do
        {
            Console.Write(message);
            input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
            {
                valid = true;
            }
            else
            {
                Console.WriteLine("Ошибка: ввод не может быть пустым.");
            }
        } while (!valid);
        return input;
    }

    /// <summary>
    /// Ввод логического значения.
    /// </summary>
    public static bool ReadBoolean(string message)
    {
        string input;
        bool valid = false;
        while (!valid)
        {
            Console.Write(message);
            input = Console.ReadLine()?.Trim().ToLower();
            if (input == "да" || input == "yes" || input == "y")
            {
                return true;
            }
            else if (input == "нет" || input == "no" || input == "n")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Ошибка: введите 'да' или 'нет'.");
            }
        }
        return false;
    }

    /// <summary>
    /// Ввод пути к файлу с проверкой существования директории
    /// Если директория не существует – запрос повторяется
    /// Если файла нет – выдаётся предупреждение, но путь принимается
    /// </summary>
    public static string ReadFilePath(string message)
    {
        string path = "";
        bool valid = false;
        while (!valid)
        {
            Console.Write(message);
            path = Console.ReadLine()?.Trim();

            if (path.Length >= 2 && path[0] == '"' && path[path.Length - 1] == '"')
            {
                path = path.Substring(1, path.Length - 2);
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine("Ошибка: путь не может быть пустым.");
                continue;
            }

            string fullPath = Path.GetFullPath(path);
            string directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Ошибка: директория '{directory}' не существует. Укажите корректный путь.");
            }
            else
            {
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Внимание: файл '{fullPath}' не найден. При сохранении будет создан новый файл.");
                }
                valid = true;
            }
        }
        return path;
    }
}