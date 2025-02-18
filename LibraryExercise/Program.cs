using System;
using System.Collections.Generic;
namespace Library
{
  class Program
  {
    static void Main(string[] args)
    {
      UI.Run();
    }
  }

  /// <summary>
  /// this class represents the user interface
  /// contains the main method that runs the program
  /// </summary>
  class UI
  {
    /// This variable keeps track of whether the program is running
    static bool running = true;
    static readonly Library library = new();

    /// <summary>
    /// This method displays the menu
    /// </summary>
    static void DisplayMenu()
    {
      Console.WriteLine("");
      Console.WriteLine("1. Add a book");
      Console.WriteLine("2. Remove a book");
      Console.WriteLine("3. Search for a book");
      Console.WriteLine("4. List all books");
      Console.WriteLine("5. Exit");
    }

    /// <summary>
    /// This method exits the program and announces it
    /// </summary>
    static void Exit()
    {
      Console.WriteLine("Exiting program!");
      Environment.Exit(0);
    }

    public static void Run()
    {

      while (running)
      {
        DisplayMenu();
        GetUserInput();
      }
      Exit();
    }
    /// <summary>
    /// This method gets the user input and calls the method that corresponds to the input
    /// </summary>
    static void GetUserInput()
    {
      Console.WriteLine("Enter your choice: ");
      string input = Console.ReadLine() ?? string.Empty;
      switch (input)
      {
        case "1":
          Book newBook = GetBookFromUserInput();
          library.AddBook(book: newBook);
          break;
        case "2":
          Console.WriteLine("Enter the isbn of the book you want to remove: ");
          library.RemoveBook(isbn: Console.ReadLine() ?? "");
          break;
        case "3":
          Console.WriteLine("Enter your search query: ");
          List<Book> foundBooks = library.SearchBooks(query: Console.ReadLine() ?? string.Empty) ?? [];
          if (foundBooks.Count == 0)
          {
            Console.WriteLine("No books found matching the query");
          }
          else
          {
            foreach (Book book in foundBooks)
            {
              book.DisplayDetails();
            }
          }
          break;
        case "4":
          int sortingOption = GetValidSortingOption();
          library.ListBooks(sortingOption);
          break;
        case "5":
          running = false;
          break;
        default:
          Console.WriteLine("Invalid input, please try again");
          GetUserInput();
          break;
      }
    }

    /// <summary>
    /// This method gets a book from the user input
    /// checks if the year is valid
    /// Also checks if the book is an ebook and if it is, makes sure that the file size is valid
    /// </summary>
    /// <returns></returns>
    static Book GetBookFromUserInput()
    {
      Console.WriteLine("Enter the title of the book: ");
      string title = Console.ReadLine() ?? string.Empty;
      Console.WriteLine("Enter the author of the book: ");
      string author = Console.ReadLine() ?? string.Empty;
      Console.WriteLine("Enter the isbn of the book: ");
      string isbn = Console.ReadLine() ?? string.Empty;
      int publicationYear = GetValidYear();
      Console.WriteLine("Enter the genre of the book: ");
      string genre = Console.ReadLine() ?? string.Empty;
      Console.WriteLine("Is this an ebook? (y/n)");
      string isEbook = Console.ReadLine() ?? string.Empty;
      if (isEbook == "y")
      {
        int fileSize = GetValidFileSize();
        return new EBook(title, author, isbn, publicationYear, genre, fileSize);
      }
      return new Book(title, author, isbn, publicationYear, genre);
    }

    /// <summary>
    /// This method gets a valid year from the user
    /// </summary>
    /// <returns></returns>
    static int GetValidYear()
    {
      bool validYear = false;
      int publicationYear = 0;
      while (!validYear)
      {
        Console.WriteLine("Enter the publication year of the book: ");
        validYear = int.TryParse(Console.ReadLine(), out int result);
        if (!validYear)
        {
          Console.WriteLine("Please enter a valid year");
        }
        else
        {
          publicationYear = result;
        }
      }
      return publicationYear;
    }

