using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XpertApp2.DB;

namespace XpertApp2.Utility
{
    public class EmailUtility
    {
        public static void SendEmail(string msg, string[] toEmails)
        {

            try
            {

                SmtpClient mySmtpClient = new SmtpClient(DB_Base.mailserver, Convert.ToInt32(DB_Base.mailport));

                // set smtp-client with basicAuthentication
                //mySmtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new  System.Net.NetworkCredential(DB_Base.mailusername, DB_Base.mailpassword);
                //mySmtpClient.Credentials = basicAuthenticationInfo;

                // add from,to mailaddresses
                //MailAddress from = new MailAddress("test@example.com");
                //MailAddress to = new MailAddress("test2@example.com");
                MailMessage myMail = new MailMessage();

                myMail.From = new MailAddress(DB_Base.mailusername);

                foreach (var toEmail in toEmails)
                {
                    myMail.To.Add(toEmail);
                }

                //// add ReplyTo
                //MailAddress replyTo = new MailAddress("reply@example.com");
                //myMail.ReplyToList.Add(replyTo);

                // set subject and encoding
                myMail.Subject = "not allowed to borrow  item";
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                myMail.Body = $"<b>{msg}</b>.";
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error send email:"+ex.Message);
            }
        }
    }
}
