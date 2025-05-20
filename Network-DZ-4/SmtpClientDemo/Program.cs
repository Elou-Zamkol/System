
using SmtpClientDemo;

Console.WriteLine("========================= SmtpClientDemo ===========================\n ");

Console.Write("Enter email address: ");
string SendersEmail = Console.ReadLine();

Console.Write("Enter to email address: ");
string EmailRecipient = Console.ReadLine();

Console.Write("Enter Message: ");
string Message = Console.ReadLine();


var message = new EmailMessage(SendersEmail, EmailRecipient, "Test Email", Message);
var sender = new EmailSender();
sender.Send(message);
