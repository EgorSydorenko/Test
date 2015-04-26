using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WorkWithImage
{
    public static class ImageControler
    {
        public static byte[] GetScreenshotBytes()
        {
            Bitmap bmp = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.CopyFromScreen(0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            PixelFormat pxlFormat = PixelFormat.Format24bppRgb;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxlFormat);
            int numBytes = bmpData.Stride * bmp.Height;
            var IntPtr = bmpData.Scan0;
            byte[] rgbValues = new byte[numBytes];
            Marshal.Copy(IntPtr, rgbValues, 0, numBytes);
            byte[] newRgbValue = new byte[numBytes];

            for (int i = 0; i < rgbValues.Length; i++)
            {
                newRgbValue[i] = (byte)(rgbValues[i] & 240);
            }

            bmp.UnlockBits(bmpData);
            return newRgbValue;
        }

        public static byte[] GetCurrentSizeScreenShotBytes(int width, int height)
        {
            Bitmap bmpTest = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmpTest);
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.CopyFromScreen(0, 0, System.Windows.Forms.Screen.PrimaryScreen.Bounds.X, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            g.Dispose();
            Bitmap bmp = new Bitmap(bmpTest,new System.Drawing.Size(width,height));
            PixelFormat pxlFormat = PixelFormat.Format24bppRgb;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxlFormat);
            int numBytes = bmpData.Stride * bmp.Height;
            var IntPtr = bmpData.Scan0;
            byte[] rgbValues = new byte[numBytes];
            Marshal.Copy(IntPtr, rgbValues, 0, numBytes);
            byte[] newRgbValue = new byte[numBytes];

            for (int i = 0; i < rgbValues.Length; i++)
            {
                newRgbValue[i] = (byte)(rgbValues[i] & 240);
            }

            bmp.UnlockBits(bmpData);
            return newRgbValue;
        }


        public static class ImageCompressor
        {

            public static List<int> getChanges(int widthResolution, int heightResolution, int blockPixelWidth, int blockPixelHeight,
                int widthCountOfBlocks, int heightCountOfBlocks, int totalCountOfBlocks, byte[] newPictureBuffer, ClientScreenShotData screenShotData)
            {

                List<int> arr = new List<int>();
                int pixelInBlock = blockPixelHeight * blockPixelWidth * 3;
                int pixelInLine = pixelInBlock * widthCountOfBlocks;

                for (int block = 0; block < totalCountOfBlocks; block++)
                {
                    int blockFullLines = block / widthCountOfBlocks;
                    int blockInNotFullLine = block % widthCountOfBlocks;
                    int startByte = blockFullLines * pixelInLine + blockInNotFullLine * blockPixelWidth * 3;

                    for (int i = startByte; i < startByte + blockPixelHeight * widthResolution * 3 - 1; i += widthResolution * 3)
                    {
                        for (int j = 0; j < blockPixelWidth * 3; j++)
                        {
                            if ((screenShotData.colorPackage[i + j] ^ newPictureBuffer[i + j]) != 0)
                            {
                                arr.Add(block);
                                goto nextStep;
                            }
                        }
                    }
                nextStep: ;
                }
                return arr;
            }

            public static byte[] byteCompressor(List<int> blocks, byte[] bufferToCompress, int blockPixelWidth, int blockPixelHeight, int widthCountOfBlocks, int heightCountOfBlocks, int widthResolution, int heightResolution)
            {
                int pixelInBlock = blockPixelHeight * blockPixelWidth * 3;
                int pixelInLine = pixelInBlock * widthCountOfBlocks;
                byte[] compressedBuffer = new byte[(blocks.Count * pixelInBlock) / 2];
                int cnt = 0;

                for (int b = 0; b < blocks.Count; b++)
                {
                    int blockFullLines = blocks[b] / widthCountOfBlocks;
                    int blockInNotFullLine = blocks[b] % widthCountOfBlocks;
                    int startByte = blockFullLines * pixelInLine + blockInNotFullLine * blockPixelWidth * 3;

                    for (int i = startByte; i < startByte + blockPixelHeight * widthResolution * 3 - 1; i += widthResolution * 3)
                    {
                        for (int j = 0; j < blockPixelWidth * 3; j += 2)
                        {
                            compressedBuffer[cnt] = (byte)(bufferToCompress[i + j] | bufferToCompress[i + j + 1] >> 4);
                            cnt++;
                        }
                    }
                }
                return compressedBuffer;
            }
        }


        public static class ImageDeCompressor
        {
            public static BitmapSource CreateNewImage(ClientScreenShotData shot)
            {
                int bitsPerPixel = ((int)137224 & 0xff00) >> 8;
                int bytesPerPixel = (bitsPerPixel + 7) / 8;
                int stride = 4 * ((shot.widthResolution * bytesPerPixel + 3) / 4);
                BitmapSource img = BitmapSource.Create(shot.widthResolution, shot.heightResolution, 96, 96, System.Windows.Media.PixelFormats.Bgr24, null, shot.colorPackage, stride);
                return img;
            }

            public static void ChangeCurrentPictureNew(ClientScreenShotData data, byte[] fileBytes, List<int> blocks, int widthResolution, int heightResolution)
            {
                if (blocks == null)
                    return;
                System.Drawing.Size rectSize = Utils.GetRectangleSize(widthResolution, heightResolution);
                int countHorizontalBlocks = widthResolution / rectSize.Width;
                int countVerticalBlocks = heightResolution / rectSize.Height;
                int pixelInBlock = rectSize.Height * rectSize.Width * 3;
                int pixelInLine = pixelInBlock * countHorizontalBlocks;
                int totalBlockCount = countHorizontalBlocks * countVerticalBlocks;
                int cnt = 0;
                for (int b = 0; b < blocks.Count; b++)
                {
                    int blockFullLines = blocks[b] / countHorizontalBlocks;
                    int blockInNotFullLine = blocks[b] % countHorizontalBlocks;
                    int startByte = blockFullLines * pixelInLine + blockInNotFullLine * rectSize.Width * 3;

                    for (int i = startByte; i < startByte + rectSize.Height * widthResolution * 3 - 1; i += widthResolution * 3)
                    {
                        for (int j = 0; j < rectSize.Width * 3; j++)
                        {
                            try
                            {
                                data.colorPackage[i + j] = fileBytes[cnt];
                                cnt++;
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                }
            }       
        }

    }
}
