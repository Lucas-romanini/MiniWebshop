using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Hosting;
using System.IO;
using MWRepo.Models;

public class EmailClient
{
    private string _smpt;
    private int _port;
    private string _systemEmail;
    private string _password;
    private bool _ssl;

    private SmtpClient mailClient;

    public EmailClient(string smtp, int port, string email, string password, bool ssl)
    {
        _smpt = smtp;
        _port = port;
        _systemEmail = email;
        _password = password;
        _ssl = ssl;

        mailClient = new SmtpClient(_smpt, _port);
        mailClient.EnableSsl = _ssl;
        mailClient.Credentials = new System.Net.NetworkCredential(email, password);
    }

    public void SendEmail(string to)
    {
        MailMessage mail = new MailMessage(_systemEmail, to);
        mail.IsBodyHtml = true;

        mail.Subject = "Hej med Dig";
        mail.From = new MailAddress(_systemEmail);
        mail.To.Add(new MailAddress(to));

        mail.Body = "dette er en email";

        mailClient.Send(mail);
    }

    public string ReadHtml(string path)
    {
        string htmlBody = "";

        using (StreamReader reader = new StreamReader(path))
        {
            htmlBody += reader.ReadToEnd();
        }

        return htmlBody;
    }

    public void SendNotification(string customerName, string email, string subject, string message)
    {
        #region to Client
        string htmlBody = (HostingEnvironment.ApplicationPhysicalPath + @"/EmailTemplates/Notification.html");

        htmlBody = htmlBody.Replace("{customerName}", customerName);
        htmlBody = htmlBody.Replace("{date}", DateTime.Today.ToShortDateString());

        MailMessage mail = new MailMessage();
        mail.IsBodyHtml = true;

        mail.Subject = "Tak for din besked - Webshop test";
        mail.From = new MailAddress(_systemEmail);
        mail.To.Add(new MailAddress(email));

        mail.Body = htmlBody;

        mailClient.Send(mail);
        #endregion

        #region to Admin

        htmlBody = "";
        htmlBody = ReadHtml(HostingEnvironment.ApplicationPhysicalPath + @"/EmailTemplates/NotificationToAdmin.html");

        htmlBody = htmlBody.Replace("{customerName}", customerName);
        htmlBody = htmlBody.Replace("{subject}", subject);
        htmlBody = htmlBody.Replace("{email}", email);
        htmlBody = htmlBody.Replace("{message}", message);
        htmlBody = htmlBody.Replace("{date}", DateTime.Today.ToShortDateString());

        MailMessage mailToAdmin = new MailMessage();
        mailToAdmin.IsBodyHtml = true;
        mailToAdmin.Subject = "Der er kommet en mail fra: " + customerName + "via kontaktformen";
        mailToAdmin.Body = htmlBody;
        mailToAdmin.From = new MailAddress(_systemEmail);
        mailToAdmin.To.Add(new MailAddress("Lucas@romanini.dk"));

        mailClient.Send(mailToAdmin);
        #endregion

    }


    public string TableRow(string data)
    {
        return "<tr>" + data + "</tr>";
    }

    public string TableData(string info)
    {
        return "<td>" + info + "</td>";
    }


    public void SendInvoice(Order order, List<ProductVM> products)
    {
        string htmlBody = ReadHtml(HostingEnvironment.ApplicationPhysicalPath + @"/EmailTemplates/InvoiceTop.html");

        htmlBody = htmlBody.Replace("{orderNumber}", "9999");
        htmlBody = htmlBody.Replace("{fullName}", order.Fullname);
        htmlBody = htmlBody.Replace("{phone}", order.Phone);
        htmlBody = htmlBody.Replace("{email}", order.Email);
        htmlBody = htmlBody.Replace("{address}", order.Address);
        htmlBody = htmlBody.Replace("{postal}", order.Postal);
        htmlBody = htmlBody.Replace("{city}", order.City);

        if (order.Fullname_Delivery != null)
        {
            htmlBody = htmlBody.Replace("{orderNumber-delivery}", "9999");
            htmlBody = htmlBody.Replace("{fullName-delivery}", order.Fullname_Delivery);
            htmlBody = htmlBody.Replace("{phone-delivery}", order.Phone_Delivery);
            htmlBody = htmlBody.Replace("{email-delivery}", order.Email_Delivery);
            htmlBody = htmlBody.Replace("{address-delivery}", order.Address_Delivery);
            htmlBody = htmlBody.Replace("{postal-delivery}", order.Postal_Delivery);
            htmlBody = htmlBody.Replace("{city-delivery}", order.City_Delivery);
        }

        string productBody = "";

        double total = 0;

        foreach (ProductVM vm in products)
        {
            productBody += TableRow(
                TableData(vm.Product.Name) +
                TableData(vm.Product.Description) +
                TableData(vm.Product.Price.ToString())
                );

            total += vm.Product.GetPrice();
        }

        productBody += TableRow(
            "<td colspan='2'>Total</td>" +
            "<td>" + total + "</td>"
            );

        htmlBody += productBody; 

        htmlBody += ReadHtml(HostingEnvironment.ApplicationPhysicalPath + @"/EmailTemplates/InvoiceBottom.html");

        htmlBody = htmlBody.Replace("{year}", DateTime.Now.Year.ToString());

        MailMessage mail = new MailMessage();
        mail.IsBodyHtml = true;
        mail.Body = htmlBody;
        mail.From = new MailAddress(_systemEmail);
        mail.To.Add(new MailAddress(order.Email));
        mail.Subject = "Invoice from Webshop test Order: 9999";

        mailClient.Send(mail);

    }





    //internal void SendNotification(string fullname, string email, string v)
    //{
    //    throw new NotImplementedException();
    //}
}