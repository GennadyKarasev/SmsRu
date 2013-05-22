using SmsRu.Enumerations;
using System;
using System.Collections.Generic;

namespace SmsRu
{
    public interface ISmsProvider
    {
        /// <summary>
        /// Совершает отправку СМС сообщения одному получателю.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номер телефона получателя.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String to, String text);

        /// <summary>
        /// Совершает отправку СМС сообщения одному получателю.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номер телефона получателя.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <param name="dateTime">Время отправки. Должно быть не больше 7 дней с момента подачи запроса. Если время меньше текущего времени, сообщение отправляется моментально.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String to, String text, DateTime dateTime);

        /// <summary>
        /// Совершает отправку СМС сообщения одному получателю.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номер телефона получателя.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String to, String text, EnumAuthenticationTypes authType);

        /// <summary>
        /// Совершает отправку СМС сообщения одному получателю.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номер телефона получателя.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <param name="dateTime">Время отправки. Должно быть не больше 7 дней с момента подачи запроса. Если время меньше текущего времени, сообщение отправляется моментально.</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String to, String text, DateTime dateTime, EnumAuthenticationTypes authType);

        /// <summary>
        /// Совершает отправку СМС сообщения нескольким получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номера телефонов получателей.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String[] to, String text);

        /// <summary>
        /// Совершает отправку СМС сообщения нескольким получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номера телефонов получателей.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <param name="dateTime">Время отправки. Должно быть не больше 7 дней с момента подачи запроса. Если время меньше текущего времени, сообщение отправляется моментально.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String[] to, String text, DateTime dateTime);

        /// <summary>
        /// Совершает отправку СМС сообщения нескольким получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номера телефонов получателей.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String[] to, String text, EnumAuthenticationTypes authType);      
  
        /// <summary>
        /// Совершает отправку СМС сообщения нескольким получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="to">Номера телефонов получателей.</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8.</param>
        /// <param name="dateTime">Время отправки. Должно быть не больше 7 дней с момента подачи запроса. Если время меньше текущего времени, сообщение отправляется моментально.</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String Send(String from, String[] to, String text, DateTime dateTime, EnumAuthenticationTypes authType);

        /// <summary>
        /// Совершает отправку разных СМС сообщений разным получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="toAndText">Пара получатель-текст сообщения (ключ-значение).</param>
        /// <returns>Ответ сервера.</returns>
        String SendMultiple(String from, Dictionary<String, String> toAndText);

        /// <summary>
        /// Совершает отправку разных СМС сообщений разным получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="toAndText">Пара получатель-текст сообщения (ключ-значение).</param>
        /// <param name="dateTime">Время отправки. Должно быть не больше 7 дней с момента подачи запроса. Если время меньше текущего времени, сообщение отправляется моментально.</param>
        /// <returns>Ответ сервера.</returns>
        String SendMultiple(String from, Dictionary<String, String> toAndText, DateTime dateTime);

        /// <summary>
        /// Совершает отправку разных СМС сообщений разным получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="toAndText">Пара получатель-текст сообщения (ключ-значение).</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String SendMultiple(String from, Dictionary<String, String> toAndText, EnumAuthenticationTypes authType);

        /// <summary>
        /// Совершает отправку разных СМС сообщений разным получателям.
        /// </summary>
        /// <param name="from">Имя отправителя (должно быть согласовано с администрацией). Если не заполнено, в качестве отправителя будет указан ваш номер.</param>
        /// <param name="toAndText">Пара получатель-текст сообщения (ключ-значение).</param>
        /// <param name="dateTime">Время отправки. Должно быть не больше 7 дней с момента подачи запроса. Если время меньше текущего времени, сообщение отправляется моментально.</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String SendMultiple(String from, Dictionary<String, String> toAndText, DateTime dateTime, EnumAuthenticationTypes authType);

