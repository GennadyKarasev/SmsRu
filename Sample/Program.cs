using SmsRu;
using SmsRu.Enumerations;
using System;
using System.Collections.Generic;

namespace Sample
{
    class Program
    {
        static void Main(String[] args)
        {
            // Не забыть настроить переменные в app.config!

            Console.WriteLine("Проверка работы API SMS.ru:\n");

            ISmsProvider sms = new SmsRuProvider();

            Console.WriteLine("Метод Send:");
            Console.WriteLine(sms.Send("79161234567", new String[] { "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString(), EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.Send("79161234567", "79161234567", DateTime.Now.ToLongTimeString()));
            Console.WriteLine(sms.Send("79161234567", new String[] { "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString()));
            Console.WriteLine(sms.SendMultiple("79161234567", new Dictionary<String, String>() { { "+79161234567", "Первое сообщение" }, { "+79161234567", "Второе сообщение" } }));
            Console.WriteLine(sms.Send("79161234567", new String[] { "79161234567", "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString(), EnumAuthenticationTypes.StrongApi));
            DateTime tomorrow = DateTime.Now + new TimeSpan(24, 0, 0);
            Console.WriteLine(sms.Send("79161234567", new String[] { "79161234567", "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString(), tomorrow, EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод SendByEmail:");
            Console.WriteLine(sms.SendByEmail(new String[] { "79161234567" }, DateTime.Now.ToLongTimeString()));

            Console.WriteLine("\nМетод CheckStatus:");
            Console.WriteLine(sms.CheckStatus("201320-126105", EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.CheckStatus("201320-126105", EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.CheckStatus("201320-126105", EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод CheckCost:");
            Console.WriteLine(sms.CheckCost("79161234567", "Сообщение длинной 1 SMS: написано кириллицей,не может превышать 70 зн.", EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.CheckCost("79161234567", "Сообщение длинной 6 SMS: В стандарте также предусмотрена возможность отправлять сегментированные сообщения. В таких сообщениях в заголовке пользовательских данных помещается информация о номере сегмента сообщения и общем количестве сегментов. Максимальная длина сегмента при этом уменьшается за счет этого заголовка. Как правило, каждый сегмент тарифицируется как отдельное сообщение.", EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.CheckCost("79161234567", "Сообщение длинной 3 SMS:  Сегментирование поддерживают почти все современные телефоны, но часто в телефонах вводится ограничение на количество сегментов в сообщении.", EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод CheckBalance:");
            Console.WriteLine(sms.CheckBalance(EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.CheckBalance(EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.CheckBalance(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод CheckLimit:");
            Console.WriteLine(sms.CheckLimit(EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.CheckLimit(EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.CheckLimit(EnumAuthenticationTypes.StrongApi));
            Console.WriteLine("\nМетод CheckSenders:");
            Console.WriteLine(sms.CheckSenders(EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.CheckSenders(EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.CheckSenders(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод AuthCheck:");
            Console.WriteLine(sms.AuthCheck(EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.AuthCheck(EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.AuthCheck(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод StoplistAdd:");
            Console.WriteLine(sms.StoplistAdd("79161234567", "ignore", EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.StoplistAdd("79161234568", "ignore", EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.StoplistAdd("79161234569", "ignore", EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод StoplistGet:");
            Console.WriteLine(sms.StoplistGet(EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.StoplistGet(EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.StoplistGet(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод StoplistDelete:");
            Console.WriteLine(sms.StoplistDelete("79161234567", EnumAuthenticationTypes.Simple));
            Console.WriteLine(sms.StoplistDelete("79161234569", EnumAuthenticationTypes.Strong));
            Console.WriteLine(sms.StoplistDelete("79161234568", EnumAuthenticationTypes.StrongApi));            

            Console.WriteLine("\nГотово.");
            Console.ReadLine();
        }
    }
}
