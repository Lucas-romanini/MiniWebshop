using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

public class EmailClient
{
    private string _smpt;
    private int _port;
    private string _email;
    private string _password;
    private bool _ssl;

    private SmtpClient mailClient;

    public EmailClient( string smtp, int port, string email,string password, bool ssl)
    {
        _smpt = smtp;
        _port = port;
        _email = email;
        _password = password;
        _ssl = ssl;

        mailClient = new SmtpClient(_smpt, _port);
        mailClient.EnableSsl = _ssl;
        mailClient.Credentials = new System.Net.NetworkCredential(email, password);
    }

    public void SendEmail(string to)
    {
        MailMessage mail = new MailMessage(_email, to);
        mail.IsBodyHtml = true;

        mail.Subject = "Hej med Dig";
        mail.From = new MailAddress(_email);
        mail.To.Add(new MailAddress(to));

        mail.Body = "This is an email";

        mailClient.Send(mail);
    }
}