    /// <summary>
    /// This method gets a valid file size from the user
    /// </summary>
    /// <returns></returns>
    static int GetValidFileSize()
    {
      bool validFileSize = false;
      int fileSize = 0;
      while (!validFileSize)
      {
        Console.WriteLine("Enter the file size in mb of the book: ");
        validFileSize = int.TryParse(Console.ReadLine(), out int result);
        if (!validFileSize)
        {
          Console.WriteLine("Please enter a valid file size");
        }
        else
        {
          fileSize = result;
        }
      }
      return fileSize;
    }
    public static void ShowSortingOptions()
    {
      Console.WriteLine("1. Sort by title");
      Console.WriteLine("2. Sort by author");
      Console.WriteLine("3. Sort by publication year");
    }

    public static int GetValidSortingOption()
    {
      bool validSortingOption = false;
      int sortingOption = 0;
      while (!validSortingOption)
      {
        ShowSortingOptions();
        Console.WriteLine("Enter your choice to sort by: ");
        validSortingOption = int.TryParse(Console.ReadLine(), out int result) && result > 0 && result < 4;
        if (!validSortingOption)
        {
          Console.WriteLine("Please enter a valid sorting option");
        }
        else
        {
          sortingOption = result;
        }
      }
      return sortingOption;
    }
  }


  interface IReadable
  {
    void Read();
  }

  /// <summary>
  /// Class representing an ebook, extends the book class
  /// </summary>
  /// <param name="title"></param>
  /// <param name="author"></param>
  /// <param name="isbn"></param>
  /// <param name="publicationYear"></param>
  /// <param name="genre"></param>
  /// <param name="fileSize"></param>
  class EBook(string title, string author, string isbn, int publicationYear, string genre, int fileSize) : Book(title, author, isbn, publicationYear, genre)
  {
    public int fileSize = fileSize;

    public override void DisplayDetails()
    {
      Console.WriteLine($"{title} by {author} is a {genre} type book published in {publicationYear} with isbn {isbn}, its filesize is {fileSize}mb");
    }
  }

  /// <summary>
  /// Class representing a library
  /// </summary>
  class Library
  {
    List<Book> Books;
    public Library()
    {
      Books = [];
    }

    /// <summary>
    /// Adds a book to the library
    /// </summary>
    /// <param name="book"></param>
    public void AddBook(Book book)
    {
      Books.Add(book);
      Console.WriteLine($"successfully added book {book.title} to the library");
    }

    /// <summary>
    /// Removes a book from the library
    /// </summary>
    /// <param name="isbn"></param>
    public void RemoveBook(string isbn)
    {
      int removedBooks = 0;
      foreach (Book book in Books)
      {
        if (book.isbn == isbn)
        {
          Books.Remove(book);
          Console.WriteLine($"successfully removed book {book.DisplayDetails} from the library");
          removedBooks++;
        }
      }
      if (removedBooks == 0)
      {
        Console.WriteLine("No books found with that isbn, so no books were removed");
      }
      else
      {
        Console.WriteLine($"successfully removed {removedBooks} books from the library");
      }
    }

    /// <summary>
    /// Searches the library for books matching the query
    /// </summary>
    /// <param name="query"></param>
    /// <returns>A list of books matching the query</returns>
    public List<Book>? SearchBooks(string query)
    {
      List<Book> matches = [];
      foreach (Book book in Books)
      {
        if (book.IsMatch(query))
        {
          matches.Add(book);
        }
      }
      return matches;
    }

    /// <summary>
    /// Prints the books in alphabetical order
    /// </summary>
    public void ListBooks(int sortType)
    {
      switch (sortType)
      {
        case 1:
          Books.Sort(CompareBooksByTitle);
          break;
        case 2:
          Books.Sort(CompareBooksByAuthor);
          break;
        case 3:
          Books.Sort(CompareBooksByPublicationYear);
          break;
        default:
          Books.Sort(CompareBooksByTitle);
          break;
      }
      foreach (Book book in Books)
      {
        book.DisplayDetails();
      }
    }



