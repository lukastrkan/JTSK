using JTSK.Services;
using JTSK.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace JTSK
{
    public partial class App : Application
    {
        public static ApplicationDbContext _context;
        public App()
        {
            _context = new ApplicationDbContext(Path.Combine(FileSystem.AppDataDirectory, "jtsk.db"));
            _context.Database.EnsureCreated();
            DependencyService.RegisterSingleton(_context);
            DependencyService.RegisterSingleton(new ConfigService());
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=f5e9e88c-5220-4862-a426-f58d0b2e2d1f;", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
