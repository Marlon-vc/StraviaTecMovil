using Acr.UserDialogs;
using StraviaTecMovil.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Xml;
using Plugin.FilePicker;
using System.IO;
using StraviaTecMovil.Helpers;
using StraviaTecMovil.Services.SQLite;
using StraviaTecMovil.Helpers.Network;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace StraviaTecMovil.ViewModels
{
    public class ActivitiesViewModel
    {
        private IUserDialogs _dialogs;

        public ActivitiesModel Model { get; set; }
        public Command OnRefreshLocalCommand { get; set; }
        public Command OnRefreshAllCommand { get; set; }
        public Command OnAddActivityCommand { get; set; }

        public ActivitiesViewModel()
        {
            _dialogs = UserDialogs.Instance;
            Model = new ActivitiesModel();
            LoadCommands();
        }

        private void LoadCommands()
        {
            OnRefreshLocalCommand = new Command(_ => OnRefreshLocal().ConfigureAwait(false));
            OnRefreshAllCommand = new Command(_ => OnRefreshAll().ConfigureAwait(false));
            OnAddActivityCommand = new Command(_ => OnAddActivity().ConfigureAwait(false));
        }

        private async Task OnRefreshLocal()
        {
            await Task.Delay(3000);
            _dialogs.Toast("Actividades locales actualizadas");

            Model.IsRefreshingLocal = false;
        }

        private async Task OnRefreshAll()
        {
            await Task.Delay(3000);
            _dialogs.Toast("Todas las actividades actualizadas");

            Model.IsRefreshingAll = false;
        }

        private async Task OnAddActivity()
        {
            var fileData = await CrossFilePicker.Current.PickFile();

            await Task.Delay(TimeSpan.FromSeconds(1));

            if (fileData == null || !fileData.FileName.EndsWith("gpx"))
            {
                return;
            }

            var result = await _dialogs.PromptAsync(new PromptConfig
            {
                Title = "Agregar actividad",
                Message = "Ingresa el nombre de la actividad",
                OkText = "Aceptar"
            });

            var dbConnection = App.SqliteCon;
            var document = new XmlDocument();
            var base64Encoded = Convert.ToBase64String(fileData.DataArray); ;
            
            using (var stream = new MemoryStream(fileData.DataArray))
            {
                document.Load(stream);
            }

            var recorrido = new Recorrido()
            {
                Nombre = result.Value,
                Fecha_hora = GpxParser.GetDateTime()
            };
            int idRecorrido = await dbConnection.InsertRecorridoAsync(recorrido);

            GpxParser.SetCurrentDoc(document);
            var points = GpxParser.ParseDoc(idRecorrido);

            foreach (var punto in points)
            {
                dbConnection.InsertPunto(punto);
            }

            var json = JsonConvert.SerializeObject(new
            {
                User = Preferences.Get(Config.UserId, "mvega", Config.SharedName),
                Nombre = result.Value,
                IdTipoActividad = 1,
                Recorrido = base64Encoded,
                Fecha = GpxParser.GetDateTime(),
                Duracion = GpxParser.GetTotalTime(points),
                Kilometros = GpxParser.PointsToDistanceInKm(points),
                EsEvento = false
            });

            var response = await Connection.Post(Urls.Actividades, json, 120);

            if (!response.Succeeded)
            {
                _dialogs.Alert("Ocurrió un error al sincronizar datos");
            }

            _dialogs.Toast("Actividad sincronizada correctamente");
        }

        private async Task<List<TipoActividad>> GetTiposActividadAsync()
        {
            var response = await Connection.Get($"{Urls.InfoEvento}/tipos")
                .ConfigureAwait(false);

            if (response.Succeeded)
            {
                return response.ParseBody<List<TipoActividad>>();
            }
            return null;
        }

    }
}
