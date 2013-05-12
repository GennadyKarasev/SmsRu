using SmsRu.Enumerations;
using System;

namespace SmsRu
{
    public interface ISmsProvider
    {
        String Send(String from, String[] to, String text, EnumAuthenticationTypes authType);
        String Send(String from, String[] to, String text, DateTime dateTime, EnumAuthenticationTypes authType);
        ResponseOnSendRequest SendByEmail (String[] to, String text);
        ResponseOnStatusRequest CheckStatus (String id, EnumAuthenticationTypes authType);
        String CheckCost(String to, String text, EnumAuthenticationTypes authType);
        String CheckBalance(EnumAuthenticationTypes authType);
        String CheckLimit(EnumAuthenticationTypes authType);
        String CheckSenders(EnumAuthenticationTypes authType);
        ResponseOnAuthRequest CheckAuth(EnumAuthenticationTypes authType);
        String GetToken();
        Boolean StoplistAdd(String phone, String text, EnumAuthenticationTypes authType);
        Boolean StoplistDelete(String phone, EnumAuthenticationTypes authType);
        String StoplistGet(EnumAuthenticationTypes authType);
    }
}
