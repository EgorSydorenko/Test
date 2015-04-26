using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaverLoaderLib
{
    public class SaverLoader
    {
        private static readonly string baseFolder = "My Boards";
        private static string filePath;
        
        public static void Save(List<string> listOfStrings)
        {
            if (filePath == null)
            {
                SaveAs(listOfStrings);
                return;
            }
            FileStream fS = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            BinaryFormatter bF = new BinaryFormatter();
            bF.Serialize(fS, listOfStrings);
            fS.Close();
        }

        public static void SaveAs(List<string> listOfStrings)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "Файлы BRD(*.brd)|*.brd";
            SFD.AddExtension = true;
            SFD.DefaultExt = "Файлы BRD(*.brd)";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                filePath = SFD.FileName;
                FileStream fS = new FileStream(SFD.FileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter bF = new BinaryFormatter();
                bF.Serialize(fS, listOfStrings);
                fS.Close();
            }
        }

        public static void SaveByDefault(List<string> listOfStrings, string group = null)
        {
            FileStream fS;
            BinaryFormatter bF = new BinaryFormatter();

            if (filePath != null)
            {
                fS = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                bF.Serialize(fS, listOfStrings);
                fS.Close();
                return;
            }

            // Determine whether the directory exists.
            if (!Directory.Exists(baseFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(baseFolder);
            }

            DateTime time = DateTime.Now;
            filePath = String.Format(@"{5}\{0}_{1}_{2}_{3}_{4}.brd", time.Year, time.Month.ToString("00"), time.Hour.ToString("00"), time.Minute.ToString("00"), time.Second.ToString("00"), baseFolder);
            fS = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            bF.Serialize(fS, listOfStrings);
            fS.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>null, if file not choosen ( not selected )</returns>
        public static List<string> Load()
        {
            List<string> listOfStrings = null;
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowReadOnly = true;
            OFD.Filter = "Файлы BRD(*.brd)|*.brd";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                FileStream fS = new FileStream(OFD.FileName, FileMode.Open, FileAccess.Read);
                BinaryFormatter bF = new BinaryFormatter();
                listOfStrings = (List<string>)bF.Deserialize(fS);
                fS.Close();
                filePath = OFD.FileName;
            }
            return listOfStrings;
        }
    }
}