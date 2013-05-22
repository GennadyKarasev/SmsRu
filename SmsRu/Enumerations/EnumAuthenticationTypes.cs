using System;

namespace SmsRu.Enumerations
{
    /// <summary>
    /// Тип авторизации пользователя на SMS.RU.
    /// </summary>
    public enum EnumAuthenticationTypes
    {
        /// <summary>Простая авторизация</summary>
        Simple = 0,
        /// <summary>Усиленная авторизация без api_id</summary>
        Strong = 1,
        /// <summary>Усиленная авторизация с api_id</summary>
        StrongApi = 3
    }
}
