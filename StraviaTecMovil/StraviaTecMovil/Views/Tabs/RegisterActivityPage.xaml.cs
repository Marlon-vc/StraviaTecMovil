using StraviaTecMovil.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraviaTecMovil.Views.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterActivityPage : ContentPage
    {
        public RegisterActivityPage()
        {
            InitializeComponent();

            BindingContext = new RegisterActivityViewModel(Navigation, viewMap);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var context = ((RegisterActivityViewModel)BindingContext);
            context.CheckLocation()
                .ConfigureAwait(false);
            //context.StartListening()
            //    .ConfigureAwait(false);
        }
    }
}