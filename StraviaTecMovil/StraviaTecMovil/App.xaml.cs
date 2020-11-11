using Microsoft.EntityFrameworkCore.Storage;
using StraviaTecMovil.Helpers;
using StraviaTecMovil.Models;
using StraviaTecMovil.Services.SQLite;
using StraviaTecMovil.ViewModels;
using StraviaTecMovil.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraviaTecMovil
{
    public partial class App : Application
    {
        static SqliteService database;

        public static SqliteService SqliteCon
        {
            get
            {
                if (database == null)
                {
                    try
                    {
                        database = new SqliteService();
                    } catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }

                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = LoggedIn() ? (Page)new MainPage() : new NavigationPage(new LoginPage());

            Messaging.Subscribe<LoginViewModel>(this, Constants.LoginDone, OnLoginDone);
        }

        private void OnLoginDone(LoginViewModel sender)
        {
            MainPage = new MainPage();
        }

        private bool LoggedIn()
        {
            return Preferences.Get(Config.UserLogged, false, Config.SharedName);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
