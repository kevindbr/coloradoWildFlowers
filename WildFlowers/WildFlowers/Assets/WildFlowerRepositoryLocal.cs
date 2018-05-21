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
    public class WildFlowerRepositoryLocal
    {

        public string StatusMessage { get; set; }
        private List<WildFlower> allWildFlowers;
        private List<WildFlower> searchPlants;

        public WildFlowerRepositoryLocal(List<WildFlower> allPlantsDB)
        {
            allWildFlowers = allPlantsDB;

        }

        // return a list of WildFlowers saved to the WildFlower table in the database
        public List<WildFlower> GetAllWildFlowers()
        {
            return allWildFlowers;
        }

        public void ClearWildFlowersLocal()
        {
            allWildFlowers = new List<WildFlower>();
            searchPlants = new List<WildFlower>();
        }

        // return a list of WildFlowers saved to the WildFlower table in the database
        public async Task<ObservableCollection<WildFlower>> GetAllSearchPlants()
        {
            return new ObservableCollection<WildFlower>(searchPlants);
        }

        public void setSearchPlants(List<WildFlower> searchPlants)
        {
            this.searchPlants = searchPlants;
        }

        public List<string> GetPlantJumpList()
        {
            return allWildFlowers.Select(x => x.genusSpeciesWeber.ToString()).Distinct().ToList();
        }

        // return a specific WildFlower given an id
        public WildFlower GetWildFlowerByAltId(int Id)
        {
            IEnumerable<WildFlower> plants = allWildFlowers.Where(p => p.plant_id.Equals(Id));

            if (plants != null)
                return plants.First();

            else return null;
        }

        // get plants marked as favorites
        public List<WildFlower> GetFavoritePlants()
        {
            return allWildFlowers.Where(p => p.isFavorite == true).ToList();
        }

        // get plants through term supplied in quick search
        public List<WildFlower> WildFlowersQuickSearch(string searchTerm)
        {
            return allWildFlowers.Where(p => p.genusSpeciesWeber.ToLower().Contains(searchTerm.ToLower()) || p.commonName1.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

    }
}
