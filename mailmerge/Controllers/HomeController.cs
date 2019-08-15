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
        public IActionResult CreateDocument()
        {
            //Opens the Word template document
            FileStream fileStreamPath = new FileStream(@"Data/Letter Formatting.docx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            using (WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx))
            {
                string[] fieldNames = { "ContactName", "CompanyName", "Address", "City", "Country", "Phone" };
                string[] fieldValues = { "Nancy Davolio", "Syncfusion", "507 - 20th Ave. E.Apt. 2A", "Seattle, WA", "USA", "(206) 555-9857-x5467" };
                //Performs the mail merge
                document.MailMerge.Execute(fieldNames, fieldValues);
                //Saves the Word document to MemoryStream
                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                document.Close();
                stream.Position = 0;
                //Download Word document in the browser
                return File(stream, "application/msword", "Result.docx");
            }
        }
    }
}
