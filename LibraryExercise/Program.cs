using System;
using System.Collections.Generic;
namespace Library
{
  class Program
  {
    static void Main(string[] args)
    {
      Book x = new Book("hoi", "Thom", "12-34", 2000, "non-fictie biografie");
      var y = new Book("hallo", "Hans", "21-43", 2001, "fictie horror");
      var z = new Book("de grote boze wolf", "Harrie", "43-21", 2002, "fictie fantasy");

      Library library = new Library();
      library.AddBook(x);
      library.AddBook(y);
      library.AddBook(z);
      library.ListBooks();
      Console.WriteLine("End of Main!");
    }
  }

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
      this.Books.Add(book);
      Console.WriteLine($"successfully added book ${book.title} to the library");
    }

    /// <summary>
    /// Removes a book from the library
    /// </summary>
    /// <param name="isbn"></param>
    public void RemoveBook(string isbn)
    {
      foreach (Book book in Books)
      {
        if (book.isbn == isbn)
        {
          Books.Remove(book);
        }
      }
    }

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

    public void ListBooks()
    {
      Books.Sort(CompareBooksByTitle);
      foreach (Book book in Books)
      {
        Console.WriteLine($"${book.title} by ${book.author} is a ${book.genre} type book published in ${book.publicationYear} with isbn ${book.isbn}");
      }
    }

/// <summary>
/// Compares two books based on their their title
/// </summary>
/// <param name="bookX">the first book</param>
/// <param name="bookY">the second book</param>
/// <returns>An integer representing which book comes first: 1 if X comes first, -1 if Y comes first, 0 if they are equal </returns>

    private static int CompareBooksByTitle(Book bookX, Book bookY)
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
  }

  public class Book(string title, string author, string isbn, int publicationYear, string genre)
  {
    public string title = title;
    public string author = author;
    public string isbn = isbn;
    public int publicationYear = publicationYear;
    public string genre = genre;

    // return true if title or author contains {searchQuery} else returns false
    public bool IsMatch(string searchQuery)
    {
      if (title.Contains(searchQuery) || author.Contains(searchQuery))
      {
        return true;
      }
      return false;
    }
  }
}