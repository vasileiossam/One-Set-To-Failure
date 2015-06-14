using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Importer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var importer = new Importer();
            importer.Start(textBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var d1 = new DateTime(635696280000000000);
            var utc = DateTime.SpecifyKind(d1, DateTimeKind.Utc);
            var d2 = new DateTime(635696640000000000);

            var today = new DateTime(2015, 6, 12, 0, 0, 0, DateTimeKind.Unspecified);
            var todayticks = today.Ticks;


            var sqliteTicks = todayticks / TimeSpan.TicksPerSecond;
            textBox1.AppendText("635696280000000000\n");
            textBox1.AppendText("------------------\n");
            textBox1.AppendText(DateTime.Today.Ticks.ToString() + "\n");
            textBox1.AppendText(today.ToString() + "\n");


            
            var time = DateTime.Today.Date;

        }
    }
}
