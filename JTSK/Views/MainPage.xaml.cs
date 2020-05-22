using Acr.UserDialogs;
using JTSK.Models;
using JTSK.Services;
using Microsoft.AppCenter.Crashes;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tiny.RestClient;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JTSK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        readonly ConfigService _configService;
        ObservableCollection<Coordinate> souradnice = new ObservableCollection<Coordinate>();
        public MainPage()
        {
            InitializeComponent();
            _configService = DependencyService.Resolve<ConfigService>();
            if (!_configService.Exists)
            {
                Navigation.PushModalAsync(new NavigationPage(new SettingsPage(true)));
            }
            SouradniceView.ItemsSource = souradnice;
            Load();
        }

        async void Load()
        {
            await LoadTask();
        }

        async Task LoadTask()
        {
            var cord = await Task.Run(() => App._context.Coordinates.OrderBy(x => x.Id).ToList());
            if (cord.Count > 0)
            {
                foreach (var c in cord)
                {
                    souradnice.Add(c);
                }
            }
        }

        //Přidat
        async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var t = await DisplayPromptAsync("Zadejte název", "", "Ok", "Zrušít");
            if (String.IsNullOrEmpty(t) || String.IsNullOrWhiteSpace(t))
            {
                return;
            }
            UserDialogs.Instance.ShowLoading("Ukládám...");
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var s = new Coordinate(location.Latitude, location.Longitude, t);
                    App._context.Coordinates.Add(s);
                    await App._context.SaveChangesAsync();
                    souradnice.Add(s);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                UserDialogs.Instance.Toast("Někde se stala chyba");
            }
            UserDialogs.Instance.HideLoading();
        }

        public async Task Clear()
        {
            await Task.Run(() => souradnice.Clear());
            await LoadTask();
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            await Clear();
        }

        private async Task Delete()
        {
            if (await DisplayAlert("Smazat?", "Opravdu chcete smazat uložená data?", "Ano", "Ne"))
            {
                await App._context.Database.ExecuteSqlRawAsync("delete from Coordinates");
                await App._context.Database.ExecuteSqlRawAsync("VACUUM");
                souradnice.Clear();
            }
        }

        async Task Convert()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Převádím...");
                var cords = App._context.Coordinates.OrderBy(x => x.Id).Select(x => new { x.Display, x.Latitude, x.Longitude }).ToList();
                string json = JsonConvert.SerializeObject(cords);
                var client = new TinyRestClient(new HttpClient(), _configService.Config.Url);
                var response = await client.PostRequest("api.php")
                    .AddFormParameter("gps", json)
                    .AddFormParameter("email", _configService.Config.Email)
                    .ExecuteAsHttpResponseMessageAsync();
                UserDialogs.Instance.HideLoading();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    UserDialogs.Instance.Toast("Převedené souřadnice byly zaslány na email");
                }
                else
                {
                    UserDialogs.Instance.Toast("Chyba");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Toast("Chyba");
            }

        }

        private async void convert_Clicked(object sender, EventArgs e)
        {
            await Convert();
        }

        private async void delete_Clicked(object sender, EventArgs e)
        {
            await Delete();
        }

        private async void SouradniceView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await DeleteItem(e);
        }

        async Task DeleteItem(ItemTappedEventArgs e)
        {
            if (await DisplayAlert("Smazat?", "Opravdu chcete smazat vybraný záznam?", "Ano", "Ne"))
            {
                souradnice.Remove(souradnice[souradnice.IndexOf((Coordinate)e.Item)]);
                App._context.Coordinates.Remove((Coordinate)e.Item);
                await App._context.SaveChangesAsync();
            }
        }

        private async void settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}