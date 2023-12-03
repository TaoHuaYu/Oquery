using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace PostgreDB
{
    public enum TextEncode
    {
        ASCII = 0,
        UTF7 = 1,
        UTF8 = 2,
        UTF32 = 3,
        Unicode = 4,
        BigEndianUnicode = 5,
        Default = 6
    }

    public class TextEngine
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <param name="Section"></param>
        /// Section name
        /// <param name="Key"></param>
        /// Key Name
        /// <param name="Value"></param>
        /// Value Name
        /// <param name="FullINIFilename"></param>
        /// Full INI Filename
        public static void WriteINIValue(string Section, string Key, string Value, string FullINIFilename)
        {
            WritePrivateProfileString(Section, Key, Value, FullINIFilename);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="FullINIFilename"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string ReadINIValue(string Section, string Key, string FullINIFilename, int Length = 255)
        {
            StringBuilder temp = new StringBuilder(Length);
            int i = GetPrivateProfileString(Section, Key, "", temp, Length, FullINIFilename);
            return temp.ToString();
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }

        public static int WriteBinaryFile(string SaveFileName, byte[] InData)
        {
            try
            {
                //開啟建立檔案
                FileStream myFile = File.Open(SaveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryWriter myWriter = new BinaryWriter(myFile);
                myWriter.Write(InData);
                myWriter.Close();
                myWriter.Dispose();
                //myWriter.Flush();
                myFile.Close();
                myFile.Dispose();
                //myFile.Flush();
                return 1;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public static int BinRead(string OpenFileName, ref byte[] InData)
        {
            try
            {
                //開啟檔案
                FileStream myFile = File.Open(OpenFileName, FileMode.Open, FileAccess.ReadWrite);
                //引用myReader類別
                BinaryReader myReader = new BinaryReader(myFile);
                int dl = System.Convert.ToInt32(myFile.Length);
                //讀取位元陣列
                InData = myReader.ReadBytes(dl);
                //讀取資料
                //釋放資源
                myReader.Close();
                myReader.Dispose();
                myFile.Close();
                myFile.Flush();
                myFile.Dispose();
                return 1;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        #region 將內容寫入到指定的檔案
        /// <summary>
        /// 將內容寫入到指定的檔案
        /// </summary>
        /// <param name="FileContent">要寫入的檔案內容</param>
        /// <param name="FileName">檔案名稱</param>
        /// <param name="EncodeMothed">文字編碼方式</param>
        public static void WriteContentToFile(string FileContent, string FileName, TextEncode EncodeMothed, FileMode FM = FileMode.OpenOrCreate, FileAccess FA = FileAccess.Write)
        {
            //FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
            FileStream fs = new FileStream(FileName, FM, FA);
            StreamWriter sw;

            Encoding ee;//Encoding ee = Encoding.ASCII;

            switch ((int)EncodeMothed)
            {
                case 0:
                    //sw = new StreamWriter(fs, Encoding.ASCII);
                    ee = Encoding.ASCII;
                    break;
                case 1:
                    ee = Encoding.UTF7;
                    break;
                case 2:
                    ee = Encoding.UTF8;
                    break;
                case 3:
                    ee = Encoding.UTF32;
                    break;
                case 4:
                    ee = Encoding.Unicode;
                    break;
                case 5:
                    ee = Encoding.BigEndianUnicode;
                    break;
                default:
                    ee = Encoding.Default;
                    break;
            }

            sw = new StreamWriter(fs, ee);

            sw.WriteLine(FileContent);
            sw.Close();
        }
        #endregion
    }
}
