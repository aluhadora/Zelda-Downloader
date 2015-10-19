using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zelda_Music_Downloader.Utilities
{
  public class PageHelper
  {
    public static string GetSource(string url)
    {
      var cachedSource = PageCache.LoadSource(url);
      if (cachedSource != null) return cachedSource;

      var source = DownloadSource(url);
      PageCache.SaveSource(url, source);

      return source;
    }

    private static string DownloadSource(string url)
    {
      string source = null;

      using (var client = new WebClient())
      {
        client.Headers[HttpRequestHeader.Accept] = "text/html, image/png, image/jpeg, image/gif, */*;q=0.1";
        client.Headers[HttpRequestHeader.UserAgent] =
          "Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";

        while (source == null)
        {
          try
          {
            source = client.DownloadString(url);
          }
          catch {}
        }

        return source;
      }
    }
  }
}
