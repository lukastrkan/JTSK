using JTSK.Views;
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
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
