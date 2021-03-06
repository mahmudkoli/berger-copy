using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.EmailLog;
using BergerMsfaApi.Models.EmailVm;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Common
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IEmailLogService _emailLog;
        private readonly IWebHostEnvironment _env;
        public EmailSender(
            //IOptions<AuthMessageSenderOptions> optionsAccessor,
            IOptions<SmtpSettings> smtpSettings,
            IWebHostEnvironment env,
            IEmailLogService emailLog
            )
        {
            //Options = optionsAccessor.Value;

            _smtpSettings = smtpSettings.Value;
            _env = env;
            _emailLog = emailLog;
        }



        public async Task SendEmailWithAttachmentAsync(string email, string subject, string body, List<Attachment> lstattachment)
        {
            try
            {
                var message = new MailMessage();
                EmailLog emailLog = new EmailLog()
                {
                    From = _smtpSettings.SenderEmail,
                    To = email,
                    Body = body,
                    Subject = subject,
                    

                };

                message.To.Add(email);
                message.From = message.From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = body;
                if (lstattachment.Count > 0)
                {
                    foreach (var item in lstattachment)
                    {
                        message.Attachments.Add(item);
                    }
                }
                
                using (var smtpClient = new SmtpClient())
                {
                    try
                    {
                        smtpClient.Host = _smtpSettings.Server;
                        if (_smtpSettings.Port > 0)
                            smtpClient.Port = _smtpSettings.Port;
                        smtpClient.EnableSsl = _smtpSettings.EnableSSL;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = _smtpSettings.UseDefaultCredentials;
                        /// Now specify the credentials 
                        smtpClient.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);

                        await smtpClient.SendMailAsync(message);
                        emailLog.LogStatus = (int)EmailStatus.Success;
                    }
                    catch (Exception e)
                    {
                        emailLog.LogStatus = (int)EmailStatus.Fail;
                        emailLog.FailResoan = e.Message;
                    }
                    finally
                    {

                        await _emailLog.CreateAsync(emailLog);


                    }
                }
               

            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var message = new MailMessage();

                EmailLog emailLog = new EmailLog()
                {
                    From = _smtpSettings.SenderEmail,
                    To = email,
                    Body = body,
                    Subject = subject,

                };



                message.To.Add(email);
                message.From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName);
                message.Subject = subject;
                message.Body = body;
                using (var smtpClient = new SmtpClient())
                {
                    try
                    {
                        smtpClient.Host = _smtpSettings.Server;
                        if (_smtpSettings.Port > 0)
                            smtpClient.Port = _smtpSettings.Port;
                        smtpClient.EnableSsl = _smtpSettings.EnableSSL;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = _smtpSettings.UseDefaultCredentials;
                        /// Now specify the credentials 
                        smtpClient.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);

                        await smtpClient.SendMailAsync(message);
                        emailLog.LogStatus = (int)EmailStatus.Success;

                    }
                    catch (Exception e)
                    {
                        emailLog.LogStatus = (int)EmailStatus.Fail;
                        emailLog.FailResoan = e.Message;
                    }
                    finally
                    {

                        await _emailLog.CreateAsync(emailLog);


                    }
                }

            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }



    }

    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, List<Attachment> attachments);
    }
}
