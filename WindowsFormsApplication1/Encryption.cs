using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Encrypter
{
    class Encryption
    {
        public void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            EncryptorWindow encryptorWindow = new EncryptorWindow();

            try
            {
                FileStream fileStreamInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
                FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(sKey),
                    IV = Encoding.ASCII.GetBytes(sKey),
                };

                ICryptoTransform desencrypt = des.CreateEncryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
                byte[] bytearrayinput = new byte[fileStreamInput.Length - 1];
                fileStreamInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Close();
                fileStreamInput.Close();
                fsEncrypted.Close();

            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(@"Error, file to encrypt not found.");
            }
            catch (ArgumentNullException)
            {
                if (encryptorWindow.FsInput == null)
                {

                }
            }
        }
    }
}
