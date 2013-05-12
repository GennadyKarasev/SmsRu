using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace SmsRu.Handlers
{
    public class CallbackHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007        /// 
        /// 
        /// Для использования CallbackHandler нужно добавить эти строчки в web.config вашего проекта.
        ///     <handlers>
        ///     <add name="SmsRuCallbackHandler" preCondition="integratedMode" verb="*" type="SmsRu.Handlers.CallbackHandler" path="/SmsRuCallback" />
        ///     </handlers> 
        ///     
        /// http://sms.ru/?panel=apps&subpanel=cb - документация по API
        /// 
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Form.Keys.Count > 0)
            {
                String log_handler = ConfigurationManager.AppSettings["logFolder"] + "Handler.txt";
                String log_error = ConfigurationManager.AppSettings["logFolder"] + "Error.txt";
                try
                {
                    
                    String index = String.Empty;
                    for (Int32 i = 0; i < context.Request.Form.Keys.Count; i++)
                    {
                        index = "data[" + i.ToString() + "]";
                        String[] lines = context.Request.Form[index].Split(new String[] { "\n" }, StringSplitOptions.None);
                        if (lines[0] == "sms_status")
                        {
                            String smsID = lines[1];
                            String status = lines[2];

                            using (StreamWriter writer = new StreamWriter(log_handler, true))
                            {
                                writer.WriteLine(String.Format("{0}={1}{2}Callback:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                                writer.WriteLine(String.Format("Запрос: {0}{1}", Environment.NewLine, context.Request.Form[index]));
                                writer.WriteLine("Документация - http://sms.ru/?panel=apps&subpanel=cb" + Environment.NewLine);
                            }

                            // Ваш код.
                            // Можно использовать EnumResponseCodes для работы со статусами.
                            context.Response.Write("100");
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер sms.ru недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Flush();
            context.Response.End();
        }

        #endregion
    }
}
