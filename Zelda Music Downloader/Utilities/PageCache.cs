using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Zelda_Music_Downloader.Utilities
{
  public static class PageCache
  {
    private const string DatabaseFileName = "PageCache.sqlite";
    private const string TableName = "PageCache";
    private const string UrlColumnName = "Url";
    private const string SourceColumnName = "Source";

    public static void CreateDatabase()
    {
      var con = NewConnection();

      try
      {
        var cmd = con.CreateCommand();
        cmd.CommandText = string.Format("SELECT * FROM {0} WHERE 1 = 0", TableName);
        con.Open();
        cmd.ExecuteNonQuery();
        con.Close();
      }
      catch (Exception)
      {
        con.Close();
        SQLiteConnection.CreateFile(DatabaseFileName);

        CreateCacheTable();
      }
    }

    private static void CreateCacheTable()
    {
      var sql = string.Format("create table {0} ({1} varchar(60), {2} TEXT)", TableName, UrlColumnName, SourceColumnName);

      var con = NewConnection();
      con.Open();
      var command = new SQLiteCommand(sql, con);
      command.ExecuteNonQuery();
      con.Close();
    }

    private static SQLiteConnection NewConnection()
    {
      var con = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DatabaseFileName));
      return con;
    }

    public static void SaveSource(string url, string source)
    {
      var con = NewConnection();
      var cmd = con.CreateCommand();

      cmd.CommandText = String.Format("INSERT INTO {0} ({1}, {2}) VALUES (@0, @1);", TableName, UrlColumnName,
        SourceColumnName);
      cmd.Parameters.Add(new SQLiteParameter("@0", DbType.String) {Value = url});
      cmd.Parameters.Add(new SQLiteParameter("@1", DbType.String) {Value = source});
      con.Open();

      try
      {
        cmd.ExecuteNonQuery();
      }
      catch (Exception e)
      {
        MessageBox.Show(e.Message);
      }

      con.Close();
    }

    public static string LoadSource(string url)
    {
      var con = NewConnection();
      var query = string.Format("SELECT {0} FROM {1} WHERE {2}='{3}';", SourceColumnName, TableName, UrlColumnName, url);

      var cmd = con.CreateCommand();
      cmd.CommandText = query;

      con.Open();
      try
      {
        try
        {
          var source = cmd.ExecuteScalar().ToString();
          con.Close();
          return source;
        }
        catch (Exception) {}
      }
      catch (Exception) {}
      con.Close();

      return null;
    }
  }
}
    