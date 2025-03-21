using System.Net.Mail;
using firstProject.Models;

namespace CoreProject.Middlewares;

public class MyErrorMiddleware
{
    private RequestDelegate next;
    public MyErrorMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        c.Items["success"] = false;
        try
        {
            await next(c);
            c.Items["success"] = true;
        }
        catch (ApplicationException ex)
        {
           
            //  c.Response.StatusCode = 400;
            await c.Response.WriteAsync(ex.Message);
            //---------------------------------------------------------------------
            // SmtpClient smtpClient = new SmtpClient("mail.MyWebsiteDomainName.com", 25);

            // smtpClient.Credentials = new System.Net.NetworkCredential("info@MyWebsiteDomainName.com", "myIDPassword");
            // // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
            // smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            // smtpClient.EnableSsl = true;
            // MailMessage mail = new MailMessage();

            // //Setting From , To and CC
            // mail.From = new MailAddress("info@MyWebsiteDomainName", "MyWeb Site");
            // mail.To.Add(new MailAddress("info@MyWebsiteDomainName"));
            // mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

            // smtpClient.Send(mail);
            //https://stackoverflow.com/questions/18326738/how-to-send-email-in-asp-net-c-sharp
            //link to code example
            //------------------------------------------------------------------------
        }
        catch (Exception e)
        {
            // c.Response.StatusCode = 500;
            await c.Response.WriteAsync("go to technical support");
        }
    }
}

public static partial class MiddlewareExtantion
{
    public static IApplicationBuilder UseMyErrorMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<MyErrorMiddleware>();
        // return app;
    }
}