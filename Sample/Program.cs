using SmsRu;
using SmsRu.Enumerations;
using System;

namespace Sample
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("Проверка работы API SMS.ru:\n");

            ISmsProvider smsProdiver = new SmsRuProvider();

            Console.WriteLine("Метод Send:");
            Console.WriteLine(smsProdiver.Send("79161234567", new String[] { "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString(), EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.Send("79161234567", new String[] { "79161234567" }, DateTime.Now.ToLongTimeString(), EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.Send("79161234567", new String[] { "79161234567", "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString(), EnumAuthenticationTypes.StrongApi));
            DateTime tomorrow = DateTime.Now + new TimeSpan(24,0,0);
            Console.WriteLine(smsProdiver.Send("79161234567", new String[] { "79161234567", "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString(), tomorrow, EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод SendByEmail:");
            Console.WriteLine(smsProdiver.SendByEmail(new String[] { "79161234567" }, DateTime.Now.ToLongTimeString()));

            Console.WriteLine("\nМетод CheckStatus:");
            Console.WriteLine(smsProdiver.CheckStatus("13-4969322", EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.CheckStatus("13-4969322", EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.CheckStatus("13-4969322", EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод CheckCost:");
            Console.WriteLine(smsProdiver.CheckCost("79161234567", "Сообщение длинной 1 SMS: написано кириллицей,не может превышать 70 зн.", EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.CheckCost("79161234567", "Длинное сообщение: В стандарте также предусмотрена возможность отправлять сегментированные сообщения. В таких сообщениях в заголовке пользовательских данных помещается информация о номере сегмента сообщения и общем количестве сегментов. Максимальная длина сегмента при этом уменьшается за счет этого заголовка. Как правило, каждый сегмент тарифицируется как отдельное сообщение.", EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.CheckCost("79161234567", "Сообщение длинной 3 SMS:  Сегментирование поддерживают почти все современные телефоны, но часто в телефонах вводится ограничение на количество сегментов в сообщении.", EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод CheckBalance:");
            Console.WriteLine(smsProdiver.CheckBalance(EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.CheckBalance(EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.CheckBalance(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод CheckLimit:");
            Console.WriteLine(smsProdiver.CheckLimit(EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.CheckLimit(EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.CheckLimit(EnumAuthenticationTypes.StrongApi));
            Console.WriteLine("\nМетод CheckSenders:");
            Console.WriteLine(smsProdiver.CheckSenders(EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.CheckSenders(EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.CheckSenders(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод CheckAuth:");
            Console.WriteLine(smsProdiver.CheckAuth(EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.CheckAuth(EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.CheckAuth(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод StoplistAdd:");
            Console.WriteLine(smsProdiver.StoplistAdd("79161234567", "ignore", EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.StoplistAdd("79161234568", "ignore", EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.StoplistAdd("79161234569", "ignore", EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод StoplistGet:");
            Console.WriteLine(smsProdiver.StoplistGet(EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.StoplistGet(EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.StoplistGet(EnumAuthenticationTypes.StrongApi));

            Console.WriteLine("\nМетод StoplistDelete:");
            Console.WriteLine(smsProdiver.StoplistDelete("79161234567", EnumAuthenticationTypes.Simple));
            Console.WriteLine(smsProdiver.StoplistDelete("79161234569", EnumAuthenticationTypes.Strong));
            Console.WriteLine(smsProdiver.StoplistDelete("79161234568", EnumAuthenticationTypes.StrongApi));            

            Console.WriteLine("\nГотово.\n\n");

            Console.ReadLine();
        }
    }
}
