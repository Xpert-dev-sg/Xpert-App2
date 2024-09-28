using GDotnet.Reader.Api.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using XpertApp2.DB;

namespace XpertApp2.Utility
{
    public class EmailUtility
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void SendEmail(string msg,string subj, string[] toEmails)
        {

            try
            {

                SmtpClient mySmtpClient = new SmtpClient(DB_Base.mailserver, Convert.ToInt32(DB_Base.mailport));

                // set smtp-client with basicAuthentication
                //mySmtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new  System.Net.NetworkCredential(DB_Base.mailusername, "DeX&21092024");//DB_Base.mailpassword
                //mySmtpClient.Credentials = basicAuthenticationInfo;
                //mySmtpClient.EnableSsl = true;  // Enable STARTTLS
                //mySmtpClient.UseDefaultCredentials = false;
                //mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage myMail = new MailMessage();

                myMail.From = new MailAddress(DB_Base.mailusername);

                foreach (var toEmail in toEmails)
                {
                    myMail.To.Add(toEmail);
                }


                myMail.Subject = subj;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;
                
                myMail.Body = $"<b>{msg}</b>.";
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                
                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error send email:"+ex.Message);
                log.Error("error send email:" + ex.Message);
            }
        }

        public static void SendEmail1(string msg, string subj, string[] toEmails)
        {

            try
            {

                SmtpClient mySmtpClient = new SmtpClient("mail.defunden.com", 465);

                // set smtp-client with basicAuthentication
                //mySmtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("info@defunden.com", "DeX&21092024");//DB_Base.mailpassword
                //mySmtpClient.Credentials = basicAuthenticationInfo;
                mySmtpClient.EnableSsl = true;  // Enable STARTTLS
                //mySmtpClient.UseDefaultCredentials = false;
                //mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage myMail = new MailMessage();

                myMail.From = new MailAddress(DB_Base.mailusername);

                foreach (var toEmail in toEmails)
                {
                    myMail.To.Add(toEmail);
                }


                myMail.Subject = subj;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                myMail.Body = $"<b>{msg}</b>.";
                myMail.BodyEncoding = System.Text.Encoding.UTF8;

                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error send email1:" + ex.Message);
                log.Error("error send email1:" + ex.Message);
            }
        }
        public static void SendEmail2(string msg, string subj, string[] toEmails)
        {

            try
            {

                SmtpClient mySmtpClient = new SmtpClient("mail.defunden.com", 26);

                // set smtp-client with basicAuthentication
                //mySmtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential("info@defunden.com", "DeX&21092024");//DB_Base.mailpassword
                //mySmtpClient.Credentials = basicAuthenticationInfo;
                mySmtpClient.EnableSsl = true;  // Enable STARTTLS
                //mySmtpClient.UseDefaultCredentials = false;
                //mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage myMail = new MailMessage();

                myMail.From = new MailAddress(DB_Base.mailusername);

                foreach (var toEmail in toEmails)
                {
                    myMail.To.Add(toEmail);
                }


                myMail.Subject = subj;
                myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                myMail.Body = $"<b>{msg}</b>.";
                myMail.BodyEncoding = System.Text.Encoding.UTF8;

                myMail.IsBodyHtml = true;

                mySmtpClient.Send(myMail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error send email2:" + ex.Message);
                log.Error("error send email2:" + ex.Message);
            }
        }
    }
}
