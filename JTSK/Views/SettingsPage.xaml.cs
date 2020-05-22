using Acr.UserDialogs;
using JTSK.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JTSK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        bool _setup;
        readonly ConfigService _configService;
        public SettingsPage(bool setup = false)
        {
            InitializeComponent();
            _configService = DependencyService.Resolve<ConfigService>();
            _setup = setup;
            if (_configService.Exists)
            {
                emailEntry.Text = _configService.Config.Email;
                urlEntry.Text = _configService.Config.Url;
            }
        }

        private async void save_Clicked(object sender, EventArgs e)
        {
            await _configService.Save(emailEntry.Text, urlEntry.Text);
            UserDialogs.Instance.Toast("Uloženo!");
            if (_setup)
            {
                await Navigation.PopModalAsync();
            }
        }
    }
}
