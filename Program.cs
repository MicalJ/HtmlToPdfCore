using DinkToPdf;
using SelectPdf;
using System;
using System.IO;

namespace HtmlToPdfCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            HtmlToPdf();
            //SelectHtmlToPdf();
            PressEnterToExit();
        }
        private static void PressEnterToExit()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Press 'Enter' to exit.");
            System.Console.ReadLine();
        }

        private static void HtmlToPdf()
        {
            var source = Path.Combine("Templates", "template.html");
            Console.WriteLine(source);
            var html = ReadFile(source);

            var destination = Path.Combine("Files", DateTime.UtcNow.Ticks.ToString() + "test.pdf");
            Console.WriteLine(destination);

            var converter = new SynchronizedConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4Plus,
                },
                            Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    }
                }
            };
            byte[] pdf = converter.Convert(doc);

            if (!Directory.Exists("Files"))
            {
                Directory.CreateDirectory("Files");
            }

            using (FileStream stream = new FileStream(destination, FileMode.Create))
            {
                stream.Write(pdf, 0, pdf.Length);
            }

        }

        private static void SelectHtmlToPdf()
        {
            var source = Path.Combine("Templates", "template.html");
            Console.WriteLine(source);

            var html = ReadFile(source);

            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(html);

            if (!Directory.Exists("Files"))
            {
                Directory.CreateDirectory("Files");
            }
            var destination = Path.Combine("Files", DateTime.UtcNow.Ticks.ToString() + "test.pdf");
            Console.WriteLine(destination);
            doc.Save(destination);
            doc.Close();
        }

        public static string ReadFile(string filePath)
        {
            var data = File.ReadAllText(filePath);
            return data;
        }
    }
}
