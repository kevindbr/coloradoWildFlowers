using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace PortableApp
{

    public class WildFlowerRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WildFlowerRepository()
		{
            //Create the Woody Plant table(only if it's not yet created) 
            //conn.DropTable<WildFlower>();
            conn.CreateTable<WildFlower>();
            //SeedDB();
        }

        public void ClearWildFlowers()
        {
            conn.DropTable<WildFlower>();
            conn.CreateTable<WildFlower>();
        }

        // return a list of WildFlowers saved to the WildFlower table in the database
        public List<WildFlower> GetAllWildFlowers()
        {
            return conn.GetAllWithChildren<WildFlower>();
        }

        // return a list of WildFlowers saved to the WildFlower table in the database
        public async Task<ObservableCollection<WildFlower>> GetAllWildFlowersAsync()
        {
            List<WildFlower> list = await connAsync.GetAllWithChildrenAsync<WildFlower>(); 
            return new ObservableCollection<WildFlower>(list);     
        }

        public List<string> GetPlantJumpList()
        {
            return GetAllWildFlowers().Select(x => x.genusSpeciesWeber.ToString()).Distinct().ToList();
        }

        // return a specificWildFlower given an id
        public WildFlower GetWildFlowerByAltId(int Id)
        {
            WildFlower plant = conn.Table<WildFlower>().Where(p => p.id.Equals(Id)).FirstOrDefault();
            return conn.GetWithChildren<WildFlower>(plant.plant_id);
        }

        public async Task AddOrUpdatePlantAsync(WildFlower plant)
        {
            try
            {
                if (string.IsNullOrEmpty(plant.commonName1))
                    throw new Exception("Valid plant required");

                await connAsync.RunInTransactionAsync((SQLite.Net.SQLiteConnection tran) =>
                {
                    tran.InsertOrReplaceWithChildren(plant);
                });

                //  await connAsync.InsertOrReplaceAllWithChildrenAsync(plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", plant, ex.Message);
            }

        }

        public async Task AddOrUpdateAllPlantsAsync(IList<WildFlower> plants)
        {
            try
            {
                // await connAsync.InsertOrReplaceWithChildrenAsync(plant);
                await connAsync.RunInTransactionAsync((SQLite.Net.SQLiteConnection tran) =>
                {
                    tran.InsertOrReplaceAllWithChildren(plants);
                });

            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", "plants", ex.Message);
            }

        }


        public async Task UpdatePlantAsync(WildFlower plant)
        {
            try
            {
                await connAsync.UpdateAsync(plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to update {0}. Error: {1}", plant, ex.Message);
            }
        }

        public List<WildFlower> GetFavoritePlants()
        {
            return GetAllWildFlowers().Where(p => p.isFavorite == true).ToList();
        }

        // get plants through term supplied in quick search
        public List<WildFlower> WoodyPlantsQuickSearch(string searchTerm)
        {
            return GetAllWildFlowers().Where(p => p.genusSpeciesWeber.ToLower().Contains(searchTerm.ToLower()) || p.commonName1.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

    }
}