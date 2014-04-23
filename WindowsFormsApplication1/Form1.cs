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
        string fileToEncrypt;
        string whereToSave;
        string fileToDecryptLocation;

        public Form1()
        {
            
            InitializeComponent();      
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            sSecretKey = GenerateKey();
            SaveFileDialog whereToSaveFileDialog = new SaveFileDialog();
            whereToSaveFileDialog.Filter = "txt files (*.txt)|*.txt";
            whereToSaveFileDialog.RestoreDirectory = true;

            if(whereToSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                whereToSave = whereToSaveFileDialog.FileName.ToString();
            }
            EncryptFile(fileToEncrypt, whereToSave, sSecretKey);
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            sSecretKey = GenerateKey();
            OpenFileDialog fileToDecrypt = new OpenFileDialog();
            fileToDecrypt.Title = "Open .txt File";
            fileToDecrypt.Filter = "txt Files|*.txt";
            fileToDecrypt.InitialDirectory = "@C:\\";

            if (fileToDecrypt.ShowDialog() == DialogResult.OK)
            {
                fileToDecryptLocation = fileToDecrypt.FileName.ToString();
            }
            SaveFileDialog whereToSaveFileDialog = new SaveFileDialog();
            whereToSaveFileDialog.Filter = "txt files (*.txt)|*.txt";
            whereToSaveFileDialog.RestoreDirectory = true;

            if (whereToSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                whereToSave = whereToSaveFileDialog.FileName.ToString();
            }

            DecryptFile(fileToDecryptLocation, whereToSave, sSecretKey);
        }

        private void passPhraseSelectButton_Click(object sender, EventArgs e)
        {
            
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Open .txt File";
            fDialog.Filter = "TXT Files|*.txt";
            fDialog.InitialDirectory = "@C:\\";

            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                fileToEncrypt = fDialog.FileName.ToString();
                fileLocation.Text = fileToEncrypt;
            }
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
                try
                {
                    DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                }
                catch (ArgumentNullException ne)
                {
                    MessageBox.Show("No password set.");
                }

                    ICryptoTransform desencrypt = DES.CreateEncryptor();
                    CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
                    byte[] bytearrayinput = new byte[fsInput.Length - 1];
                    fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
                    cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                    cryptostream.Close();
                    fsInput.Close();
                    fsEncrypted.Close();
                
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Error, file to encrypt not found.");
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
