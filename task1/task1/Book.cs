using System;
using System.Linq;

public class Book
{
    private string _title;
    private string _author;
    private int _year;
    private string _genre;
    private string _isbn;
    private int _pageCount;
    private double _price;
    private bool _isAvailable;

    public Book()
    {
        _title = "Неизвестно";
        _author = "Неизвестен";
        _year = DateTime.Now.Year;
        _genre = "Не указан";
        _isbn = "000-0-00-000000-0";
        _pageCount = 1;
        _price = 0;
        _isAvailable = false;
    }

    public Book(string title, string author, int year, string genre,
                string isbn, int pageCount, double price, bool isAvailable)
    {
        _title = "Неизвестно";
        _author = "Неизвестен";
        _year = DateTime.Now.Year;
        _genre = "Не указан";
        _isbn = "000-0-00-000000-0";
        _pageCount = 1;
        _price = 0;
        _isAvailable = false;

        Title = title;
        Author = author;
        Year = year;
        Genre = genre;
        ISBN = isbn;
        PageCount = pageCount;
        Price = price;
        IsAvailable = isAvailable;
    }

    public string Title
    {
        get
        {
            return _title;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Название книги не может быть пустым.");
            }
            _title = value;
        }
    }

    public string Author
    {
        get
        {
            return _author;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Автор книги не может быть пустым.");
            }
            _author = value;
        }
    }

    public int Year
    {
        get
        {
            return _year;
        }
        set
        {
            int currentYear = DateTime.Now.Year;
            if (value < 868 || value > currentYear)
            {
                throw new ArgumentException(
                    $"Год издания должен быть в диапазоне от 868 до {currentYear}.");
            }
            _year = value;
        }
    }

    public string Genre
    {
        get
        {
            return _genre;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Жанр книги не может быть пустым.");
            }
            _genre = value;
        }
    }

    public string ISBN
    {
        get 
        { 
            return _isbn; 
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ISBN не может быть пустым.");
            }

            if (!value.All(c => char.IsDigit(c) || c == '-'))
            {
                throw new ArgumentException("ISBN может содержать только цифры и дефисы.");
            }

            string digitsOnly = new string(value.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length != 10 && digitsOnly.Length != 13)
            {
                throw new ArgumentException("ISBN должен содержать 10 или 13 цифр (без учёта дефисов).");
            }

            _isbn = value;
        }
    }

    public int PageCount
    {
        get
        {
            return _pageCount;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Количество страниц должно быть больше нуля.");
            }
            _pageCount = value;
        }
    }

    public double Price
    {
        get
        {
            return _price;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Цена не может быть отрицательной.");
            }
            _price = value;
        }
    }

    public bool IsAvailable
    {
        get
        {
            return _isAvailable;
        }
        set
        {
            _isAvailable = value;
        }
    }

    public override string ToString()
    {
        return $"ISBN: {_isbn,-17} | {_title,-35} | {_author,-24} | {_year,-6} | {_genre,-21} | " +
               $"Стр.: {_pageCount,-6} | Цена: {_price,10:F2} руб. | В наличии: {(_isAvailable ? "Да" : "Нет")}";
    }
}