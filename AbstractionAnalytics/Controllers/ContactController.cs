using AbstractionAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AbstractionAnalytics.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        // POST: ApplyOnline
        [HttpPost]  
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Contact model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Analytics Enquiry From: {0}<br>"
                    + "City: {1}<br>"
                    + "Email: {2}<br>"
                    + "Telephone: {3}<br>"
                    + "Enquiry: {4}<br></p>"
                    + "This is an automated message.<br>";

                var body1 = "<p>Dear {0},<br>"
                    + "Thank you for the interest you have shown in Abstraction Analytics. </p>"
                    + "This is an automated message.<br>";

                // send information for Abstraction Analytics
                var receivedMessage = new MailMessage();
                receivedMessage.To.Add(new MailAddress("info@abanalytics.com"));  // lexus email // "info@abanalytics.com"
                receivedMessage.From = new MailAddress(model.email);  // users email
                receivedMessage.Sender = new MailAddress(model.email); // change sender
                receivedMessage.Subject = "Analytics Enquiry From website";
                receivedMessage.Body = string.Format(body, model.fullname, model.city, model.email, model.telephone, model.enquiry);
                receivedMessage.IsBodyHtml = true;

                // send email back to client from Abstraction Analytics
                var sendMessage = new MailMessage();
                sendMessage.To.Add(new MailAddress(model.email));  // user email // 
                sendMessage.From = new MailAddress("info@abanalytics.com");
                sendMessage.Sender = new MailAddress("info@abanalytics.com"); // change sender
                sendMessage.Subject = "Confirmation Email";
                sendMessage.Body = string.Format(body1, model.fullname);
                sendMessage.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.ServicePoint.MaxIdleTime = 1;
                    smtp.Timeout = 10000;
                    await smtp.SendMailAsync(receivedMessage);
                    await smtp.SendMailAsync(sendMessage);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }
        

        public ActionResult Sent(Contact model)
        {
            ViewBag.Message = "Your sent email page.";
            ViewBag.mail = model.email;
            return View(model);
        }
    }

   

}