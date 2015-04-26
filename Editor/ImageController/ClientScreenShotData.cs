using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithImage
{
    public class ClientScreenShotData
    {
        public int widthResolution { set; get; }

        public int heightResolution { set; get; }

        public byte[] colorPackage { set; get; }

        public ClientScreenShotData() { }

        public ClientScreenShotData(int width, int height, byte[] bytes)
        {
            this.heightResolution = height;
            this.widthResolution = width;
            this.colorPackage = bytes;
        }
    }
}
