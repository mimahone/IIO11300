using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H11BookShop
{
  public class Book
  {
    #region PROPERTIES

    private int id;

    public int ID
    {
      get { return id; }
    }

    private string name;

    public string Name
    {
      get { return name; }
      set { name = value; }
    }

    private string author;

    public string Author
    {
      get { return author; }
      set { author = value; }
    }

    private string country;

    public string Country
    {
      get { return country; }
      set { country = value; }
    }

    private int year;

    public int Year
    {
      get { return year; }
      set { year = value; }
    }

    #endregion

    #region CONSTRUCTORS

    public Book(int id)
    {
      this.id = id;
    }

    public Book(int id, string name, string author, string country, int year)
    {
      this.id = id;
      this.name = name;
      this.author = author;
      this.country = country;
      this.year = year;
    }

    #endregion

    #region METHODS

    public override string ToString()
    {
      return name + " written by " + author;
    }

    #endregion
  }

  public static class BookShop
  {
    private static string cs = Properties.Settings.Default.Kirjakauppa;

    public static List<Book> GetTestBooks()
    {
      List<Book> temp = new List<Book>();
      temp.Add(new Book(1, "Sota ja rauha", "Leo Tolstoi", "Venäjä", 1867));
      temp.Add(new Book(2, "Anna Karenina", "Leo Tolstoi", "Venäjä", 1877));
      return temp;
    }

    public static List<Book> GetBooks()
    {
      return GetTestBooks();
    }

    public static List<Book> GetBooks(bool useDB)
    {
      try
      {
        DataTable dt;
        List<Book> books = new List<Book>();

        if (useDB)
        {
          dt = DBBookShop.GetBooks(cs);
        }
        else
        {
          dt = DBBookShop.GetTestData();
        }

        // ORM =  Muutetaan datatablen rivit olioiksi
        Book book;

        foreach (DataRow row in dt.Rows)
        {
          book = new Book((int)row[0]);
          book.Name = row[1].ToString();
          book.Author = row[2].ToString();
          book.Country = row[3].ToString();
          book.Year = (int)row[4];
          books.Add(book);
        }

        // Palautus
        return books;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static int UpdateBook(Book book)
    {
      try
      {
        int c = DBBookShop.UpdateBook(cs, book.ID, book.Name, book.Author, book.Country, book.Year);
        return c;
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static bool InsertBook(Book book)
    {
      try
      {
        int c = DBBookShop.InsertBook(cs, book.Name, book.Author, book.Country, book.Year);
        return (c > 0);
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static bool DeleteBook(Book book)
    {
      try
      {
        int c = DBBookShop.DeleteBook(cs, book.ID);
        return (c > 0);
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}
