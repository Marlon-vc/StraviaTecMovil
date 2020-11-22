using Newtonsoft.Json;
using StraviaTecMovil.Helpers;
using StraviaTecMovil.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StraviaTecMovil.ViewModels
{
    public class HomeViewModel
    {
        public RecordActivityModel Model { get; set; }

        private INavigation _nav;

        public HomeViewModel(INavigation nav)
        {
            Model = new RecordActivityModel();
            _nav = nav;
        }

    }
}
