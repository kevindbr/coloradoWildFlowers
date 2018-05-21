using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class App : Application
    {
        public static WildFlowerRepository WildFlowerRepo { get; private set; }
        public static WildFlowerRepositoryLocal WildFlowerRepoLocal { get; set; }
        public static WildFlowerSettingRepository WildFlowerSettingsRepo { get; private set; }
        //public static WoodySearchRepository WoodySearchRepo { get; private set; }

        public App(ISQLitePlatform sqliteplatform, string dbPath)
        {
            InitializeComponent();

            SQLiteConnection newConn = new SQLiteConnection(sqliteplatform, dbPath, false);
            DBConnection dbConn = new DBConnection(newConn);

            SQLiteAsyncConnection newConnAsync = new SQLiteAsyncConnection(() => new SQLiteConnectionWithLock(sqliteplatform, new SQLiteConnectionString(dbPath, false)));
            DBConnection dbConnAsync = new DBConnection(newConnAsync);

            WildFlowerRepo = new WildFlowerRepository();
            WildFlowerSettingsRepo = new WildFlowerSettingRepository();
            WildFlowerRepoLocal = new WildFlowerRepositoryLocal(WildFlowerRepo.GetAllWildFlowers());


            this.MainPage = new NavigationPage(new MainPage());
        }
    }
}
