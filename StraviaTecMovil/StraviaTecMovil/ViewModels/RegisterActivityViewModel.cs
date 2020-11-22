using Acr.UserDialogs;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using StraviaTecMovil.Helpers;
using StraviaTecMovil.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace StraviaTecMovil.ViewModels
{
    public class RegisterActivityViewModel
    {
        private INavigation _nav;
        private IUserDialogs dialogs;
        private ActivityTimer Activity;
        private IGeolocator locator;
        private Xamarin.Forms.Maps.Map viewMap;
        public RecordActivityModel Model { get; set; }
        public Command CurrentActionCommand { get; set; }
        public Command FinishActivityCommand { get; set; }

        public RegisterActivityViewModel(INavigation nav, Xamarin.Forms.Maps.Map map)
        {
            _nav = nav;
            viewMap = map;
            dialogs = UserDialogs.Instance;
            Model = new RecordActivityModel();
            locator = CrossGeolocator.Current;
            Activity = new ActivityTimer(OnTimerTick, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
            LoadCommands();
        }

        public async Task StartListening()
        {
            if (locator.IsListening)
                return;

            locator.PositionChanged += Locator_PositionChanged;
            await locator.StartListeningAsync(TimeSpan.FromSeconds(3), 5);
        }

        public async Task StopListening()
        {
            if (!locator.IsListening)
                return;

            await locator.StopListeningAsync();
        }

        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            var position = e.Position;
            dialogs.Toast($"{position.Latitude} | {position.Longitude}", TimeSpan.FromSeconds(1));
            
            var location = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(100));

            var lastPos = Model.Route.LastOrDefault();
            if (lastPos == null || (lastPos != null && Distance.BetweenPositions(location, lastPos).Meters > 5))
            {
                Model.AddPosition(location);
                viewMap.MapElements.Clear();
                var route = new Polyline()
                {
                    StrokeColor = Color.FromHex("#fc6203"),
                    StrokeWidth = 10
                };

                foreach (var pos in Model.Route)
                {
                    route.Geopath.Add(pos);
                }
                viewMap.MapElements.Add(route);
            }

            viewMap.MoveToRegion(mapSpan);
        }

        private void LoadCommands()
        {
            CurrentActionCommand = new Command(CurrentActionPressed);
            FinishActivityCommand = new Command(FinishActivityTracking);
        }

        private void CurrentActionPressed()
        {
            if (!Activity.OnActivity())
            {
                Speak("Let's start")
                    .ConfigureAwait(false);
                StartListening()
                    .ConfigureAwait(false);
                Activity.Start();
                Model.OnActivity = true;
                Model.CurrentColspan = 1;
                Model.CurrentAction = "Pausar";
                return;
            }

            Activity.Toggle();
            if (Activity.IsRunning())
            {
                Model.CurrentAction = "Pausar";
                Speak("Resumed")
                    .ConfigureAwait(false);
            } else
            {
                Model.CurrentAction = "Resumir";
                Speak("Paused")
                    .ConfigureAwait(false);
            }

        }

        public async Task CheckLocation()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return;

            await dialogs.AlertAsync(new AlertConfig
            {
                OkText = "Aceptar",
                Title = "Permisos",
                Message = "Se necesita acceder a la ubicación del dispositivo para poder monitorear la ruta de la actividad."
            });
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            viewMap.IsShowingUser = true;
            viewMap.MoveToLastRegionOnLayoutChange = false;
        } 

        private async Task Speak(string sentence)
        {
            await TextToSpeech.SpeakAsync(sentence);
        }

        private void FinishActivityTracking()
        {
            Speak("Activity finished")
                .ConfigureAwait(false);
            StopListening()
                .ConfigureAwait(false);
            Activity.Stop();
            Model.OnActivity = false;
            Model.CurrentColspan = 2;
            Model.CurrentAction = "Iniciar";
            Model.ElapsedTime = "00:00";
            dialogs.Alert(new AlertConfig
            {
                OkText = "Aceptar",
                Title = "Resumen de actividad",
                Message = $"Duración: {FormatTime(Activity.GetTotalActivityTime())}",
            });
        }

        private void OnTimerTick(object o)
        {
            var elapsed = Activity.GetElapsedTime();
            Model.ElapsedTime = FormatTime(elapsed);
        }

        private string FormatTime(TimeSpan time)
        {
            return string.Format("{0:D2}:{1:D2}", (int)time.TotalMinutes, time.Seconds);
        }
    }
}
