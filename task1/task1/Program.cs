using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string filePath = CheckInput.ReadFilePath(
            "Введите полный путь к файлу каталога (или имя файла для текущей папки): ");
        BookCatalog catalog = new BookCatalog(filePath);
        catalog.LoadFromFile();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n===== КАТАЛОГ КНИГ =====");
            Console.WriteLine("1. Показать все книги");
            Console.WriteLine("2. Добавить книгу");
            Console.WriteLine("3. Удалить книгу по ISBN");
            Console.WriteLine("4. LINQ-запросы");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    catalog.PrintAll();
                    break;
                case "2":
                    AddBookInteractive(catalog);
                    break;
                case "3":
                    RemoveBookInteractive(catalog);
                    break;
                case "4":
                    LinqQueriesMenu(catalog);
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                    break;
            }
        }
    }

    private static void AddBookInteractive(BookCatalog catalog)
    {
        try
        {
            string title = CheckInput.ReadNonEmptyString("Название: ");
            string author = CheckInput.ReadNonEmptyString("Автор: ");
            int year = CheckInput.ReadPositiveInt("Год издания: ");
            string genre = CheckInput.ReadNonEmptyString("Жанр: ");
            string isbn = CheckInput.ReadNonEmptyString("ISBN: ");
            int pages = CheckInput.ReadPositiveInt("Количество страниц: ");
            double price = CheckInput.ReadPositiveDouble("Цена (руб.): ");
            bool isAvailable = CheckInput.ReadBoolean("В наличии (да/нет): ");

            var newBook = new Book(title, author, year, genre, isbn, pages, price, isAvailable);
            catalog.AddBook(newBook);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void RemoveBookInteractive(BookCatalog catalog)
    {
        try
        {
            string isbn = CheckInput.ReadNonEmptyString("Введите ISBN удаляемой книги: ");
            catalog.RemoveByISBN(isbn);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void LinqQueriesMenu(BookCatalog catalog)
    {
        Console.WriteLine("\n--- LINQ-ЗАПРОСЫ ---");
        Console.WriteLine("1. Книги по автору");
        Console.WriteLine("2. Доступные книги дешевле N");
        Console.WriteLine("3. Общая стоимость всех книг");
        Console.WriteLine("4. Среднее число страниц по жанру");
        Console.Write("Выберите запрос: ");
        string q = Console.ReadLine();

        switch (q)
        {
            case "1":
                string author = CheckInput.ReadNonEmptyString("Введите имя автора (или часть): ");
                List<Book> booksByAuthor = catalog.GetBooksByAuthor(author);
                if (booksByAuthor.Count == 0)
                {
                    Console.WriteLine("Книги не найдены.");
                }
                else
                {
                    foreach (Book b in booksByAuthor)
                    {
                        Console.WriteLine(b);
                    }
                }
                break;
            case "2":
                double maxPrice = CheckInput.ReadPositiveDouble("Максимальная цена: ");
                List<Book> cheapBooks = catalog.GetAvailableCheaperThan(maxPrice);
                if (cheapBooks.Count == 0)
                {
                    Console.WriteLine("Нет доступных книг, удовлетворяющих условию.");
                }
                else
                {
                    foreach (Book b in cheapBooks)
                    {
                        Console.WriteLine(b);
                    }
                }
                break;
            case "3":
                double total = catalog.GetTotalPrice();
                Console.WriteLine($"Общая стоимость всех книг: {total:F2} руб.");
                break;
            case "4":
                string genre = CheckInput.ReadNonEmptyString("Жанр: ");
                double avgPages = catalog.GetAveragePagesByGenre(genre);
                if (avgPages == 0)
                {
                    Console.WriteLine($"Нет книг жанра «{genre}».");
                }
                else
                {
                    Console.WriteLine($"Среднее количество страниц в жанре «{genre}»: {avgPages:F1}");
                }
                break;
            default:
                Console.WriteLine("Неверный пункт.");
                break;
        }
    }
}