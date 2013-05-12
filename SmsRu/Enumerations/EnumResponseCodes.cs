using System;

namespace SmsRu.Enumerations
{
    /// <summary>
    /// Отправка СМС сообщения одному или нескольким получателям.
    /// </summary>
    public enum ResponseOnSendRequest 
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Сообщение принято к отправке. На следующих строчках вы найдете идентификаторы отправленных сообщений в том же порядке, в котором вы указали номера, на которых совершалась отправка.</summary>
        MessageAccepted = 100,
        /// <summary>Неправильный api_id.</summary>
        WrongID = 200,
        /// <summary>Не хватает средств на лицевом счету.</summary>
        OutOfMoney = 201,
        /// <summary>Неправильно указан получатель.</summary>
        BadRecipient = 202,
        /// <summary>Нет текста сообщения.</summary>
        MessageTextNotSpecified = 203,
        /// <summary>Имя отправителя не согласовано с администрацией.</summary>
        BadSender = 204,
        /// <summary>Сообщение слишком длинное (превышает 8 СМС).</summary>
        LongMessage = 205,
        /// <summary>Будет превышен или уже превышен дневной лимит на отправку сообщений.</summary>
        DayMessageLimit = 206,
        /// <summary>На этот номер (или один из номеров) нельзя отправлять сообщения, либо указано более 100 номеров в списке получателей.</summary>
        CantSendToThisNumber = 207,
        /// <summary>Параметр time указан неправильно.</summary>
        WrongTime = 208,
        /// <summary>Вы добавили этот номер (или один из номеров) в стоп-лист.</summary>
        BlacklistedRecepient = 209,
        /// <summary>Используется GET, где необходимо использовать POST.</summary>
        ShouldUsePOST = 210,
        /// <summary>Метод не найден.</summary>
        MethodNotFound = 211,
        /// <summary>Сервис временно недоступен, попробуйте чуть позже</summary>
        ServiceIsUnavailable = 220,
        /// <summary>Неправильный token (возможно истек срок действия, либо ваш IP изменился).</summary>
        WrongToken = 300,
        /// <summary>Неправильный пароль, либо пользователь не найден.</summary>
        WrongPassword = 301,
        /// <summary>Пользователь авторизован, но аккаунт не подтвержден (пользователь не ввел код, присланный в регистрационной смс).</summary>
        AccountNotVerified = 302
    }

    /// <summary>
    /// Статус отправленного сообщения.
    /// </summary>
    public enum ResponseOnStatusRequest
    {
        /// <summary>Сообщение не найдено или ошибка в коде.</summary>
        MessageNotFoundOrError = -1,
        /// <summary>Сообщение находится в очереди SMS.RU.</summary>
        Queued = 100,
        /// <summary>Сообщение передается оператору.</summary>
        SendingToTheOperator = 101,
        /// <summary>Сообщение отправлено (в пути).</summary>
        SendingToTheRecepient = 102,
        /// <summary>Сообщение доставлено.</summary>
        MessageRecieved = 103,
        /// <summary>Не может быть доставлено: время жизни истекло.</summary>
        FailedOutOfTime = 104,
        /// <summary>Не может быть доставлено: удалено оператором.</summary>
        CancelledByOperator = 105,
        /// <summary>Не может быть доставлено: сбой в телефоне.</summary>
        PhoneMalfunction = 106,
        /// <summary>Не может быть доставлено: неизвестная причина.</summary>
        ReasonUnknown = 107,
        /// <summary>Не может быть доставлено: отклонено.</summary>
        MessageDeclined = 108,
        /// <summary>Неправильный api_id.</summary>
        WrongApiID = 200,
        /// <summary>Используется GET, где необходимо использовать POST.</summary>
        ShouldUsePOST = 201,
        /// <summary>Метод не найден.</summary>
        MethodNotFound = 211,
        /// <summary>Сервис временно недоступен, попробуйте чуть позже.</summary>
        ServiceIsUnavailable = 220,
        /// <summary>Неправильный token (возможно истек срок действия, либо ваш IP изменился).</summary>
        WrongToken = 300,
        /// <summary>Неправильный пароль, либо пользователь не найден.</summary>
        WrongPassword = 301,
        /// <summary>Пользователь авторизован, но аккаунт не подтвержден (пользователь не ввел код, присланный в регистрационной смс).</summary>
        AccountNotVerified = 302
    }

    /// <summary>
    /// Стоимость сообщения на указанный номер и количество сообщений, необходимых для его отправки.
    /// </summary>
    public enum ResponseOnCostRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Запрос выполнен. На второй строчке будет указана стоимость сообщения. На третьей строчке будет указана его длина.</summary>
        Done = 100,
        /// <summary>Неправильный api_id.</summary>
        WrongApiID = 200,
        /// <summary>Неправильно указан получатель.</summary>
        WrongRecepient = 202,
        /// <summary>На этот номер нельзя отправлять сообщения.</summary>
        BadRecepient = 207,
        /// <summary>Используется GET, где необходимо использовать POST.</summary>
        ShouldUsePOST = 210,
        /// <summary>Метод не найден.</summary>
        MethodNotFound = 211,
        /// <summary>Сервис временно недоступен, попробуйте чуть позже.</summary>
        ServiceIsUnavailable = 220,
        /// <summary>Неправильный token (возможно истек срок действия, либо ваш IP изменился).</summary>
        WrongToken = 300,
        /// <summary>Неправильный пароль, либо пользователь не найден.</summary>
        WrongPassword = 301,
        /// <summary>Пользователь авторизован, но аккаунт не подтвержден (пользователь не ввел код, присланный в регистрационной смс).</summary>
        AccountNotVerified = 302
    }

    /// <summary>
    /// Cостояние баланса.
    /// </summary>
    public enum ResponseOnBalanceRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Запрос выполнен. На второй строчке вы найдете ваше текущее состояние баланса.</summary>
        Done = 100,
        /// <summary>Неправильный api_id.</summary>
        WrongApiID = 200,
        /// <summary>Используется GET, где необходимо использовать POST.</summary>
        ShouldUsePOST = 210,
        /// <summary>Метод не найден.</summary>
        MethodNotFound = 211,
        /// <summary>Сервис временно недоступен, попробуйте чуть позже.</summary>
        ServiceIsUnavailable = 220,
        /// <summary>Неправильный token (возможно истек срок действия, либо ваш IP изменился).</summary>
        WrongToken = 300,
        /// <summary>Неправильный пароль, либо пользователь не найден.</summary>
        WrongPassword = 301,
        /// <summary>Пользователь авторизован, но аккаунт не подтвержден (пользователь не ввел код, присланный в регистрационной смс).</summary>
        AccountNotVerified = 302
    }

    /// <summary>
    /// Текущее состояние дневного лимита.
    /// </summary>
    public enum ResponseOnLimitRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Запрос выполнен. На второй строчке вы найдете ваше текущее дневное ограничение. На третьей строчке количество сообщений, отправленных вами в текущий день.</summary>
        Done = 100,
        /// <summary>Неправильный api_id.</summary>
        WrongApiID = 200,
        /// <summary>Используется GET, где необходимо использовать POST.</summary>
        ShouldUsePOST = 210,
        /// <summary>Метод не найден.</summary>
        MethodNotFound = 211,
        /// <summary>Сервис временно недоступен, попробуйте чуть позже.</summary>
        ServiceIsUnavailable = 220,
        /// <summary>Неправильный token (возможно истек срок действия, либо ваш IP изменился).</summary>
        WrongToken = 300,
        /// <summary>Неправильный пароль, либо пользователь не найден.</summary>
        WrongPassword = 301,
        /// <summary>Пользователь авторизован, но аккаунт не подтвержден (пользователь не ввел код, присланный в регистрационной смс).</summary>
        AccountNotVerified = 302
    }

    /// <summary>
    /// Список отправителей.
    /// </summary>
    public enum ResponseOnSendersRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Запрос выполнен. На второй и последующих строчках вы найдете ваших одобренных отправителей, которые можно использовать в параметре &from= метода sms/send..</summary>
        Done = 100,
        /// <summary>Неправильный api_id.</summary>
        WrongApiID = 200,
        /// <summary>Используется GET, где необходимо использовать POST.</summary>
        ShouldUsePOST = 210,
        /// <summary>Метод не найден.</summary>
        MethodNotFound = 211,
        /// <summary>Сервис временно недоступен, попробуйте чуть позже.</summary>
        ServiceIsUnavailable = 220,
        /// <summary>Неправильный token (возможно истек срок действия, либо ваш IP изменился).</summary>
        WrongToken = 300,
        /// <summary> Неправильный пароль, либо пользователь не найден.</summary>
        WrongPassword = 301,
        /// <summary>Пользователь авторизован, но аккаунт не подтвержден (пользователь не ввел код, присланный в регистрационной смс).</summary>
        AccountNotVerified = 302
    }

    /// <summary>
    /// Проверка номера телефона и пароля на действительность.
    /// </summary>
    public enum ResponseOnAuthRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>ОК, номер телефона и пароль совпадают.</summary>
        Done = 100,
        /// <summary>Неправильный token (возможно истек срок действия, либо ваш IP изменился).</summary>
        WrongToken = 300,
        /// <summary>Неправильный пароль, либо пользователь не найден.</summary>
        WrongPassword = 301,
        /// <summary>Пользователь авторизован, но аккаунт не подтвержден (пользователь не ввел код, присланный в регистрационной смс).</summary>
        AccountNotVerified = 302
    }

    /// <summary>
    /// Добавление номера в stoplist.
    /// </summary>
    public enum ResponseOnStoplistAddRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Запрос выполнен. Номер добавлен в стоплист.</summary>
        Done = 100,
        /// <summary>Номер телефона в неправильном формате.</summary>
        WrongFormat = 202
    }

    /// <summary>
    /// Удаление номера из stoplist.
    /// </summary>
    public enum ResponseOnStoplistDeleteRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Запрос выполнен. Номер удален из стоплиста.</summary>
        Done = 100,
        /// <summary>Номер телефона в неправильном формате.</summary>
        WrongFormat = 202
    }

    /// <summary>
    /// Получение stoplist.
    /// </summary>
    public enum ResponseOnStoplistGetRequest
    {
        /// <summary>Ошибка в коде.</summary>
        Error = -1,
        /// <summary>Запрос обработан. На последующих строчках будут идти номера телефонов, указанных в стоплисте в формате номер;примечание.</summary>
        Done = 100
    }
}
