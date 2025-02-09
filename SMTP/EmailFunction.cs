using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SMTP
{
    public static class EmailFunction
    {
        [FunctionName("SendEmail")]
        [return: SendGrid(ApiKey = "SendGridApiKey")]
        public static SendGridMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing email request...");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<EmailRequest>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.ToEmail) || string.IsNullOrEmpty(data.Subject))
            {
                log.LogInformation("SendEmail failed: data null");
                return new SendGridMessage();
            }

            var from = new EmailAddress("inquiries@deveddie.com", "DevEddie");
            var to = new EmailAddress(data.ToEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, data.Subject, data.Body, null);

            return msg;
        }

        [FunctionName("SendEmailUsingClient")]
        //[return: SendGrid(ApiKey = "SendGridApiKey")]
        public static async Task<SendGridMessage> SendEmailUsingClient(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing email request...");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<EmailRequest>(requestBody);

            if (data == null || string.IsNullOrEmpty(data.ToEmail) || string.IsNullOrEmpty(data.Subject))
            {
                //return new BadRequestObjectResult("Invalid request payload. 'ToEmail', and 'Subject'are required.");
                //return new ArgumentNullException("data");
            }

            var from = new EmailAddress("inquiries@deveddie.com", "DevEddie");
            var to = new EmailAddress(data.ToEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, data.Subject, data.Body, null);

            try
            {
                var client = new SendGridClient(Environment.GetEnvironmentVariable("SendGridApiKey"));
                var response = await client.SendEmailAsync(msg);
                log.LogInformation($"SendGrid response status: {response.StatusCode}");
                log.LogInformation($"SendGrid response body: {await response.Body.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                log.LogError($"Error sending email: {ex.Message}");
            }

            return msg;
        }
    }

    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
