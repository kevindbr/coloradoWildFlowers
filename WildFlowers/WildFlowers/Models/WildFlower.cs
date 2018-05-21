using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("wildflowers")]
    public class WildFlower
    {
        [PrimaryKey]        
        public int plant_id { get; set; }

        [Unique]
        public int id { get; set; }
        
        public string familyScientific { get; set; }

        public string familyScientific2 { get; set; }

        public string familyCommon { get; set; }

        public string genusSpeciesWeber { get; set; }

        public string genusSpeciesAckerfield { get; set; }

        public string genusWeber { get; set; }

        public string speciesWeber { get; set; }

        public string genusAckerfield { get; set; }

        public string commonName1 { get; set; }

        public string commonName2 { get; set; }

        public string color { get; set; }

        public string month { get; set; }

        public string zone { get; set; }

        public string origin { get; set; }

        public string noxious { get; set; }

        public string description { get; set; }

        public string similar { get; set; }

        public string photos { get; set; }

        public string thumbnail { get; set; }

        public bool isFavorite { get; set; }

        public string genusSpeciesWeberFirstInitial { get { return genusSpeciesWeber[0].ToString(); } }
        public string familyScientificFirstInitial { get { return familyScientific[0].ToString(); } }
        public string commonName1FirstInitial { get { return commonName1[0].ToString(); } }


        public IFolder rootFolder { get { return FileSystem.Current.LocalStorage; } }

        //public IFolder rootFolder { get { return FileSystem.Current.LocalStorage; } }
        public string ThumbnailPathDownloaded { get { return rootFolder.Path + "/Images/" + genusSpeciesWeber + "_1.jpg"; } }
        public string ThumbnailPathStreamed
        {
            get { return "http://sdt1.agsci.colostate.edu/mobileapi/api/flower/image_name/" + genusSpeciesWeber + "_1"; }
        }


        public List<WildFlowerImage> Images
        {
            get
            {
                List<WildFlowerImage> images = new List<WildFlowerImage>();
                List<string> names = photos.Split(',').ToList<string>();
                foreach (string name in names)
                {
                    WildFlowerImage image = new WildFlowerImage(name.Trim(), rootFolder);
                    images.Add(image);
                }
                try
                {
                    return images;
                }
                catch (NullReferenceException e)
                {
                    return null;
                }

            }
        }


        public string RangePathDownloaded
        {
            get
            {
                try
                {
                    return rootFolder.Path + "/Images/map_" + genusSpeciesWeber + ".png";
                }
                catch (NullReferenceException e)
                {
                    return null;
                }
            }
        }

        public string RangePathStreamed
        {
            get
            {
                try
                {

                    return "http://sdt1.agsci.colostate.edu/mobileapi/api/flower/range_images/" + "map_" + genusSpeciesWeber;
                }
                catch (NullReferenceException e)
                {
                    return null;
                }
            }

        }


    }

}
