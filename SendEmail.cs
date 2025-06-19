using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Security;

class EmailSender
{
          private string smtpServer;
          private int smtpPort;
          private string smtpUser;
          private string smtpPassword;

          public EmailSender(string smtpServer, int smtpPort, string smtpUser, string smtpPassword)
          {
               this.smtpServer = smtpServer;
               this.smtpPort = smtpPort;
               this.smtpUser = smtpUser;
               this.smtpPassword = smtpPassword;
          }
     


          public void SendEmail(string from, string to, string subject, string body)
     {
          try
          {
               // Criando o objeto de mensagem de e-mail
               MailMessage mailMessage = new MailMessage(from, to, subject, body);

               // Configurando o cliente SMTP
               SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort)
               {
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true // Usar SSL para segurança
               };

               // Enviando o e-mail
               smtpClient.Send(mailMessage);
               Console.WriteLine("E-mail enviado com sucesso!");
          }
          catch (Exception ex)
          {
               Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
          }
     }     


}



