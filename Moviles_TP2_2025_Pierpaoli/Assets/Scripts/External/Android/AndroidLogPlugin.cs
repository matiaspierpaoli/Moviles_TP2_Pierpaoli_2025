using UnityEngine;

namespace Game.External
{
    public static class AndroidLogPlugin
    {
        const string JavaClassName = "com.pierpaoli.unity.LogBridge";
        static AndroidJavaClass _cls;
        static AndroidJavaClass Cls => _cls ??= new AndroidJavaClass(JavaClassName);

        public static void Send(string m,string st,string t)
        { if (Application.platform==RuntimePlatform.Android) Cls.CallStatic("sendLog", m??"", st??"", t??"Log"); }

        public static string GetAll()
        { return Application.platform==RuntimePlatform.Android ? Cls.CallStatic<string>("getAllLogs") ?? "" : ""; }

        public static void ConfirmAndDelete(string go,string cb)
        { if (Application.platform==RuntimePlatform.Android) Cls.CallStatic("confirmAndDelete", go, cb); }
    }
}