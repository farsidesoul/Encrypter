using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace Encrypter
{
    public partial class Form1 : Form
    {
        string sSecretKey;
        string password;

        public Form1()
        {
            
            InitializeComponent();      
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            EncryptFile(@"C:\Users\ElephantPC\Desktop\TestFile.txt", @"C:\Users\ElephantPC\Desktop\EncryptedTestFile.txt", sSecretKey);
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            DecryptFile(@"C:\Users\ElephantPC\Desktop\EncryptedTestFile.txt", @"C:\Users\ElephantPC\Desktop\DencryptedTestFile.txt", sSecretKey);
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            sSecretKey = GenerateKey();
        }

        string GenerateKey()
        {
            password = secretKeyBox.Text;
            PasswordDeriveBytes cdk = new PasswordDeriveBytes(password, null);
            // Generate a DES key
            byte[] iv = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] key = cdk.CryptDeriveKey("DES", "SHA1", 56, iv);
            Console.WriteLine(key.Length * 8);
            string str = System.Text.Encoding.ASCII.GetString(key);
            return str;
            
        }

        private void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            try
            {
                FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
                FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                ICryptoTransform desencrypt = DES.CreateEncryptor();
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
                byte[] bytearrayinput = new byte[fsInput.Length - 1];
                fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                cryptostream.Close();
                fsInput.Close();
                fsEncrypted.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, no password set.");
            }
            

        }

        private void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            // Set secret key for DES algorithm
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            // Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            // Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);

            // Create a DES decryptor from the DES instance
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            
            //Create crypto stream set to read and do a DES decryption transform on incoming bytes
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);

            //Print the contents of the decrpyted file
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            try
            {
                fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            }
            catch (CryptographicException de)
            {
                MessageBox.Show("Password is incorrect.");
            }
            
            fsDecrypted.Flush();
            fsDecrypted.Close();
        }
    }
}
