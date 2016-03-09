using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H7ADOConsoleDemo
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        // 1. Luodaan yhteys
        string connStr = GetConnectionString();

        using (SqlConnection conn = new SqlConnection(connStr))
        {
          // Avataan yhteys
          conn.Open();

          // 2. Tehdään SQL-kysely
          string sql = "SELECT asioid, lastname, firstname FROM Presences WHERE asioid = 'k6058'";
          SqlCommand cmd = new SqlCommand(sql, conn);

          // 3. Käsitellään tulos tässä DataReader-oliolla
          SqlDataReader rdr = cmd.ExecuteReader();

          // Käydään reader-olio läpi, forward only
          if (rdr.HasRows)
          {
            while (rdr.Read())
            {
              Console.WriteLine("Läsnäolosi: {0} {1} {2}", rdr.GetString(0), rdr.GetString(1), rdr.GetString(2));
            }

            Console.WriteLine("Tiedot haettu onnistuneesti");
          }

          // Suljetaan tarvittavat
          rdr.Close();
          conn.Close();
          Console.WriteLine("Tietokantayhteys suljettu onnistuneesti.");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        Console.ReadKey();
      }
    }

    private static string GetConnectionString()
    {
      return Properties.Settings.Default.Tietokanta;
    }
  }
}
