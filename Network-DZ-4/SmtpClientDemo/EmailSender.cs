using System.Net;
using System.Net.Mail;

namespace SmtpClientDemo;

public class EmailSender
{
    private string Host = "smtp.gmail.com";
    private int Port = 587;
    private bool EnableSSl = true;
    
    private string Email = "raulabd83@gmail.com";
    private string Password = "cntelncyyvinlcbz"; 
    
    public EmailSender(){}
    
    public void Send(EmailMessage message)
    {
        using var client = new SmtpClient(Host, Port);
        
        client.Credentials = new NetworkCredential(Email, Password);
        client.EnableSsl = EnableSSl;

        var mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body);
        mailMessage.From = new MailAddress("raulabd83@gmail.com", message.From);

        try
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Connecting to SMTP server...");

            client.Send(mailMessage);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Email sent successfully!");
        }
        catch (SmtpException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SMTP Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"General Error: {ex.Message}");
        }
        finally
        {
            Console.ResetColor();
        }
    }
}