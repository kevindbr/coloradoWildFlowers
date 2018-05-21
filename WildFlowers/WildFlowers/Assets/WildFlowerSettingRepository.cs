using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortableApp.Models;

namespace PortableApp
{
    public class WildFlowerSettingRepository : DBConnection
    {

        public string StatusMessage { get; set; }

        public WildFlowerSettingRepository()
        {
            // Create the Woody Setting table (only if it's not yet created)
            //conn.DropTable<WildFlowerSetting>();
            conn.CreateTable<WildFlowerSetting>();
            SeedDB();
        }

        public void ClearWildFlowerSettings()
        {
            conn.DropTable<WildFlowerSetting>();
            conn.CreateTable<WildFlowerSetting>();
            SeedDB();
        }

        // return a list of WildFlowers saved to the WildFlowerSetting table in the database
        public List<WildFlowerSetting> GetAllWildFlowerSettings()
        {
            return (from s in conn.Table<WildFlowerSetting>() select s).ToList();
        }

        // get a list of image settings stored in the local database
        public List<WildFlowerSetting> GetAllImageSettings()
        {
            return conn.Table<WildFlowerSetting>().Where(s => s.name.Equals("ImagesZipFile")).ToList();
        }

        // get an individual setting based on its name
        public WildFlowerSetting GetSetting(string settingName)
        {
            return conn.Table<WildFlowerSetting>().FirstOrDefault(s => s.name.Equals(settingName));
        }

        // (async) get an individual setting based on its name
        public async Task<WildFlowerSetting> GetSettingAsync(string settingName)
        {
            return await connAsync.Table<WildFlowerSetting>().Where(s => s.name.Equals(settingName)).FirstOrDefaultAsync();
        }

        public WildFlowerSetting GetImageZipFileSetting(string fileName)
        {
            return conn.Table<WildFlowerSetting>().Where(s => s.valuetext.Equals(fileName)).FirstOrDefault();
        }

        // add a setting
        public void AddSetting(WildFlowerSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = conn.Insert(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add a setting async
        public async Task AddSettingAsync(WildFlowerSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = await connAsync.InsertAsync(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add or update a setting
        public void AddOrUpdateSetting(WildFlowerSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = conn.InsertOrReplace(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add or update a setting
        public async Task AddOrUpdateSettingAsync(WildFlowerSetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = await connAsync.InsertOrReplaceAsync(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // Seed database with essential settings
        public void SeedDB()
        {
            if (GetSetting("Sort Field") == null)
                conn.Insert(new WildFlowerSetting { name = "Sort Field", valuetext = "Scientific Name", valueint = 0 });
            if (GetSetting("Download Images") == null)
                conn.Insert(new WildFlowerSetting { name = "Download Images", valuebool = true });
            if (GetSetting("Date Plants Downloaded") == null)
                conn.Insert(new WildFlowerSetting { name = "Date Plants Downloaded", valuetimestamp = null });
        }

    }
}
