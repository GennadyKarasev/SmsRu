using System;

namespace SmsRu.Helpers
{
    /// <summary>
    /// Вспомогательный класс для конвертации различных данных.
    /// </summary>
    public static class ConvertersHelper
    {
        // http://stackoverflow.com/a/624379
        // Почему этот метод быстрее BitConverter.
        public static String ByteArrayToHex(Byte[] bytes)
        {
            Char[] c = new Char[bytes.Length * 2];
            Int32 b;
            for (Int32 i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (Char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (Char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new String(c);
        }
    }
}
