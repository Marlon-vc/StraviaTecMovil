using StraviaTecMovil.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StraviaTecMovil.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            Detail = new NavigationPage(new HomePage());
            BindingContext = new HomeViewModel(Detail.Navigation);
        }

        private void ListView_ItemTapped(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Models.MenuItem item))
                return;

            IsPresented = false;
            ((ListView)sender).SelectedItem = null;

            ((HomeViewModel)BindingContext).OnItemTapped(item);
        }
    }
}
