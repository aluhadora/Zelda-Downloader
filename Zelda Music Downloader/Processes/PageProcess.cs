using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using Zelda_Music_Downloader.Utilities;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Zelda_Music_Downloader.Processes
{
  public class PageProcess
  {
    private static string _folder;
    private static WebClient _client;

    public static Action<string> AnnounceSection;
    public static Action<string> AnnounceFile;

    public static void DownloadPage(string url)
    {
      _client = new WebClient();

      var doc = new HtmlDocument();
      doc.LoadHtml(PageHelper.GetSource(url));

      _folder = GetFolderName(doc);
      CreateFolder(_folder);

      var sections = GetSections(doc);

      foreach (var section in sections)
      {
        if (AnnounceSection != null) AnnounceSection("Track Listing");
        var folder = CreateDiscFolder(sections, section);
        
        foreach (var li in GetListItems(section))
        {
          if (li.Name != "li") continue;
          var address = li.FirstChild.Attributes["href"].Value;

          var fileName = Path.GetFileName(address).Replace("%20", " ").Replace("%7E", "~").Replace("&amp;", "&");
          if (AnnounceFile != null) AnnounceFile(fileName);

          try
          {
            _client.DownloadFile(address, Path.Combine(folder, fileName));
          }
          catch (Exception)
          {
            continue;
          }
          
        }

      }

      if (AnnounceFile != null) AnnounceFile(string.Empty);
      if (AnnounceSection != null) AnnounceSection(string.Empty);
    }

    private static string CreateDiscFolder(IList<HtmlNode> sections, HtmlNode section)
    {
      var folder = _folder;

      if (sections.Count > 1)
      {
        var name = section.ChildNodes[1].InnerText.Trim().Replace(":", " -");
        if (AnnounceSection != null) AnnounceSection(name);
        folder = Path.Combine(_folder, name);
      }

      CreateFolder(folder);

      return folder;
    }

    private static IEnumerable<HtmlNode> GetListItems(HtmlNode section)
    {
      foreach (var childNode in section.ChildNodes)
      {
        if (childNode.Name == "ol") return childNode.ChildNodes;
      }

      return new List<HtmlNode>();
    }

    private static IList<HtmlNode> GetSections(HtmlDocument doc)
    {
      var content = doc.GetElementbyId("content-text");

      var sections = new List<HtmlNode>();
      
      foreach (var childNode in content.ChildNodes)
      {
        if (childNode.GetAttributeValue("class", "") == "section")
        {
          sections.Add(childNode);
        }
      }

      return sections;
    }

    private static void CreateFolder(string folder)
    {
      if (Directory.Exists(folder)) return;
      Directory.CreateDirectory(folder);
    }

    private static string GetFolderName(HtmlDocument doc)
    {
      var mastHead = doc.GetElementbyId("masthead");
      var folder = WebUtility.HtmlDecode(mastHead.ChildNodes[1].InnerText.Trim().Replace(":", " -"));
      return Path.Combine("Music", folder);
    }
  }
}
