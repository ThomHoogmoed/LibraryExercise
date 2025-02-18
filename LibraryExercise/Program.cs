using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

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
    static Library library;

    /// <summary>
    /// This method runs the program
    /// </summary>
    public static void Run()
    {
      bool loggedIn = false;
      while(!loggedIn){
        // TODO: Implement login
        // int authenticationOption = AuthenticationManager.GetAuthenticationOptionFromUserInput();
        // switch (authenticationOption)
        // {
        //   case 1:
      }

      library = LoadLibrary();
      library ??= new Library();

      while (running)
      {
        DisplayMenu();
        GetUserInput();
      }
      SaveLibrary();
      Exit();
    }

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

    /// <summary>
    /// This method saves the library to a file
    /// Throws an exception if an error occurs while saving the library
    /// </summary>
    public static void SaveLibrary()
    {
      try
      {
        // Save the library to a file
        Console.WriteLine("Saving library");
        string fileName = "libraryData.json";
        string jsonString = JsonSerializer.Serialize(library);
        // Console.WriteLine(jsonString);
        File.WriteAllText(fileName, jsonString);
        Console.WriteLine("Library saved");
      }
      catch (Exception e)
      {
        Console.WriteLine("An error occured while saving the library");
        Console.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// This method loads the library from a file
    /// Throws an exception if an error occurs while loading the library
    /// </summary>
    public static Library LoadLibrary()
    {
      try
      {
        // Load the library from a file
        Console.WriteLine("Loading library");
        string fileName = "libraryData.json";
        if (File.Exists(fileName) == false)
        {
          Console.WriteLine("No library data found");
          return new Library();
        }
        string jsonString = File.ReadAllText(fileName);
        if (jsonString == "")
        {
          Console.WriteLine("No library data found");
          return new Library();
        }
        library = JsonSerializer.Deserialize<Library>(jsonString);
        Console.WriteLine("Library loaded");
        return library;
      }
      catch (Exception e)
      {
        Console.WriteLine("An error occured while loading the library");
        Console.WriteLine(e.Message);
        return null;
      }
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
            Console.WriteLine($"Books found matching the query: {foundBooks.Count}");
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

    /// <summary>
    /// This method gets a valid sorting option from the user
    /// </summary>
    /// <returns></returns>
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

  /// <summary>
  /// Class representing a library
  /// </summary>
  class Library
  {
    public List<Book> Books { get; set; }
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
      Console.WriteLine($"successfully added book {book.Title} to the library");
    }

    /// <summary>
    /// Removes a book from the library
    /// </summary>
    /// <param name="isbn"></param>
    public void RemoveBook(string isbn)
    {
      List<int> indexesToRemove = [];
      for (int i = 0; i < Books.Count; i++)
      {
        if (Books[i].Isbn == isbn)
        {
          indexesToRemove.Add(i);
        }
      }
      if (indexesToRemove.Count == 0)
      {
        Console.WriteLine("No books found with that isbn, so no books were removed");
      }
      else
      {
        foreach (int index in indexesToRemove)
        {
          Books.RemoveAt(index);
        }
        Console.WriteLine($"successfully removed {indexesToRemove.Count} books with isbn {isbn}");
      }
    }

    /// <summary>
    /// Searches the library for books matching the query
    /// </summary>
    /// <param name="query"></param>
    /// <returns>A list of books matching the query</returns>
    public List<Book>? SearchBooks(string query)
    {
      return Books.Where(book => book.IsMatch(query)).ToList();
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
      if (bookX.Title == null)
      {
        // if both are null, they are the same
        if (bookY.Title == null)
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
        if (bookY.Title == null)
        {
          return 1;
        }
        // check which title comes first 
        else
        {
          int result = bookX.Title.CompareTo(bookY.Title);
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
      if (bookX.Author == null)
      {
        // if both are null, they are the same
        if (bookY.Author == null)
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
        if (bookY.Author == null)
        {
          return 1;
        }
        // check which author comes first 
        else
        {
          int result = bookX.Author.CompareTo(bookY.Author);
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
      if (bookX.PublicationYear == null)
      {
        // if both are null, they are the same
        if (bookY.PublicationYear == null)
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
        if (bookY.PublicationYear == null)
        {
          return 1;
        }
        // check which publicationYear comes first 
        else
        {
          return bookX.PublicationYear.CompareTo(bookY.PublicationYear);
        }
      }
    }
  }

  /// <summary>
  /// Interface representing a readable object
  /// </summary>
  interface IReadable
  {
    void Read();
  }
  /// <summary>
  /// Class representing a book
  /// </summary>
  /// <param name="title"></param>
  /// <param name="author"></param>
  /// <param name="isbn"></param>
  /// <param name="publicationYear"></param>
  /// <param name="genre"></param>
  public class Book : IReadable
  {
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Isbn { get; set; }
    public required int PublicationYear { get; set; }
    public required string Genre { get; set; }

    [SetsRequiredMembers]
    public Book(string title, string author, string isbn, int publicationYear, string genre)
    {
      Title = title;
      Author = author;
      Isbn = isbn;
      PublicationYear = publicationYear;
      Genre = genre;
    }

    /// <summary>
    /// This method checks if the title or autho of the book matches the search query
    /// </summary>
    /// <param name="searchQuery"></param>
    /// <returns></returns>
    public bool IsMatch(string searchQuery)
    {
      // Convert to lower case to allow search while ignoring capitalization in the query.
      if (Title.ToLower().Contains(searchQuery.ToLower()) || Author.ToLower().Contains(searchQuery.ToLower()))
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
      Console.WriteLine($"{Title} by {Author} is a {Genre} book published in {PublicationYear} with isbn {Isbn}");
    }
    /// <summary>
    /// This method reads the book
    /// </summary>
    public void Read()
    {
      Console.WriteLine($"reading {Title} by {Author} from {PublicationYear}");
    }
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
  public class EBook : Book
  {
    public int FileSize { get; set; }
    [SetsRequiredMembers]
    public EBook(string title, string author, string isbn, int publicationYear, string genre, int fileSize)
        : base(title, author, isbn, publicationYear, genre)
    {
      FileSize = fileSize;
    }
    public override void DisplayDetails()
    {
      Console.WriteLine($"{Title} by {Author} is a {Genre} type book published in {PublicationYear} with isbn {Isbn}, its filesize is {FileSize}mb");
    }
  }

  public class User
  {
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public required string Role { get; set; } = "User";
    [SetsRequiredMembers]
    public User(string username, string password)
    {
      UserName = username;
      Password = password;
    }
  }

  public class Admin : User
  {
    [SetsRequiredMembers]
    public Admin(string username, string password) : base(username, password)
    {
      Role = "Admin";
    }
  }

  class AuthenticationManager
  {
    public static List<User> Users { get; set; }
    /// <summary>
    /// This method initializes the authentication manager by loading the users from a file, 
    /// if no users are found, it creates the initial admin
    /// </summary>
    private void InitializeAuthenticationManager()
    {
      Users = LoadUsers();
      if (Users.Count == 0)
      {
        Console.WriteLine("No users found, creating standard admin");
        CreateInitialAdmin();
      }
    }

    /// <summary>
    /// This method creates the initial admin
    /// </summary>
    private void CreateInitialAdmin()
    {
      Admin admin = new("admin", "1234");
      Users.Add(admin);
    }

    /// <summary>
    /// This method loads the users from a file
    /// </summary>
    /// <returns></returns>
    public static List<User> LoadUsers()
    {
      try
      {
        // Load the users from a file
        Console.WriteLine("Loading users");
        string fileName = "userData.json";
        if (File.Exists(fileName) == false)
        {
          Console.WriteLine("No file with user data found");
          return [];
        }
        string jsonString = File.ReadAllText(fileName);
        if (jsonString == "")
        {
          Console.WriteLine("No user data found");
          return [];
        }
        List<User>? users = JsonSerializer.Deserialize<List<User>>(jsonString);
        Console.WriteLine("users loaded");
        return users ?? [];
      }
      catch (Exception e)
      {
        Console.WriteLine("An error occured while loading the library");
        Console.WriteLine(e.Message);
        return [];
      }
    }

    /// <summary>
    /// This method displays the authentication menu
    /// </summary>
    public static void DisplayAuthenticationMenu()
    {
      Console.WriteLine("1. Login");
      Console.WriteLine("2. Register");
    }

    /// <summary>
    /// This method gets the authentication option from the user input
    /// </summary>
    /// <returns></returns>
    public static int GetAuthenticationOptionFromUserInput()
    {
      bool ValidAuthenticationOption = false;
      string input;
      int result = 0;
      while (ValidAuthenticationOption == false)
      {
        DisplayAuthenticationMenu();
        input = Console.ReadLine() ?? string.Empty;
        if (ValidateAuthenticationOption(input))
        {
          break;
        }
        result = int.Parse(input);
      }
      return result;
    }

    /// <summary>
    /// This method validates the authentication option from the user input
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool ValidateAuthenticationOption(string input)
    {
      //TODO: Implement option to login
      return
      // input == "1" || 
      input == "2";
    }

    /// <summary>
    /// This method registers a user
    /// </summary>
    public static void RegisterUser(User user)
    {
      //TODO: check if user already exists
      Users.Add(user);
    }


  }
}