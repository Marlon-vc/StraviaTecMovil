﻿using StraviaTecMovil.Helpers;
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