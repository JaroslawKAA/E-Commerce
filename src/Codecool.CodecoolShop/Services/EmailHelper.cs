using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Codecool.CodecoolShop.Services
{
    public static class EmailHelper
    {
        public static async void SendEmailConfirmation(
            string clientEmail,
            string emailBody)
        {
            MailMessage message = new MailMessage("our.super.shop.in.cc@gmail.com", clientEmail)
            {
                Subject = "Order confirmation", Body = emailBody, IsBodyHtml = true,
            };
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = "smtp.gmail.com",
                EnableSsl = true,
            };

            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("our.super.shop.in.cc@gmail.com", "ytrewq09");

            try
            {
                client.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public static async Task<string> RenderPartialViewToString(
            string viewName,
            object model,
            ViewDataDictionary viewData,
            Controller controller,
            ICompositeViewEngine viewEngine)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData = viewData;
            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    viewEngine.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}