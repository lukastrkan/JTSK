using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JTSK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        bool _setup;
        public SettingsPage(bool setup = false)
        {
            InitializeComponent();
            _setup = setup;     
            if(Application.Current.Properties.ContainsKey("email"))
            {
                emailEntry.Text = (string)Application.Current.Properties["email"];
            }
            if (Application.Current.Properties.ContainsKey("url"))
            {
                urlEntry.Text = (string)Application.Current.Properties["url"];
            }
        }

        private async void save_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["email"] = emailEntry.Text;
            if (!urlEntry.Text.Contains("http://"))
            {
                Application.Current.Properties["url"] = "http://" + urlEntry.Text;
            }
            else
            {
                Application.Current.Properties["url"] = urlEntry.Text;
            }
            await Application.Current.SavePropertiesAsync();
            UserDialogs.Instance.Toast("Uloženo!");
            if(_setup)
            {
                await Navigation.PopModalAsync();
            }           
        }
    }
}
