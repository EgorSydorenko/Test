using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithImage
{
    public static class Utils
    {
        public static List<int> GetRectNumbs(string protocol)
        {
            var curProtocol = protocol.Substring(1, protocol.Length - 2).Split(',');
            List<int> numbs = new List<int>();
            for (int i = 0; i < curProtocol.Count(); i++)
            {
                numbs.Add(Int32.Parse(curProtocol[i]));
            }
            return numbs;
        }

        public static System.Drawing.Size GetRectangleSize(int width, int height)
        {
            int sector_width = 0;
            int sector_height = 0;

            for (int i = 3; i < width; i++)
            {
                for (int j = 3; j < height; j++)
                {
                    if (Math.Abs((i + 1) * (j + 1)) > 600 && Math.Abs((i + 1) * (j + 1)) < 2000 && width % (i + 1) == 0 && height % (j + 1) == 0)
                    {
                        sector_height = j + 1;
                        sector_width = i + 1;
                        goto Out;
                    }
                }
            }

        Out:
            return new System.Drawing.Size((int)(sector_width), (int)(sector_height));

        }

        public static byte[] UnPacking(byte[] bts)
        {
            bts = Decompress(bts);
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < bts.Length; i++)
            {
                bytes.Add((byte)(bts[i] & 240));
                bytes.Add((byte)((bts[i] & 15) << 4));
            }
            return bytes.ToArray();
        }

        public static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 32768;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static List<byte> Packcage(byte[] rgbValues)
        {
            List<byte> package = new List<byte>();
            for (int i = 0; i < rgbValues.Length; i += 2)
            {
                byte redGreen = (byte)((rgbValues[i]) + (rgbValues[i + 1] >> 4));
                package.Add(redGreen);
            }
            return Compress(package.ToArray()).ToList();
        }

        public static byte[] GetArraysProtocol(List<int> blocks, int width, int height, int byteLength)
        {
            StringBuilder protocol = new StringBuilder();
            protocol.AppendFormat("{3}x{0}x{1}x{2}x[", width, height, byteLength, "ScreenShot");
            int counter = 0;
            while (counter < blocks.Count)
            {
                if (counter == 0)
                    protocol.Append(blocks[counter]);
                else
                    protocol.AppendFormat(",{0}", blocks[counter]);
                counter++;
            }
            protocol.Append("]|");
            return Encoding.Unicode.GetBytes(protocol.ToString());
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
