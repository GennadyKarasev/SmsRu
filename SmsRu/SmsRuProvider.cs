using SmsRu.Enumerations;
using SmsRu.Helpers;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SmsRu
{
    /// <summary>
    /// Класс для работы с SMS.RU API.
    /// </summary>
    public class SmsRuProvider : ISmsProvider
    {
        /*
         * Проект открытый, можно использовать как угодно. Сохраняйте только авторство.
         * Официальная документация по API - http://sms.ru/?panel=api&subpanel=method&show=sms/send.
         * Разработчик - gennadykarasev@gmail.com. В случае, если что-то не работает, то писать на эту почту.
         * 
         * Для работы с методами класса, нужно указать в app.config значения для переменных ниже.
         *
         */

        #region Переменные

        String login = ConfigurationManager.AppSettings["smsRuLogin"];                              // Логин для доступа к SMS.RU
        String password = ConfigurationManager.AppSettings["smsRuPassword"];                        // Пароль для доступа к SMS.RU 
        String apiId = ConfigurationManager.AppSettings["smsRuApiID"];                              // Является вашим секретным кодом, который используется во внешних программах 
        String partnerId = ConfigurationManager.AppSettings["partnerId"];                           // Если вы участвуете в партнерской программе, укажите этот параметр в запросе
        String SmsRuEmail = ConfigurationManager.AppSettings["smsRuApiID"] + "@sms.ru";             // Ваш уникальный адрес (для отправки СМС по email)
        String email = ConfigurationManager.AppSettings["email"];                                   // Ваш email адрес для отправки
        String smtpLogin = ConfigurationManager.AppSettings["smtpLogin"];                           // Логин для авторизации на SMTP-сервере.
        String smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];                     // Пароль для авторизации на SMTP-сервере.
        String smtpServer = ConfigurationManager.AppSettings["smtpServer"];                         // SMTP-сервер.
        Int32 smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);             // Порт для авторизации на SMTP-сервере.
        Boolean smtpUseSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpUseSSL"]);     // Флаг - использовать SSL.
        Boolean test = Convert.ToBoolean(ConfigurationManager.AppSettings["test"]);                 // Имитирует отправку сообщения для тестирования ваших программ на правильность обработки ответов сервера. При этом само сообщение не отправляется и баланс не расходуется.

        // Адреса-константы для работы с API
        const String tokenUrl = "http://sms.ru/auth/get_token";
        const String sendUrl = "http://sms.ru/sms/send";
        const String statusUrl = "http://sms.ru/sms/status";
        const String costUrl = "http://sms.ru/sms/cost";
        const String balanceUrl = "http://sms.ru/my/balance";
        const String limitUrl = "http://sms.ru/my/limit";
        const String sendersUrl = "http://sms.ru/my/senders";
        const String authUrl = "http://sms.ru/auth/check";
        const String stoplistAddUrl = "http://sms.ru/stoplist/add";
        const String stoplistDelUrl = "http://sms.ru/stoplist/del";
        const String stoplistGetUrl = "http://sms.ru/stoplist/get";

        // Пути к файлам с логами работы.
        String dir = ConfigurationManager.AppSettings["logFolder"];
        String log_sent = ConfigurationManager.AppSettings["logFolder"] + "Sent.txt";
        String log_status = ConfigurationManager.AppSettings["logFolder"] + "Status.txt";
        String log_cost = ConfigurationManager.AppSettings["logFolder"] + "Cost.txt";
        String log_balance = ConfigurationManager.AppSettings["logFolder"] + "Balance.txt";
        String log_limit = ConfigurationManager.AppSettings["logFolder"] + "Limit.txt";
        String log_senders = ConfigurationManager.AppSettings["logFolder"] + "Senders.txt";
        String log_auth = ConfigurationManager.AppSettings["logFolder"] + "Authentication.txt";
        String log_error = ConfigurationManager.AppSettings["logFolder"] + "Error.txt";
        String log_stoplist = ConfigurationManager.AppSettings["logFolder"] + "Stoplist.txt";
        #endregion

        #region Отправка сообщений
        /// <summary>
        /// Совершает отправку СМС сообщения одному или нескольким получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номер телефона получателя (либо несколько номеров, через запятую — до 100 штук за один запрос)</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8</param>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера</returns>
        public String Send(String from, String[] to, String text, EnumAuthenticationTypes authType)
        {
            return Send(from, to, text, DateTime.MinValue, authType);
        }


        /// <summary>
        /// Совершает отправку СМС сообщения одному или нескольким получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номер телефона получателя (либо несколько номеров, через запятую — до 100 штук за один запрос)</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8</param>
        /// <param name="dateTime">Дата и время отправки. Должно быть не больше 7 дней с момента подачи запроса. Если время меньше текущего времени, сообщение отправляется моментально.</param>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера</returns>
        public String Send(String from, String[] to, String text, DateTime dateTime, EnumAuthenticationTypes authType)
        {
            String result = String.Empty;

            if (to.Length < 1)
                throw new ArgumentNullException("to", "Неверные входные данные - массив пуст.");
            if (to.Length > 100)
                throw new ArgumentOutOfRangeException("to", "Неверные входные данные - слишком много элементов (больше 100) в массиве.");
            if (dateTime == DateTime.MinValue)
                dateTime = DateTime.Now;
            if ((DateTime.Now - dateTime).Days > new TimeSpan(7, 0, 0, 0).Days)
                throw new ArgumentOutOfRangeException("dateTime", "Неверные входные данные - должно быть не больше 7 дней с момента подачи запроса.");

            // TODO: Нужно проверить хватит ли баланса. Баланса не хватит, чтобы отправить на все номера - сообщение будет уничтожено (его не получит никто).
                        
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_sent, true))
            {
                String auth = String.Empty;
                String parameters = String.Empty;
                String answer = String.Empty;
                String recipients = String.Empty;
                String token = String.Empty;

                foreach (String item in to)
                {
                    recipients += item + ",";
                }
                recipients = recipients.Substring(0, recipients.Length - 1);
                writer.WriteLine(String.Format("{0}={1}{2}Отправка СМС получателям: {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, recipients));

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("api_id={0}", apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("login={0}&token={1}&sha512={2}", login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("login={0}&token={1}&sha512={2}", login, token, sha512wapi);

                    parameters = String.Format("{0}&to={1}&text={2}&from={3}", auth, recipients, text, from);
                    if (dateTime != DateTime.MinValue)
                        parameters += "&time=" + TimeHelper.GetUnixTime(dateTime);
                    if (partnerId != String.Empty)
                        parameters += "&partner_id=" + partnerId;
                    if (test == true)
                        parameters += "&test=1";
                    writer.WriteLine(String.Format("Запрос: {0}{1}", Environment.NewLine, parameters));

                    WebRequest request = WebRequest.Create(sendUrl);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    Byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                    request.ContentLength = bytes.Length;
                    Stream os = request.GetRequestStream();
                    os.Write(bytes, 0, bytes.Length);
                    os.Close();

                    using (WebResponse resp = request.GetResponse())
                    {
                        if (resp == null) return null;
                        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            answer = sr.ReadToEnd().Trim();
                        }
                    }

                    writer.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/send" + Environment.NewLine);

                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnSendRequest.MessageAccepted))
                    {
                        result = answer;
                    }
                    else
                    {
                        using (StreamWriter w = new StreamWriter(log_error, true))
                        {
                            w.WriteLine(String.Format("{0}={1}{2}Отправка СМС получателям: {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, recipients));
                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/send" + Environment.NewLine);
                        }
                        result = String.Empty;
                    }

                    //WebRequest req = WebRequest.Create(link);
                    //using (WebResponse response = req.GetResponse())
                    //{
                    //    using (Stream stream = response.GetResponseStream())
                    //    {
                    //        if (stream != null)
                    //            using (StreamReader streadReader = new StreamReader(stream))
                    //            {
                    //                answer = streadReader.ReadToEnd();

                    //                writer.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                    //                writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/send" + Environment.NewLine);

                    //                String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                    //                if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnSendRequest.MessageAccepted))
                    //                {
                    //                    result = answer;
                    //                }
                    //                else
                    //                {
                    //                    using (StreamWriter w = new StreamWriter(log_error, true))
                    //                    {
                    //                        w.WriteLine(String.Format("{0}={1}{2}Отправка СМС получателям: {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, recipients));
                    //                        w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                    //                        w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/send" + Environment.NewLine);
                    //                    }
                    //                    result = String.Empty;
                    //                }
                    //            }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Отправка СМС сообщений по электронной почте (более надежно, но нет возможность отслеживать в реальном времени ошибки типа нехватки средств).
        /// </summary>
        /// <param name="to">Номер получателя должен быть написан только цифрами, без пробелов и других знаков. Если вы согласовали с нами имя отправителя, вы можете указать его в заголовке через пробел после номера получателя с префиксом "from:" (пример: 79161234567 from:Service). Если вы хотите указать нескольких получателей (до 50 за раз), укажите их через запятую (но без пробелов), пример: 79161234567,79161234567.</param>
        /// <param name="text">Тело письма - текст сообщения</param>
        /// <returns></returns>
        public ResponseOnSendRequest SendByEmail(String[] to, String text)
        {
            /*
             * Используется отправка по SMTP протоколу.
             * Надежность заключается в том, что в случае если между вашим и нашим сервером наблюдается ошибка связи, протокол SMTP обеспечит гарантированную повторную отправку вашего сообщения.
             * Если бы вы использовали стандартный метод sms/send, вам бы пришлось отслеживать эти ошибки и дополнительно разрабатывать дополнительный программный код для обработки очереди исходящих сообщений.
             */

            ResponseOnSendRequest result = ResponseOnSendRequest.Error;

            if (to.Length < 1)
                throw new ArgumentNullException("to", "Неверные входные данные - массив пуст.");
            if (to.Length > 50)
                throw new ArgumentOutOfRangeException("to", "Неверные входные данные - слишком много элементов (больше 50) в массиве.");

            // TODO: Нужно проверить хватит ли баланса. Баланса не хватит, чтобы отправить на все номера - сообщение будет уничтожено (его не получит никто).

            String recipients = String.Empty;
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_sent, true))
            {
                try
                {
                    foreach (String item in to)
                    {
                        recipients += item + ",";
                    }
                    recipients = recipients.Substring(0, recipients.Length - 1);
                    writer.WriteLine(String.Format("{2}{0}={1}{2}Отправка СМС получателям:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                    writer.WriteLine(recipients + Environment.NewLine);

                    var smtp = new SmtpClient
                    {
                        Host = smtpServer,
                        Port = smtpPort,
                        EnableSsl = smtpUseSSL,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(smtpLogin, smtpPassword),
                        Timeout = 20000
                    };
                    using (var message = new MailMessage(email, SmsRuEmail)
                    {
                        Subject = recipients,
                        BodyEncoding = Encoding.UTF8,
                        IsBodyHtml = false,
                        Body = text
                    })
                    {
                        smtp.Send(message);
                        writer.WriteLine(String.Format("Письмо успешно отправлено."));
                    }

                    result = ResponseOnSendRequest.MessageAccepted;

                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    result = ResponseOnSendRequest.Error;
                }
            }
            return result;
        }

        #endregion

        #region Проверка статуса сообщения
        /// <summary>
        /// Проверка статуса отправленного сообщения. Мы настоятельно рекомендуем использовать систему Callback для более быстрого и удобного получения статусов сообщений.
        /// </summary>
        /// <param name="id">Идентификатор сообщения, полученный при использовании метода sms/send</param>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера.</returns>
        public ResponseOnStatusRequest CheckStatus(String id, EnumAuthenticationTypes authType)
        {
            ResponseOnStatusRequest result = ResponseOnStatusRequest.MethodNotFound;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_status, true))
            {
                writer.WriteLine(String.Format("{0}={1}{2}Проверка статуса по сообщению: {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, id));
                String auth = String.Empty;
                String link = String.Empty;
                String answer = String.Empty;
                String token = String.Empty;

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("{0}?api_id={1}", statusUrl, apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", statusUrl, login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", statusUrl, login, token, sha512wapi);

                    link = String.Format("{0}&id={1}", auth, id);
                    writer.WriteLine(String.Format("Запрос: {0}", link));

                    WebRequest req = WebRequest.Create(link);
                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    answer = sr.ReadToEnd();

                                    writer.WriteLine(String.Format("Ответ: {0}", answer));
                                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/status" + Environment.NewLine);

                                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnStatusRequest.MessageRecieved))
                                    {
                                        result = ResponseOnStatusRequest.MessageRecieved;
                                    }
                                    else
                                    {
                                        using (StreamWriter w = new StreamWriter(log_error, true))
                                        {
                                            writer.WriteLine(String.Format("{0}={1}{2}Проверка статуса сообщения: {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, id));
                                            writer.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                                            writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/status" + Environment.NewLine);
                                        }

                                        result = (ResponseOnStatusRequest)Convert.ToInt32(lines[0]);
                                    }
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }

                    result = ResponseOnStatusRequest.MessageNotFoundOrError;
                }
            }
            return result;
        }
        #endregion

        #region Узнать стоимость сообщения и количество необходимых для отправки сообщений
        /// <summary>
        /// Возвращает стоимость сообщения на указанный номер и количество сообщений, необходимых для его отправки.
        /// </summary>
        /// <param name="to">Номер телефона получателя</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8. Если текст не введен, то возвращается стоимость 1 сообщения. Если текст введен, то возвращается стоимость, рассчитанная по длине сообщения.</param>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Стоимость сообщения на указанный номер и количество сообщений, необходимых для его отправки.</returns>
        public String CheckCost(String to, String text, EnumAuthenticationTypes authType)
        {
            String result = String.Empty;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_cost, true))
            {
                writer.WriteLine(String.Format("{0}={1}{2}Cтоимость сообщения и количество необходимых для отправки сообщений: {3}{2}{4}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, to, text));
                String auth = String.Empty;
                String link = String.Empty;
                String answer = String.Empty;
                String token = String.Empty;

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("{0}?api_id={1}", costUrl, apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", costUrl, login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", costUrl, login, token, sha512wapi);

                    link = String.Format("{0}&to={1}&text={2}", auth, to, text);
                    writer.WriteLine(String.Format("Запрос: {0}", link));

                    WebRequest req = WebRequest.Create(link);
                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    answer = sr.ReadToEnd();
                                    writer.WriteLine(String.Format("Ответ: {0}", answer));
                                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/cost" + Environment.NewLine);

                                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnCostRequest.Done))
                                    {
                                        result = answer;
                                    }
                                    else
                                    {
                                        using (StreamWriter w = new StreamWriter(log_error, true))
                                        {
                                            w.WriteLine(String.Format("{0}={1}{2}Cтоимость сообщения и количество необходимых для отправки сообщений: {3}{2}{4}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, to, text));
                                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=sms/cost" + Environment.NewLine);
                                        }

                                        result = String.Empty;
                                    }
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }
            }
            return result;
        }
        #endregion

        #region Получение состояния баланса
        /// <summary>
        /// Получение состояния баланса.
        /// </summary>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера.</returns>
        public String CheckBalance(EnumAuthenticationTypes authType)
        {
            String result = String.Empty;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_balance, true))
            {
                writer.WriteLine(String.Format("{0}={1}{2}Получение состояния баланса", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                String auth = String.Empty;
                String link = String.Empty;
                String answer = String.Empty;
                String token = String.Empty;

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("{0}?api_id={1}", balanceUrl, apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", balanceUrl, login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", balanceUrl, login, token, sha512wapi);

                    link = String.Format("{0}", auth);
                    writer.WriteLine(String.Format("Запрос: {0}", link));

                    WebRequest req = WebRequest.Create(link);
                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    answer = sr.ReadToEnd();

                                    writer.WriteLine(String.Format("Ответ: {0}", answer));
                                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=my/balance" + Environment.NewLine);

                                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnBalanceRequest.Done))
                                    {
                                        result = answer;
                                    }
                                    else
                                    {
                                        using (StreamWriter w = new StreamWriter(log_error, true))
                                        {
                                            w.WriteLine(String.Format("{0}={1}{2}Получение состояния баланса", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                                            w.WriteLine(String.Format("Ответ: {0}", answer));
                                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=my/balance" + Environment.NewLine);
                                        }

                                        result = String.Empty;
                                    }
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }
            }
            return result;
        }
        #endregion

        #region Получение текущего состояния дневного лимита
        /// <summary>
        /// Получение текущего состояния вашего дневного лимита.
        /// </summary>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера.</returns>
        public String CheckLimit(EnumAuthenticationTypes authType)
        {
            String result = String.Empty;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_limit, true))
            {
                writer.WriteLine(String.Format("{0}={1}{2}Получение текущего состояния дневного лимита:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                String auth = String.Empty;
                String link = String.Empty;
                String answer = String.Empty;
                String token = String.Empty;

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("{0}?api_id={1}", limitUrl, apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", limitUrl, login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", limitUrl, login, token, sha512wapi);

                    link = String.Format("{0}", auth);
                    writer.WriteLine(String.Format("Запрос: {0}", link));

                    WebRequest req = WebRequest.Create(link);
                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    answer = sr.ReadToEnd();

                                    writer.WriteLine(String.Format("Ответ: {0}", answer));
                                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=my/limit" + Environment.NewLine);

                                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnLimitRequest.Done))
                                    {
                                        result = answer;
                                    }
                                    else
                                    {
                                        using (StreamWriter w = new StreamWriter(log_error, true))
                                        {
                                            w.WriteLine(String.Format("{0}={1}{2}Получение текущего состояния дневного лимита:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=my/limit" + Environment.NewLine);
                                        }

                                        result = String.Empty;
                                    }
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }
            }
            return result;
        }
        #endregion

        #region Получение списка отправителей
        /// <summary>
        /// Получение списка отправителей
        /// </summary>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера.</returns>
        public String CheckSenders(EnumAuthenticationTypes authType)
        {
            String result = String.Empty;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_senders, true))
            {
                writer.WriteLine(String.Format("{0}={1}{2}Получение списка отправителей:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                String auth = String.Empty;
                String link = String.Empty;
                String answer = String.Empty;
                String token = String.Empty;

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("{0}?api_id={1}", sendersUrl, apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", sendersUrl, login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", sendersUrl, login, token, sha512wapi);

                    link = String.Format("{0}", auth);
                    writer.WriteLine(String.Format("Запрос: {0}", link));

                    WebRequest req = WebRequest.Create(link);
                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    answer = sr.ReadToEnd();

                                    writer.WriteLine(String.Format("Ответ: {0}", answer));
                                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=my/senders" + Environment.NewLine);

                                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnSendersRequest.Done))
                                    {
                                        result = answer;
                                    }
                                    else
                                    {
                                        using (StreamWriter w = new StreamWriter(log_error, true))
                                        {
                                            w.WriteLine(String.Format("{0}={1}{2}Получение списка отправителей:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=my/senders" + Environment.NewLine);
                                        }
                                    }
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }
            }
            return result;
        }
        #endregion

        #region Получение токена
        /// <summary>
        /// Получение временного ключа, с помощью которого в дальнейшем шифруется пароль. Используется в методах, требущих усиленную авторизацию. Закреплен за вашим IP адресом и работает только в течение 10 минут.
        /// </summary>
        /// <returns>32х значный ключ, закрепленный за вашим ip адресом на 10 минут, который используется для шифрования вашего пароля в других методах.</returns>
        public String GetToken()
        {
            String result = String.Empty;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            try
            {
                WebRequest request = WebRequest.Create(tokenUrl);
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                result = sr.ReadToEnd();
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = new StreamWriter(log_error, true))
                {
                    w.WriteLine("Возникла ошибка при получении токена по адресу http://sms.ru/auth/get_token.");
                    w.WriteLine(ex.Message);
                    w.WriteLine(ex.StackTrace + Environment.NewLine);
                }
            }
            return result;
        }
        #endregion

        #region Проверка статуса сообщения
        /// <summary>
        /// Проверка номера телефона и пароля на действительность
        /// </summary>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера.</returns>
        public ResponseOnAuthRequest CheckAuth(EnumAuthenticationTypes authType)
        {
            ResponseOnAuthRequest result = ResponseOnAuthRequest.Error;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_auth, true))
            {
                writer.WriteLine(String.Format("{0}={1}{2}Проверка номера телефона и пароля на действительность:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                String auth = String.Empty;
                String link = String.Empty;
                String answer = String.Empty;
                String token = String.Empty;

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("{0}?api_id={1}", authUrl, apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", authUrl, login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", authUrl, login, token, sha512wapi);

                    link = String.Format("{0}", auth);
                    writer.WriteLine(String.Format("Запрос: {0}", link));

                    WebRequest req = WebRequest.Create(link);
                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    answer = sr.ReadToEnd();

                                    writer.WriteLine(String.Format("Ответ: {0}", answer));
                                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=auth/check" + Environment.NewLine);

                                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnAuthRequest.Done))
                                    {
                                        result = ResponseOnAuthRequest.Done;
                                    }
                                    else
                                    {
                                        using (StreamWriter w = new StreamWriter(log_error, true))
                                        {
                                            w.WriteLine(String.Format("{0}={1}{2}Проверка номера телефона и пароля на действительность:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=auth/check" + Environment.NewLine);
                                        }

                                        result = (ResponseOnAuthRequest)Convert.ToInt32(lines[0]);
                                    }
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }

                    result = ResponseOnAuthRequest.Error;
                }
            }
            return result;
        }
        #endregion

        #region Операции с Stoplist
        /// <summary>
        /// На номера, добавленные в стоплист, не доставляются сообщения (и за них не списываются деньги).
        /// </summary>
        /// <param name="phone">Номер телефона.</param>
        /// <param name="text">Примечание (доступно только вам).</param>
        /// <returns>Номер добавлен в стоплист = true, иначе - false</returns>
        public Boolean StoplistAdd(String phone, String text, EnumAuthenticationTypes authType)
        {
            Boolean result = false;

            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException("text", "Неверные входные данные - обязательный параметр.");                  
            
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_stoplist, true))
            {
                String auth = String.Empty;
                String parameters = String.Empty;
                String answer = String.Empty;
                String recipients = String.Empty;
                String token = String.Empty;

                writer.WriteLine(String.Format("Добавление номера в стоплист:{0}Номер: {1}, Примечание: {2}", Environment.NewLine, phone, text));

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("api_id={0}", apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("login={0}&token={1}&sha512={2}", login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("login={0}&token={1}&sha512={2}", login, token, sha512wapi);

                    parameters = String.Format("{0}&stoplist_phone={1}&stoplist_text={2}", auth, phone, text);
                   
                    writer.WriteLine(String.Format("Запрос: {0}{1}", Environment.NewLine, parameters));

                    WebRequest request = WebRequest.Create(stoplistAddUrl);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    Byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                    request.ContentLength = bytes.Length;
                    Stream os = request.GetRequestStream();
                    os.Write(bytes, 0, bytes.Length);
                    os.Close();

                    using (WebResponse resp = request.GetResponse())
                    {
                        if (resp == null) return false;
                        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            answer = sr.ReadToEnd().Trim();
                        }
                    }

                    writer.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=stoplist/add" + Environment.NewLine);

                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnStoplistAddRequest.Done))
                    {
                        result = true;
                    }
                    else
                    {
                        using (StreamWriter w = new StreamWriter(log_error, true))
                        {
                            w.WriteLine(String.Format("{0}={1}{2}Добавление номера в стоплист:{2}Номер: {3}, Примечание: {4}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, phone, text));
                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=stoplist/add" + Environment.NewLine);
                        }
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Удаляет один номер из стоплиста
        /// </summary>
        /// <param name="phone">Номер телефона.</param>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Номер удален из стоплиста = true, иначе - false</returns>
        public Boolean StoplistDelete(String phone, EnumAuthenticationTypes authType)
        {
            Boolean result = false;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_stoplist, true))
            {
                String auth = String.Empty;
                String parameters = String.Empty;
                String answer = String.Empty;
                String recipients = String.Empty;
                String token = String.Empty;

                writer.WriteLine(String.Format("Удаление номера из стоплиста:{0}Номер: {1}", Environment.NewLine, phone));

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("api_id={0}", apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("login={0}&token={1}&sha512={2}", login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("login={0}&token={1}&sha512={2}", login, token, sha512wapi);

                    parameters = String.Format("{0}&stoplist_phone={1}", auth, phone);

                    writer.WriteLine(String.Format("Запрос: {0}{1}", Environment.NewLine, parameters));

                    WebRequest request = WebRequest.Create(stoplistDelUrl);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    Byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                    request.ContentLength = bytes.Length;
                    Stream os = request.GetRequestStream();
                    os.Write(bytes, 0, bytes.Length);
                    os.Close();

                    using (WebResponse resp = request.GetResponse())
                    {
                        if (resp == null) return false;
                        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            answer = sr.ReadToEnd().Trim();
                        }
                    }

                    writer.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=stoplist/del" + Environment.NewLine);

                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnStoplistDeleteRequest.Done))
                    {
                        result = true;
                    }
                    else
                    {
                        using (StreamWriter w = new StreamWriter(log_error, true))
                        {
                            w.WriteLine(String.Format("{0}={1}{2}Удаление номера из стоплиста:{2}Номер: {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine, phone));
                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=stoplist/del" + Environment.NewLine);
                        }
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }

            }
            return result;
        }

        /// <summary>
        /// Весь стоплист выгружается в формате номер;примечание на отдельных строчках (первая строчка - цифра 100).
        /// </summary>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Запрос обработан. На последующих строчках будут идти номера телефонов, указанных в стоплисте в формате номер;примечание.</returns>
        public String StoplistGet(EnumAuthenticationTypes authType)
        {
            String result = String.Empty;

            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка доступа", ex.InnerException);
            }

            using (StreamWriter writer = new StreamWriter(log_stoplist, true))
            {
                String auth = String.Empty;
                String link = String.Empty;
                String answer = String.Empty;
                String token = String.Empty;

                writer.WriteLine(String.Format("Получение номеров из стоплиста:"));

                try
                {
                    token = GetToken();

                    String sha512 = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}", password, token)).ToLower();
                    String sha512wapi = HashCodeHelper.GetSHA512Hash(String.Format("{0}{1}{2}", password, token, apiId)).ToLower();

                    if (authType == EnumAuthenticationTypes.Simple)
                        auth = String.Format("{0}?api_id={1}", stoplistGetUrl, apiId);
                    if (authType == EnumAuthenticationTypes.Strong)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", stoplistGetUrl, login, token, sha512);
                    if (authType == EnumAuthenticationTypes.StrongApi)
                        auth = String.Format("{0}?login={1}&token={2}&sha512={3}", stoplistGetUrl, login, token, sha512wapi);

                    link = String.Format("{0}", auth);
                    writer.WriteLine(String.Format("Запрос: {0}", link));

                    WebRequest req = WebRequest.Create(link);
                    using (WebResponse response = req.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    answer = sr.ReadToEnd();

                                    writer.WriteLine(String.Format("Ответ: {0}", answer));
                                    writer.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=stoplist/get" + Environment.NewLine);

                                    String[] lines = answer.Split(new String[] { "\n" }, StringSplitOptions.None);
                                    if (Convert.ToInt32(lines[0]) == Convert.ToInt32(ResponseOnStoplistGetRequest.Done))
                                    {
                                        result = answer;
                                    }
                                    else
                                    {
                                        using (StreamWriter w = new StreamWriter(log_error, true))
                                        {
                                            w.WriteLine(String.Format("{0}={1}{2}Получение номеров из стоплиста:", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), Environment.NewLine));
                                            w.WriteLine(String.Format("Ответ: {0}{1}", Environment.NewLine, answer));
                                            w.WriteLine("Документация - http://sms.ru/?panel=api&subpanel=method&show=stoplist/get" + Environment.NewLine);
                                        }
                                    }
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace + Environment.NewLine);

                    using (StreamWriter w = new StreamWriter(log_error, true))
                    {
                        w.WriteLine("Возникла непонятная ошибка. Нужно проверить значения в файле конфигурации и разобраться в коде. Скорее всего введены неверные значения, либо сервер SMS.RU недоступен.");
                        w.WriteLine(ex.Message);
                        w.WriteLine(ex.StackTrace + Environment.NewLine);
                    }
                }

            }
            return result;
        }
        #endregion
    }
}