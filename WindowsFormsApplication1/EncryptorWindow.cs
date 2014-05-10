using System;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Encrypter
{
    public partial class EncryptorWindow : Form
    {
        //ButtonClicks buttonClicks = new ButtonClicks();
        private string _sSecretKey;
        string _fileToEncrypt;
        string _whereToSave;
        string _fileToDecryptLocation;

        private const string FilterType = "txt files (*.txt)|*.txt";
        private const string BlankPassPhrase = "Passphrase cannot be blank";

        Encryption encryption = new Encryption();
        static Decryption decryption = new Decryption();

        public EncryptorWindow()
        {
            InitializeComponent();      
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            //buttonClicks.encryptButton_Click(sender, e);
            _sSecretKey = GenerateKey();
            // Checks to ensure there is a passphrase to use to encrypt
            if (secretKeyBox.Text != null)
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
                encryption.EncryptFile(_fileToEncrypt, _whereToSave, _sSecretKey);
            }
            else
            {
                MessageBox.Show(BlankPassPhrase);
            }

        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            //buttonClicks.decryptButton_Click(sender, e);
            _sSecretKey = GenerateKey();
            // Checks to ensure there is a passphrase to use to decrypt.
            if (secretKeyBox.Text != null)
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

                decryption.DecryptFile(_fileToDecryptLocation, _whereToSave, _sSecretKey);
            }
            else
            {
                MessageBox.Show(BlankPassPhrase);
            }
            
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            //buttonClicks.browseButton_Click(sender,e);
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

        private string GenerateKey()
        {
            PasswordDeriveBytes cdk = new PasswordDeriveBytes(secretKeyBox.Text, null);
            // Generate a DES key
            byte[] iv = { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] key = cdk.CryptDeriveKey("DES", "SHA1", 56, iv);
            Console.WriteLine(key.Length * 8);
            string str = System.Text.Encoding.ASCII.GetString(key);
            return str;
        }
        
        public object FsInput { get; set; }
        public string GetGenerateKey { get { return this.GenerateKey(); }}
        public string FileLocationLabel { set { fileLocation.Text = value; }}
    }
}
