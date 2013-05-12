using System;

namespace SmsRu.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с классом System.DateTime.
    /// </summary>
    class TimeHelper
    {
        /// <summary>
        /// Получить текущее время в UNIX-формате.
        /// </summary>
        /// <returns>Время в UNIX-формате.</returns>
        public static Int32 GetCurrentUnixTime()
        {            
            return GetUnixTime(DateTime.Now);
        }

        /// <summary>
        /// Получить время на определённый момент в UNIX-формате.
        /// </summary>
        /// <param name="dateTime">Требуемый момент времени.</param>
        /// <returns>Время в UNIX-формате.</returns>
        public static Int32 GetUnixTime(DateTime dateTime)
        {
            // http://en.wikipedia.org/wiki/Unix_time

            dateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
            TimeSpan t = (dateTime - new DateTime(1970, 1, 1, 0, 0, 0));

            return (Int32)t.TotalSeconds;
        }
    }
}
