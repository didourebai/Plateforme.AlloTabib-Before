using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;
//using Microsoft.Office.Interop.Outlook;
//using OutlookApp = Microsoft.Office.Interop.Outlook.Application;
//using Outlook = Microsoft.Office.Interop.Outlook;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.ContactDomainServices
{
    public class ContactDomainServices : IContactDomainServices
    {
        private readonly RessourceManager  _ressourceManager = RessourceManager.getInstance();

        public ResultOfType<MailTemplateModel> SendMail(string from, string to,string subject, string body)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body)  )
                
           return new Return<MailTemplateModel>()
                    .Error()
                    .AsValidationFailure(null,
                        "Les données sont vides.",
                    "Contact")
                    .WithDefaultResult();
            Logger.LogInfo(string.Format("Send Mail"));
            
            try
            {
                SendMailHelper.SendEmail(@from, to, subject, body);

                return new Return<MailTemplateModel>().OK().WithResult(new MailTemplateModel
                    {
                        Body = body,
                        From = from,
                        Subject = subject,
                        To = to
                    });
            }
            catch (System.Exception ex)
            {
                return new Return<MailTemplateModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Erreur d'envoi de votre message suite à l'exception : " + ex.Message + " ,Veuillez réessayer plus tard.").WithDefaultResult();
            }

        }

      

        ////Satrt Send Email Function
        //public string SendMail(string toList, string from, string subject, string body)
        //{

        //    MailMessage message = new MailMessage();
        //    SmtpClient smtpClient = new SmtpClient();
        //    string msg = string.Empty;
        //    try
        //    {
        //        MailMessage mm = new MailMessage(from, toList);
        //        mm.Subject = subject;
        //        mm.Body = body;
              
        //        mm.IsBodyHtml = true;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.EnableSsl = true;
        //        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
        //        NetworkCred.UserName = "sender@gmail.com";
        //        NetworkCred.Password = "xxxxx";
        //        smtp.UseDefaultCredentials = true;
        //        smtp.Credentials = NetworkCred;
        //        smtp.Port = 587;
        //        smtp.Send(mm);

        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //    }
        //    return msg;
        //}




        public ResultOfType<MessageModel> SendMotDePasseOublie(string destination)
        {
            try
            {
                //Get password to Send

                var emailConfig = SendMailHelper.InitConfiguration();
                emailConfig.Destination = destination;
                emailConfig.EmailSubject = "Test Mail ...";


                using (var mailer = new SmtpClient(emailConfig.SmtpServerAddress, Convert.ToInt32(emailConfig.SmtpPort)))
                {
                    if (emailConfig.WithCredentialsFlag)
                    {
                        mailer.Credentials = new NetworkCredential(emailConfig.SmtpUsername, emailConfig.SmtpPassword);
                        mailer.EnableSsl = true;
                    }

                    Logger.LogInfo(string.Format("Start Sending Email mot de passe oublie_Id: {0} ",destination));

                    var message = new MailMessage
                    {
                        From = new MailAddress(emailConfig.EmailSender)
                    };

                    message.To.Add(destination);
                    message.Subject = string.Format(emailConfig.EmailSubject);
                    message.IsBodyHtml = false;


                    mailer.Send(message);

                    return new Return<MessageModel>().OK().WithResult(new MessageModel
                    {
                      Message = "Mail envoyé avec succès!!"
                    });

                }
            }
            catch (System.Exception ex)
            {
                return new Return<MessageModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                   null, "Erreur d'envoi de votre message suite à l'exception : " + ex.Message + " ,Veuillez réessayer plus tard.").WithDefaultResult();
            }

           
        }

        public ResultOfType<MailTemplateModel> SendRendezVousVersOutlook(MailTemplateModel mail, string HDFrom, string HDTo)
        {
            try
            {
                #region Outlook

                ////First thing you need to do is add a reference to Microsoft Outlook 11.0 Object Library. Then, create new instance of Outlook.Application object: 

                //OutlookApp outlookApp = new OutlookApp(); // creates new outlook app

                ////Next, create an instance of AppointmentItem object and set the properties: 

                //var oAppointment = (AppointmentItem)outlookApp.CreateItem(OlItemType.olAppointmentItem);

                //oAppointment.Subject = mail.Subject;
                //oAppointment.Body = mail.Body;
                //oAppointment.Location = mail.AdressePraticien;

                //// Set the start date
                //oAppointment.Start = new DateTime(2015, 07, 20, 18, 0, 0);
                //// End date 
                //oAppointment.End = new DateTime(2015, 07, 20, 19, 0, 0);
                //// Set the reminder 15 minutes before start
                //oAppointment.ReminderSet = true;
                //oAppointment.ReminderMinutesBeforeStart = 15;


                ////Setting the sound file for a reminder: 
                ////set ReminderPlaySound = true;
                ////set ReminderSoundFile to a filename. 

                ////Setting the importance: 
                ////use OlImportance enum to set the importance to low, medium or high

                //oAppointment.Importance = OlImportance.olImportanceHigh;

                ///* OlBusyStatus is enum with following values:
                //olBusy
                //olFree
                //olOutOfOffice
                //olTentative
                //*/
                //oAppointment.BusyStatus = OlBusyStatus.olBusy;

                ////Finally, save the appointment: 

                //// Save the appointment
                //oAppointment.Save();

                //// When you call the Save () method, the appointment is saved in Outlook. Another useful method is ForwardAsVcal () which can be used to send the Vcs file via email. 

                //Outlook.MailItem mailItem = oAppointment.ForwardAsVcal();

                //mailItem.To = mail.To;
                //mailItem.Send();

                #endregion

                #region Dernier essai

                DateTime outputDateTimeValue;
                DateTime.TryParseExact(HDFrom, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputDateTimeValue);
                

                DateTime outputDateTimeValue2;
                DateTime.TryParseExact(HDTo, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputDateTimeValue2);

                string startTime1 = Convert.ToDateTime(outputDateTimeValue).ToString("yyyyMMddTHHmmssZ");
                string endTime1 = Convert.ToDateTime(outputDateTimeValue2).ToString("yyyyMMddTHHmmssZ");
                //SmtpClient sc = new SmtpClient("smtp.gmail.com")
                //{
                //    Port = 587,
                //    Credentials = new NetworkCredential("contact@allotabib.net", "reb@i321"),
                //    EnableSsl = true
                //};

                MailMessage msg = new MailMessage {From = new MailAddress(mail.From, "This is the email from")};

                msg.To.Add(new MailAddress(mail.To));
                msg.Subject = mail.Subject;
                msg.Body = mail.Body;

                StringBuilder str = new StringBuilder();
                str.AppendLine("BEGIN:VCALENDAR");

                //PRODID: identifier for the product that created the Calendar object
                str.AppendLine("PRODID:-//ABC Company//Outlook MIMEDIR//EN");
                str.AppendLine("VERSION:2.0");
                str.AppendLine("METHOD:REQUEST");

                str.AppendLine("BEGIN:VEVENT");

                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime1));//TimeZoneInfo.ConvertTimeToUtc("BeginTime").ToString("yyyyMMddTHHmmssZ")));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime1));//TimeZoneInfo.ConvertTimeToUtc("EndTime").ToString("yyyyMMddTHHmmssZ")));
                str.AppendLine(string.Format("LOCATION: {0}", mail.AdressePraticien));

                // UID should be unique.
                str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
                str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));

                str.AppendLine("STATUS:CONFIRMED");
                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:Accept");
                str.AppendLine("DESCRIPTION:Reminder");
                str.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");

                str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));
                str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

                str.AppendLine("END:VCALENDAR");
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
                ct.Parameters.Add("method", "REQUEST");
                ct.Parameters.Add("name", "meeting.ics");
                AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
                msg.AlternateViews.Add(avCal);
                //Response.Write(str);
                // sc.ServicePoint.MaxIdleTime = 2;
                //sc.Send(msg);

                #endregion
               

                return new Return<MailTemplateModel>().OK().WithResult(new MailTemplateModel
                {
                    Body = mail.Body,
                    From = mail.From,
                    Subject = mail.Subject,
                    To = mail.To
                });

            }
            catch (System.Exception ex)
            {
                return new Return<MailTemplateModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                 null, "Erreur d'envoi de votre message vers outloook suite à l'exception : " + ex.Message + " ,Veuillez réessayer plus tard.").WithDefaultResult();
            }
           
            //throw new NotImplementedException();
        }
    }
}
