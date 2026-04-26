using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BookCatalog
{
    private readonly string _filePath;
    private List<Book> _books;

    public BookCatalog(string filePath)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }
        _filePath = filePath;
    }

    public void LoadFromFile()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine("Файл базы данных не найден. " +
                "Будет создан новый каталог.");
            _books = new List<Book>();
        }
        else
        {
            try
            {
                FileInfo fi = new FileInfo(_filePath);
                if (fi.Length == 0)
                {
                    Console.WriteLine("Файл пуст. " +
                        "Инициализирован пустой каталог.");
                    _books = new List<Book>();
                }
                else
                {
                    using (BinaryReader reader = 
                        new BinaryReader(File.Open(_filePath, FileMode.Open)))
                    {
                        if (reader.BaseStream.Length < sizeof(int))
                        {
                            Console.WriteLine("Файл повреждён: недостаточно данных. " +
                                "Каталог будет пересоздан при сохранении.");
                            _books = new List<Book>();
                        }
                        else
                        {
                            int count = reader.ReadInt32();
                            _books = new List<Book>(count);

                            for (int i = 0; i < count; i++)
                            {
                                if (reader.BaseStream.Position >= reader.BaseStream.Length)
                                {
                                    Console.WriteLine($"Файл повреждён: " +
                                        $"записей заявлено {count}, " +
                                        $"но данные закончились. " +
                                        $"Загружено {_books.Count} книг.");
                                    break;
                                }

                                Book book = new Book(
                                    title: reader.ReadString(),
                                    author: reader.ReadString(),
                                    year: reader.ReadInt32(),
                                    genre: reader.ReadString(),
                                    isbn: reader.ReadString(),
                                    pageCount: reader.ReadInt32(),
                                    price: reader.ReadDouble(),
                                    isAvailable: reader.ReadBoolean()
                                    );
                                _books.Add(book);
                            }
                            Console.WriteLine($"Загружено {_books.Count} записей.");
                        }
                    }
                }
            }
            catch (EndOfStreamException ex)
            {
                Console.WriteLine($"Файл повреждён (неожиданный конец): " +
                    $"{ex.Message}. Каталог будет пересоздан при сохранении.");
                _books = new List<Book>();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}. " +
                    $"Возможно, файл занят другим процессом или повреждён.");
                _books = new List<Book>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке файла: " +
                    $"{ex.Message}. Будет использован пустой каталог.");
                _books = new List<Book>();
            }
        }
    }

    public void SaveToFile()
    {
        try
        {
            using (BinaryWriter writer = 
                new BinaryWriter(File.Open(_filePath, FileMode.Create)))
            {
                writer.Write(_books.Count);
                foreach (Book book in _books)
                {
                    writer.Write(book.Title);
                    writer.Write(book.Author);
                    writer.Write(book.Year);
                    writer.Write(book.Genre);
                    writer.Write(book.ISBN);
                    writer.Write(book.PageCount);
                    writer.Write(book.Price);
                    writer.Write(book.IsAvailable);
                }
            }
            Console.WriteLine("База данных успешно сохранена.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
        }
    }

    public void PrintAll()
    {
        if (_books.Count == 0)
        {
            Console.WriteLine("Каталог пуст.");
        }
        else
        {
            Console.WriteLine("\n=== КАТАЛОГ КНИГ ===");
            Console.WriteLine(new string('-', 180));
            foreach (Book book in _books)
            {
                Console.WriteLine(book.ToString());
            }
            Console.WriteLine(new string('-', 180));
        }
    }

    public void AddBook(Book newBook)
    {
        if (newBook == null)
        {
            throw new ArgumentNullException(nameof(newBook));
        }

        if (_books.Any(b => b.ISBN == newBook.ISBN))
        {
            throw new InvalidOperationException($"Книга с ISBN " +
                $"'{newBook.ISBN}' уже существует.");
        }

        _books.Add(newBook);
        Console.WriteLine("Книга успешно добавлена.");
        SaveToFile();
    }

    public void RemoveByISBN(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            throw new ArgumentException("ISBN не может быть пустым.");
        }

        var bookToRemove = _books.FirstOrDefault(b => b.ISBN.Equals(
            isbn, StringComparison.OrdinalIgnoreCase));
        if (bookToRemove == null)
        {
            Console.WriteLine($"Книга с ISBN '{isbn}' не найдена.");
        }
        else
        {
            _books.Remove(bookToRemove);
            Console.WriteLine($"Книга '{bookToRemove.Title}' удалена.");
            SaveToFile();
        }
    }





    public List<Book> GetBooksByAuthor(string author)
    {
        return _books.Where(b => b.Author.IndexOf(
            author, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
    }

    public List<Book> GetAvailableCheaperThan(double maxPrice)
    {
        return _books.Where(b => b.IsAvailable && b.Price < maxPrice)
                        .OrderBy(b => b.Price)
                        .ToList();
    }

    public double GetTotalPrice()
    {
        return _books.Sum(b => b.Price);
    }

    public double GetAveragePagesByGenre(string genre)
    {
        return _books.Where(b => b.Genre.Equals(
            genre, StringComparison.OrdinalIgnoreCase))
                        .Average(b => (double?)b.PageCount) ?? 0;
    }
}