using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace PortableApp
{
    public class ExternalDBConnection
    {
        // Declare variables
        public string Url = "http://sdt1.agsci.colostate.edu/mobileapi/api/flower";
        HttpClient client = new HttpClient();
        private string result;
        private Stream resultStream;

        // Set headers for client
        public ExternalDBConnection()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "p4OqMiplghVdWPbVv5rx84jdlskdJk*jdlsKDIE84");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IEnumerable<WildFlower>> GetAllPlants()
        {
            result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IList<WildFlower>>(result);
        }

        public async Task<WildFlowerSetting> GetDateUpdatedDataOnServer()
        {
            try
            {
                result = await client.GetStringAsync(Url + "_settings/DatePlantDataUpdatedOnServer");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error", e.Message);
                return null;
            }
            return JsonConvert.DeserializeObject<WildFlowerSetting>(result);
        }

        //hardcoded
        public async Task<IEnumerable<WildFlowerSetting>> GetImageZipFileSettings()
        {
            result = await client.GetStringAsync(Url + "_settings/ImagesZipFile");
            WildFlowerSetting setting = JsonConvert.DeserializeObject<WildFlowerSetting>(result);

            List<WildFlowerSetting> settingList = new List<WildFlowerSetting>();

            settingList.Add(setting);

            return settingList;
        }


        public async Task<Stream> GetImageZipFiles(string imageFileToDownload)
        {
            resultStream = await client.GetStreamAsync(Url + "/image_zip_files/" + imageFileToDownload);
            return resultStream;
        }
    }
}
