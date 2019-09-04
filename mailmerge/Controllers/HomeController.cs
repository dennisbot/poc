using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mailmerge.Models;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;

namespace mailmerge.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult getruta()
        {
            string cad = Path.Combine("hola", "mundo", "probando");
            return Content(cad);
        }

        public IActionResult CreateDocument()
        {
            //Opens the Word template document
            FileStream fileStreamPath = new FileStream(@"wwwroot/test_template.docx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx))
            {
                //string[] fieldNames = { "ContactName", "CompanyName", "Address", "City", "Country", "Phone" };
                //string[] fieldValues = { "Nancy Davolio", "Syncfusion", "507 - 20th Ave. E.Apt. 2A", "Seattle, WA", "USA", "(206) 555-9857-x5467" };

                string[] fieldNames = { "firstname", "lastname", "address", "age"};
                string[] fieldValues = { "Dennis", "Huillca", "APV Agua Buena D-1 San Sebastian", "30" };
                string[] fieldValues2 = { "Roy", "Palacios", "Surco Lima", "31" };
                //Performs the mail merge
                document.MailMerge.Execute(fieldNames, fieldValues);
                document.MailMerge.Execute(fieldNames, fieldValues2);

                //Converts Word document to PDF
                DocIORenderer render = new DocIORenderer();
                PdfDocument pdfDocument = render.ConvertToPDF(document);
                render.Dispose();
                //adding the watermark
                PdfGraphics graphics = pdfDocument.Pages[0].Graphics;
                //set the font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 60);
                PdfGraphicsState state = graphics.Save();
                graphics.SetTransparency(0.15f);
                graphics.RotateTransform(-30);
                graphics.DrawString("PREVIEW", font, PdfPens.Black, PdfBrushes.Red, new PointF(-40, 450));
                //Saves de pdf to disk
                FileStream outfs = new FileStream(@"wwwroot/foobar.pdf", FileMode.Create, FileAccess.Write);
                pdfDocument.Save(outfs);
                outfs.Close();
                pdfDocument.Close();

                //Saves the Word document to MemoryStream
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                FileStream fs = new FileStream(@"wwwroot/foobar.docx", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                document.Save(fs, FormatType.Docx);
                fs.Close();
                //stream.WriteTo(fs);

                stream.Position = 0;
                //Download Word document in the browser
                return File(stream, "application/msword", "Result.docx");
            }
        }
    }
}
