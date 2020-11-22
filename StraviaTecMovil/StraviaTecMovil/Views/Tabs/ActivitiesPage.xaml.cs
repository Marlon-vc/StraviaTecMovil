using StraviaTecMovil.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraviaTecMovil.Views.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivitiesPage : TabbedPage
    {
        public ActivitiesPage()
        {
            InitializeComponent();
            BindingContext = new ActivitiesViewModel();
        }
    }
}