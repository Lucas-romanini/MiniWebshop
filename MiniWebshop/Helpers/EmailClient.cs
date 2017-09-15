using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Hosting;
using System.IO;

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

    internal void SendNotification(string fullname, string email, string v)
    {
        throw new NotImplementedException();
    }
}