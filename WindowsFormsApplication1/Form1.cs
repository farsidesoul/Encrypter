using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Encrypter
{
    public partial class Form1 : Form
    {
        string _sSecretKey;
        string _password;
        string _fileToEncrypt;
        string _whereToSave;
        string _fileToDecryptLocation;
        private const string FilterType = "txt files (*.txt)|*.txt";
        private const string BlankPassPhrase = "Passphrase cannot be blank";

        public Form1()
        {
            
            InitializeComponent();      
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            _sSecretKey = GenerateKey();
            // Checks to ensure there is a passphrase to use to encrypt
            if (_password.Length > 0)
            {
                SaveFileDialog whereToSaveFileDialog = new SaveFileDialog
                {
                    Title = @"Choose location to save Encrypted file:",
                    Filter = FilterType,
                    RestoreDirectory = true,
                    FileName = "Encrypted File.txt",
                };

                if (whereToSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _whereToSave = whereToSaveFileDialog.FileName;
                }
                EncryptFile(_fileToEncrypt, _whereToSave, _sSecretKey); 
            }
            else
            {
                MessageBox.Show(BlankPassPhrase);
            }
            
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            _sSecretKey = GenerateKey();
            // Checks to ensure there is a passphrase to use to decrypt.
            if (_password.Length > 0)
            {
                OpenFileDialog fileToDecrypt = new OpenFileDialog
                {
                    Title = @"Open .txt File",
                    Filter = FilterType,
                    InitialDirectory = "@C:\\"
                };

                if (fileToDecrypt.ShowDialog() == DialogResult.OK)
                {
                    _fileToDecryptLocation = fileToDecrypt.FileName;
                }
                SaveFileDialog whereToSaveFileDialog = new SaveFileDialog
                {
                    Title = @"Choose location to save decrypted file:",
                    Filter = FilterType,
                    FileName = "Decrypted File.txt",
                    RestoreDirectory = true,
                };

                if (whereToSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _whereToSave = whereToSaveFileDialog.FileName;
                }

                DecryptFile(_fileToDecryptLocation, _whereToSave, _sSecretKey);
            }
            else
            {
                MessageBox.Show(BlankPassPhrase);
            }
            
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog
            {
                Title = @"Open .txt File",
                Filter = FilterType,
                InitialDirectory = "@C:\\"
            };

            if (fDialog.ShowDialog() != DialogResult.OK) return;
            _fileToEncrypt = fDialog.FileName;
            fileLocation.Text = _fileToEncrypt;
        }

        string GenerateKey()
        {
            _password = secretKeyBox.Text;
            PasswordDeriveBytes cdk = new PasswordDeriveBytes(_password, null);
            // Generate a DES key
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] key = cdk.CryptDeriveKey("DES", "SHA1", 56, iv);
            Console.WriteLine(key.Length * 8);
            string str = System.Text.Encoding.ASCII.GetString(key);
            return str;
        }

        private void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
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
                if (FsInput == null)
                {

                }
            }
        }

        private static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
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
        }

        public object FsInput { get; set; }
    }
}