    /// <summary>
    /// Compares two books based on their their title
    /// </summary>
    /// <param name="bookX">the first book</param>
    /// <param name="bookY">the second book</param>
    /// <returns>An integer representing which book comes first: 1 if X comes first, -1 if Y comes first, 0 if they are equal </returns>
    public static int CompareBooksByTitle(Book bookX, Book bookY)
    {
      // check for null first
      if (bookX.title == null)
      {
        // if both are null, they are the same
        if (bookY.title == null)
        {
          return 0;
        }
        // if title of y is not null, y comes first
        else
        {
          return -1;
        }
      }
      else
      {
        // if title of x is not null, but y's title is, then x comes first
        if (bookY.title == null)
        {
          return 1;
        }
        // check which title comes first 
        else
        {
          int result = bookX.title.CompareTo(bookY.title);
          return result;

        }
      }
    }
    /// <summary>
    /// Compares two books based on their their Author
    /// </summary>
    /// <param name="bookX"></param>
    /// <param name="bookY"></param>
    /// <returns></returns>
    public static int CompareBooksByAuthor(Book bookX, Book bookY)
    {
      // check for null first
      if (bookX.author == null)
      {
        // if both are null, they are the same
        if (bookY.author == null)
        {
          return 0;
        }
        // if author of y is not null, y comes first
        else
        {
          return -1;
        }
      }
      else
      {
        // if author of x is not null, but y's author is, then x comes first
        if (bookY.author == null)
        {
          return 1;
        }
        // check which author comes first 
        else
        {
          int result = bookX.author.CompareTo(bookY.author);
          return result;
        }
      }
    }
    /// <summary>
    /// Compares two books based on their publication year
    /// </summary>
    /// <param name="bookX"></param>
    /// <param name="bookY"></param>
    /// <returns></returns>
    public static int CompareBooksByPublicationYear(Book bookX, Book bookY)
    {
      // check for null first
      if (bookX.publicationYear == null)
      {
        // if both are null, they are the same
        if (bookY.publicationYear == null)
        {
          return 0;
        }
        // if publicationYear of y is not null, y comes first
        else
        {
          return -1;
        }
      }
      else
      {
        // if publicationYear of x is not null, but y's publicationYear is, then x comes first
        if (bookY.publicationYear == null)
        {
          return 1;
        }
        // check which publicationYear comes first 
        else
        {
          return bookX.publicationYear.CompareTo(bookY.publicationYear);
        }
      }
    }
  }

  /// <summary>
  /// Class representing a book
  /// </summary>
  /// <param name="title"></param>
  /// <param name="author"></param>
  /// <param name="isbn"></param>
  /// <param name="publicationYear"></param>
  /// <param name="genre"></param>
  public class Book(string title, string author, string isbn, int publicationYear, string genre) : IReadable
  {
    public string title = title;
    public string author = author;
    public string isbn = isbn;
    public int publicationYear = publicationYear;
    public string genre = genre;

    /// <summary>
    /// This method checks if the title or autho of the book matches the search query
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    public bool IsMatch(string searchQuery)
    {
      // Convert to lower case to allow search while ignoring capitalization in the query.
      if (title.ToLower().Contains(searchQuery.ToLower()) || author.ToLower().Contains(searchQuery.ToLower()))
      {
        return true;
      }
      return false;
    }
    /// <summary>
    /// This method displays the details of the book
    /// </summary>
    public virtual void DisplayDetails()
    {
      Console.WriteLine($"{title} by {author} is a {genre} type book published in {publicationYear} with isbn {isbn}");
    }
    /// <summary>
    /// This method reads the book
    /// </summary>
    public void Read()
    {
      Console.WriteLine($"reading {title} by {author} from {publicationYear}");
    }
  }
}