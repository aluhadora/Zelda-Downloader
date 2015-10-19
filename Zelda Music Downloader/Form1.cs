using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zelda_Music_Downloader.Processes;

namespace Zelda_Music_Downloader
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      PageProcess.AnnounceSection += x => textBox2.Invoke(new Action<string>(y => textBox2.Text = y), x);
      PageProcess.AnnounceFile += x => textBox3.Invoke(new Action<string>(y => textBox3.Text = y), x);
      new Thread(() => PageProcess.DownloadPage(textBox1.Text)).Start();
    }


  }
}
