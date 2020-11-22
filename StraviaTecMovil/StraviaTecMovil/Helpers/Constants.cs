using System;
using System.Collections.Generic;
using System.Text;

namespace StraviaTecMovil.Helpers
{
    public static class Constants
    {
        public static string LoginDone => "LOGIN_COMPLETED";
    }

    public static class Config
    {
        public static string SharedName => "StraviaTecApp";
        public static string UserLogged => "USER_LOGGED_IN";
        public static string UserType => "USER_TYPE";
        public static string UserId => "USER_ID";
    }

    public static class Urls
    {
        public static string Usuarios => "http://192.168.0.16:5001/api/Usuarios";
        public static string Login => "http://192.168.0.16:5001/api/Login";
        public static string InfoEvento => "http://192.168.0.16:5001/api/InfoEvento";
        public static string Actividades => "http://192.168.0.16:5001/api/Actividades";
    }
}