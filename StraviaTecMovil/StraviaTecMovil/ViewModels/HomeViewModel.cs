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
        public HomeModel Model { get; set; }

        private INavigation _nav;

        public HomeViewModel(INavigation nav)
        {
            _nav = nav;
            Model = new HomeModel();

            //TODO: Verificar tipo de usuario.

            LoadMenu().ConfigureAwait(false);
        }

        public void OnItemTapped(Models.MenuItem item)
        {
            if (item.Target == "logout")
            {
                //_nav.PushAsync(new LoginPage(), true);
                return;
            }

            var page = Type.GetType(item.Target);
            if (page == null) return;
            
            var instance = (Page)Activator.CreateInstance(page);
            if (instance == null) return;

            _nav.PushAsync(instance, true);
        }

        private async Task LoadMenu()
        {
            string menuToLoad = "StraviaTecMovil.Resources.MenuUsuario.json";

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(menuToLoad);
            
            if (stream == null)
                return;

            using (var reader = new StreamReader(stream))
            {
                var json = await reader.ReadToEndAsync();
                var menu = JsonConvert.DeserializeObject<List<Models.MenuItem>>(json);

                Model.AddItems(menu);
            }
        }
    }
}
