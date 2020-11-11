using Acr.UserDialogs;
using Newtonsoft.Json;
using StraviaTecMovil.Helpers;
using StraviaTecMovil.Helpers.Network;
using StraviaTecMovil.Models;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StraviaTecMovil.ViewModels
{
    public class LoginViewModel
    {
        public LoginModel Model { get; set; }
        public Command LoginCommand { get; set; }
        private INavigation Nav { get; set; }
        private IUserDialogs Dialogs { get; set; }

        public LoginViewModel(INavigation nav)
        {
            Nav = nav;
            Model = new LoginModel();
            Dialogs = UserDialogs.Instance;
            LoginCommand = new Command(_ => OnLogin().ConfigureAwait(false));
        }

        public async Task OnLogin()
        {
            if (Model.Busy) return;

            if (Model.IsEmpty())
            {
                Dialogs.Alert(new AlertConfig
                {
                    Message = "Debes completar todos los campos",
                    OkText = "Aceptar"
                });
                return;
            }

            Model.Busy = true;
            Dialogs.ShowLoading("Cargando");

            var json = JsonConvert.SerializeObject(new
            {
                Model.User,
                Password = Model.Pass,
                UserType = "athlete"
            });

            var response = await Connection.Post(Urls.Login, json, 120)
                .ConfigureAwait(true);

            Dialogs.HideLoading();

            if (response.Succeeded)
            {
                //var loginData = response.ParseBody<Models.Api.LoginModel>();
                //var userType = loginData.Client ? "cliente" : "productor";

                Preferences.Set(Config.UserLogged, true, Config.SharedName);
                Preferences.Set(Config.UserId, Model.User, Config.SharedName);

                Messaging.Send(this, Constants.LoginDone);

            } else
            {
                Dialogs.Alert(new AlertConfig
                {
                    Message = $"Ocurrió un error al iniciar sesión - {response.Code}",
                    OkText = "Aceptar"
                });
            }

            Model.Busy = false;
        }
    }
}
