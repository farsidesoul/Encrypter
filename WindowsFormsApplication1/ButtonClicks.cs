using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Encrypter
{
    class ButtonClicks
    {
//        string _sSecretKey;
//        string _password;
//        string _fileToEncrypt;
//        string _whereToSave;
//        string _fileToDecryptLocation;

//        private const string FilterType = "txt files (*.txt)|*.txt";
//        private const string BlankPassPhrase = "Passphrase cannot be blank";

//        EncryptorWindow encryptorWindow = new EncryptorWindow();
//        Encryption encryption = new Encryption();
//        static Decryption decryption = new Decryption();

//        public void encryptButton_Click(object sender, EventArgs e)
//        {
//            _sSecretKey = encryptorWindow.GetGenerateKey;
//            // Checks to ensure there is a passphrase to use to encrypt
//            if (_password.Length > 0)
//            {
//                SaveFileDialog whereToSaveFileDialog = new SaveFileDialog
//                {
//                    Title = @"Choose location to save Encrypted file:",
//                    Filter = FilterType,
//                    RestoreDirectory = true,
//                    FileName = "Encrypted File.txt",
//                };

//                if (whereToSaveFileDialog.ShowDialog() == DialogResult.OK)
//                {
//                    _whereToSave = whereToSaveFileDialog.FileName;
//                }
//                encryption.EncryptFile(_fileToEncrypt, _whereToSave, _sSecretKey);
//            }
//            else
//            {
//                MessageBox.Show(BlankPassPhrase);
//            }

//        }

//        public void decryptButton_Click(object sender, EventArgs e)
//        {
//            _sSecretKey = encryptorWindow.GetGenerateKey;
//            // Checks to ensure there is a passphrase to use to decrypt.
//            if (_password.Length > 0)
//            {
//                OpenFileDialog fileToDecrypt = new OpenFileDialog
//                {
//                    Title = @"Open .txt File",
//                    Filter = FilterType,
//                    InitialDirectory = "@C:\\"
//                };

//                if (fileToDecrypt.ShowDialog() == DialogResult.OK)
//                {
//                    _fileToDecryptLocation = fileToDecrypt.FileName;
//                }
//                SaveFileDialog whereToSaveFileDialog = new SaveFileDialog
//                {
//                    Title = @"Choose location to save decrypted file:",
//                    Filter = FilterType,
//                    FileName = "Decrypted File.txt",
//                    RestoreDirectory = true,
//                };

//                if (whereToSaveFileDialog.ShowDialog() == DialogResult.OK)
//                {
//                    _whereToSave = whereToSaveFileDialog.FileName;
//                }

//                decryption.DecryptFile(_fileToDecryptLocation, _whereToSave, _sSecretKey);
//            }
//            else
//            {
//                MessageBox.Show(BlankPassPhrase);
//            }

//        }

//        public void browseButton_Click(object sender, EventArgs e)
//        {
//            OpenFileDialog fDialog = new OpenFileDialog
//            {
//                Title = @"Open .txt File",
//                Filter = FilterType,
//                InitialDirectory = "@C:\\"
//            };

//            if (fDialog.ShowDialog() != DialogResult.OK) return;
//            _fileToEncrypt = fDialog.FileName;
//            encryptorWindow.FileLocationLabel = _fileToEncrypt;
//        }
   }
}
