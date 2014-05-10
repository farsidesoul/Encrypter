using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Encrypter
{
    class Decryption
    {
        public void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                // Set secret key for DES algorithm
                Key = Encoding.ASCII.GetBytes(sKey),
                // Set initialization vector.
                IV = Encoding.ASCII.GetBytes(sKey),
            };

            // Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);

            // Create a DES decryptor from the DES instance
            ICryptoTransform desdecrypt = des.CreateDecryptor();

            //Create crypto stream set to read and do a DES decryption transform on incoming bytes
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);

            //Print the contents of the decrpyted file
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            try
            {
                fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            }
            catch (CryptographicException)
            {
                MessageBox.Show(@"Password is incorrect.");
            }

            fsDecrypted.Flush();
            fsDecrypted.Close();
            cryptostreamDecr.Close();
            fsread.Close();
        }
    }
}
