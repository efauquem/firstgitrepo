using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Printing;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            var paperSize =  new PaperSize();
            paperSize.RawKind = (int)PaperKind.A4;
            System.Diagnostics.Debug.WriteLine(paperSize.Height + " pouces de hauteur");
            System.Diagnostics.Debug.WriteLine(paperSize.Width + " pouces de hauteur");

            PageSettings psettings = new PageSettings();
            psettings.Landscape = true; // force landscape
            psettings.PaperSize = paperSize; // force A4

            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings = psettings;
            pd.DocumentName = "Test pour Vuitton";
            pd.PrintPage += PrintPage;
            pd.EndPrint += PrintPageEnd;
            pd.Print();

        }

        private void PrintPageEnd(object sender, PrintEventArgs e)
        {
            MessageBox.Show("print is done");
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            string res = BuildImage(textBox1.Text);
            System.Drawing.Image img = System.Drawing.Image.FromFile(res);
            Point loc = new Point(10, 10);
            e.Graphics.DrawImage(img, loc);


        }




        private string BuildImage(string text2Add)
        {
            Font arialFont = new Font("Arial", 10);

            Random rnd = new Random();
            int randomInt = (int)Math.Round(rnd.NextDouble() * 1000);
            string targetPath = "D:\\temp\\sheep" + randomInt.ToString() + ".jpg";


            Bitmap a = new Bitmap("D:\\temp\\sheep.jpg");

            using (Graphics g = Graphics.FromImage(a))
            {
                g.DrawString(text2Add, arialFont, Brushes.Black, new PointF(50, 50));

                a.Save(targetPath);
            }

            return targetPath;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectQuery query = new SelectQuery("Win32_PrintJob");

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection printJobs = searcher.Get())
                foreach (ManagementObject printJob in printJobs)
                {
                    // The format of the Win32_PrintJob.Name property is "PrinterName,JobNumber"
                    string name = (string)printJob["Name"];
                    string[] nameParts = name.Split(',');
                    string printerName = nameParts[0];
                    string jobNumber = nameParts[1];
                    string document = (string)printJob["Document"];
                    string jobStatus = (string)printJob["JobStatus"];

                    // Process job properties...
                    MessageBox.Show(name + " : " + jobStatus);
                }
        }

        /// <summary>
        /// Enumerates network printers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var installedPrinters = new string[PrinterSettings.InstalledPrinters.Count];
            PrinterSettings.InstalledPrinters.CopyTo(installedPrinters, 0);

            var printers = new List<string>();
            var printServers = new List<string>();
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            foreach (var printer in searcher.Get())
            {
                var serverName = @"\\" + printer["SystemName"].ToString().TrimStart('\\');
                if (!printServers.Contains(serverName))
                    printServers.Add(serverName);
            }

            foreach (var printServer in printServers)
            {
                var server = new PrintServer(printServer);
                try
                {
                    var queues = server.GetPrintQueues();
                    //printers.AddRange(queues.Select(q => q.Name));
                    if (queues != null)
                    {
                        foreach (PrintQueue pq in queues)
                        {
                            if (pq != null)
                            {
                                Console.WriteLine("Q : " + pq.Name);
                                if (!(pq.IsOffline))
                                {
                                    var jobInfo = pq.GetPrintJobInfoCollection();
                                    if (jobInfo != null)
                                    {
                                        foreach (var ji in jobInfo)
                                        {
                                            System.Diagnostics.Debug.WriteLine(ji.Name + " on " + printServer + " : " + ji.JobStatus.ToString()) ;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception correctly
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            // \\NGRPSIN-FS02.smart-up.net\HP4730-2
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String printerName = @"\\NGRPSIN-FS02.smart-up.net\HP4730-2";
            var server = new PrintServer(printerName);
            try
            {
                var queues = server.GetPrintQueues();
                foreach (var q in queues)
                {
                    System.Diagnostics.Debug.WriteLine(q.Name);
                }
            }
            catch (Exception)
            {
                // Handle exception correctly
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var printer = new ClassLibrary1.Printer();
            printer.Print("From WinForms");
        }

        
    }
}
