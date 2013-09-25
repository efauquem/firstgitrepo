using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class Printer
    {
        private string _message;

        private void PrintPageEnd(object sender, PrintEventArgs e)
        {
            //
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            string res = BuildImage(_message);
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

        public void Print(string message)
        {
            _message = message;
            var paperSize = new PaperSize();
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

    }
}
