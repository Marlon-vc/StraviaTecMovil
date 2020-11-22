using StraviaTecMovil.Views.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraviaTecMovil.Views
{
    public partial class HomePage : TabbedPage
    {
        public HomePage()
        {
            Children.Add(new ActivitiesPage());
            Children.Add(new NavigationPage(new RegisterActivityPage())
            {
                IconImageSource = "track.png",
                Title = "Registrar"
            });
            Children.Add(new ProfilePage());

            InitializeComponent();
        }
    }
}