using System;
using System.Windows.Forms;

namespace Zelda_Music_Downloader
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }

    //private const string ApiLnk = "http://zeldauniverse.net/media/music/majoras-mask-orchestrations/";

    //static void Main(string[] args)
    //{
    //  PageCache.CreateDatabase();

    //  MessageBox.Show(PageHelper.GetSource(ApiLnk));
    //}
  }
}
