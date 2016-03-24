using System;
using System.Data;
using System.Data.SqlClient;

namespace H11BookShop
{
  public class DBBookShop
  {
    public static DataTable GetTestData()
    {
      // Luodaan testausta varten oikeanlainen DataTable
      DataTable dt = new DataTable();

      // Sarakkeiden määritys
      dt.Columns.Add("ID", typeof(int));
      dt.Columns.Add("Name", typeof(string));
      dt.Columns.Add("Author", typeof(string));
      dt.Columns.Add("Country", typeof(string));
      dt.Columns.Add("Year", typeof(int));

      // Rivit eli tietueet
      dt.Rows.Add(11, "Pekka Lipposen seikkailut", "Outsider", "Suomi", 1950);
      dt.Rows.Add(12, "Lucky Luke", "René Coscinny", "Belgia", 1946);

      return dt;
    }

    public static DataTable GetBooks(string connStr)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
          const string sql = @"SELECT ID, Name, Author, Country, Year FROM Books";
          conn.Open(); // kai tarpeeton?
          SqlCommand cmd = new SqlCommand(sql, conn);
          SqlDataAdapter da = new SqlDataAdapter(cmd);
          DataTable dt = new DataTable("Books");
          da.Fill(dt);
          conn.Close(); // kai tarpeeton?
          return dt;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static int UpdateBook(string connStr, int id, string name, string author, string country, int year)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
          // HUOM: käytetään SQL-kyselyn parametreja kahdesta syystä:
          // 1: Heittomerkki (hipsa) engl. apostrophe esim. nimessä O'Hara
          // 2: Tietoturva: parametrisoidut kyselyt ovat tietoturvallisempia
          const string sql = @"UPDATE Books SET Name=@Name, Author=@Author, Country=@Country, Year=@Year WHERE ID = @Id";
          
          SqlCommand cmd = new SqlCommand(sql, conn);
          //SqlParameter sp;
          //sp = new SqlParameter("@Name", SqlDbType.NVarChar);
          //sp.Value = name;
          //cmd.Parameters.Add(sp);

          cmd.Parameters.AddWithValue("@ID", id);
          cmd.Parameters.AddWithValue("@Name", name);
          cmd.Parameters.AddWithValue("@Author", author);
          cmd.Parameters.AddWithValue("@Country", country);
          cmd.Parameters.AddWithValue("@Year", year);

          conn.Open();
          int c = cmd.ExecuteNonQuery();
          conn.Close();

          return c;
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static int InsertBook(string connStr, string name, string author, string country, int year)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
          const string sql = @"INSERT INTO Books (Name, Author, Country, Year) VALUES (@Name, @Author, @Country, @Year)";

          SqlCommand cmd = new SqlCommand(sql, conn);
          cmd.Parameters.AddWithValue("@Name", name);
          cmd.Parameters.AddWithValue("@Author", author);
          cmd.Parameters.AddWithValue("@Country", country);
          cmd.Parameters.AddWithValue("@Year", year);

          conn.Open();
          int c = cmd.ExecuteNonQuery();
          conn.Close();

          return c;
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static int DeleteBook(string connStr, int id)
    {
      try
      {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
          const string sql = @"DELETE FROM Books WHERE ID = @ID";

          SqlCommand cmd = new SqlCommand(sql, conn);
          cmd.Parameters.AddWithValue("@ID", id);

          conn.Open();
          int c = cmd.ExecuteNonQuery();
          conn.Close();

          return c;
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

  }
}