        /// <summary>
        /// Отправка СМС сообщений по электронной почте (более надежно, но нет возможность отслеживать в реальном времени ошибки типа нехватки средств).
        /// </summary>
        /// <param name="to">Номер получателя должен быть написан только цифрами, без пробелов и других знаков. Если вы согласовали с нами имя отправителя, вы можете указать его в заголовке через пробел после номера получателя с префиксом "from:" (пример: 79161234567 from:Service). Если вы хотите указать нескольких получателей (до 50 за раз), укажите их через запятую (но без пробелов), пример: 79161234567,79161234567.</param>
        /// <param name="text">Тело письма - текст сообщения.</param>
        /// <returns></returns>
        ResponseOnSendRequest SendByEmail (String[] to, String text);

        /// <summary>
        /// Проверка статуса отправленного сообщения. Мы настоятельно рекомендуем использовать систему Callback для более быстрого и удобного получения статусов сообщений.
        /// </summary>
        /// <param name="id">Идентификатор сообщения, полученный при использовании метода sms/send</param>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Ответ сервера.</returns>
        ResponseOnStatusRequest CheckStatus (String id, EnumAuthenticationTypes authType);

        /// <summary>
        /// Возвращает стоимость сообщения на указанный номер и количество сообщений, необходимых для его отправки.
        /// </summary>
        /// <param name="to">Номер телефона получателя</param>
        /// <param name="text">Текст сообщения в кодировке UTF-8. Если текст не введен, то возвращается стоимость 1 сообщения. Если текст введен, то возвращается стоимость, рассчитанная по длине сообщения.</param>
        /// <param name="authType">Тип авторизации - простая, усиленная без api_id, усиленная с api_id</param>
        /// <returns>Стоимость сообщения на указанный номер и количество сообщений, необходимых для его отправки.</returns>
        String CheckCost(String to, String text, EnumAuthenticationTypes authType);
        
        /// <summary>
        /// Получение состояния баланса.
        /// </summary>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String CheckBalance(EnumAuthenticationTypes authType);

        /// <summary>
        /// Получение текущего состояния вашего дневного лимита.
        /// </summary>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String CheckLimit(EnumAuthenticationTypes authType);

        /// <summary>
        /// Получение списка отправителей
        /// </summary>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        String CheckSenders(EnumAuthenticationTypes authType);

        /// <summary>
        /// Проверка номера телефона и пароля на действительность
        /// </summary>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Ответ сервера.</returns>
        ResponseOnAuthRequest AuthCheck(EnumAuthenticationTypes authType);

        /// <summary>
        /// Получение временного ключа, с помощью которого в дальнейшем шифруется пароль. Используется в методах, требущих усиленную авторизацию. Закреплен за вашим IP адресом и работает только в течение 10 минут.
        /// </summary>
        /// <returns>32х значный ключ, закрепленный за вашим ip адресом на 10 минут, который используется для шифрования вашего пароля в других методах.</returns>
        String GetToken();

        /// <summary>
        /// На номера, добавленные в стоплист, не доставляются сообщения (и за них не списываются деньги).
        /// </summary>
        /// <param name="phone">Номер телефона.</param>
        /// <param name="text">Примечание (доступно только вам).</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Номер добавлен в стоплист = true, иначе - false</returns>
        Boolean StoplistAdd(String phone, String text, EnumAuthenticationTypes authType);

        /// <summary>
        /// Удаляет один номер из стоплиста
        /// </summary>
        /// <param name="phone">Номер телефона.</param>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Номер удален из стоплиста = true, иначе - false</returns>
        Boolean StoplistDelete(String phone, EnumAuthenticationTypes authType);

        /// <summary>
        /// Весь стоплист выгружается в формате номер;примечание на отдельных строчках (первая строчка - цифра 100).
        /// </summary>
        /// <param name="authType">Тип авторизации на сервере.</param>
        /// <returns>Запрос обработан. На последующих строчках будут идти номера телефонов, указанных в стоплисте в формате номер;примечание.</returns>
        String StoplistGet(EnumAuthenticationTypes authType);
    }
}
