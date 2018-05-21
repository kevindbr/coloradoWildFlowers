using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;

namespace PortableApp.Models
{
    public class WildFlowerImage
    {
        public string imageName { get; set; }
        public string ImagePathStreamed { get; set; }
        public string ImagePathDownloaded { get; set; }

        public WildFlowerImage(string imageName, IFolder rootFolder)
        {
            this.imageName = imageName;

            ImagePathDownloaded = rootFolder.Path + "/Images/" + imageName + ".jpg";

            ImagePathStreamed = "http://sdt1.agsci.colostate.edu/mobileapi/api/flower/image_name/" + imageName;
        }

    }
}
