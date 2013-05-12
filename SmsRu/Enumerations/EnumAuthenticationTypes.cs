using System;

namespace SmsRu.Enumerations
{
    /// <summary>
    /// 
    /// </summary>
    public enum EnumAuthenticationTypes
    {
        /// <summary>Простая авторизация</summary>
        Simple = 0,
        /// <summary>Усиленная авторизация без api_id (необязательно)</summary>
        Strong = 1,
        /// <summary>Усиленная авторизация с api_id (необязательно):</summary>
        StrongApi = 3
    }
}
