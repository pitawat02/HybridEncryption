// MainWindow.axaml.cs
using System.IO.Abstractions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HybridEncryption.Services; // Include the namespace
using System;
using Avalonia.Media;
using Avalonia;
namespace HybridEncryption
{
    public partial class MainWindow : Window
    {
        private readonly EncryptionService _encryptionService;
        public MainWindow()
        {
            InitializeComponent();
            _encryptionService = new EncryptionService();
        }
        
        // Encrypt File Tab Event Handlers
        private async void ToEncryptFilePath_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                ToEncryptFilePath.Text = result[0];
            }
        }
        private async void CertificateFileSECPath_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                CertificateFileSECPath.Text = result[0];
            }
        }
        // Encrypt File Output Path
        private async void EncryptFile_OutputPath_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new OpenFolderDialog();
            var result = await openFolderDialog.ShowAsync(this);
            if (!string.IsNullOrEmpty(result))
            {
                EncryptFile_OutputPath.Text = result;
            }
        }

        private void EncryptFile_Execute_Click(object sender, RoutedEventArgs e)
        {
            var filePath = ToEncryptFilePath.Text;
            var certificatePath = CertificateFileSECPath.Text;
            var isProduction = EncryptFile_Option.IsChecked == true;
            var outputPath = EncryptFile_OutputPath.Text;
            var result = _encryptionService.EncryptFile(filePath, certificatePath, outputPath, isProduction);
            // Update status label and response textbox
            UpdateStatusLabel(EncryptFile_StatusLabel, EncryptFile_StatusText, result.Status);
            EncryptFile_ResponseText.Text = result.Response;
        }
        // Encrypt Folder Tab Event Handlers
        private async void CertificateFileSECPath_Click2(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                CertificateFileSECPath2.Text = result[0];
            }
        }
        private async void FolderPath_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new OpenFolderDialog();
            var result = await openFolderDialog.ShowAsync(this);
            if (!string.IsNullOrEmpty(result))
            {
                FolderPath.Text = result;
            }
        }
        // Encrypt Folder Output Path
        private async void EncryptFolder_OutputPath_Click(object sender, RoutedEventArgs e)
        {
            var openFolderDialog = new OpenFolderDialog();
            var result = await openFolderDialog.ShowAsync(this);
            if (!string.IsNullOrEmpty(result))
            {
                EncryptFolder_OutputPath.Text = result;
            }
        }
        private void EncryptFolder_Execute_Click(object sender, RoutedEventArgs e)
        {
            var folderPath = FolderPath.Text;
            var certificatePath = CertificateFileSECPath2.Text;
            var isProduction = EncryptFolder_Option.IsChecked == true;
            var outputPath = EncryptFolder_OutputPath.Text;
            var outputName = "test";
            var result = _encryptionService.EncryptFolder(folderPath, certificatePath, outputPath,outputName, isProduction);
            // Update status label and response textbox
            UpdateStatusLabel(EncryptFolder_StatusLabel, EncryptFolder_StatusText, result.Status);
            EncryptFolder_ResponseText.Text = result.Response;
        }       
        // Check Certificate Tab Event Handlers
        private async void CheckCert_BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var result = await openFileDialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                CertificateFileSECPath3.Text = result[0];
            }
        }

        private void CheckCert_Execute_Click(object sender, RoutedEventArgs e)
        {
            var certificatePath = CertificateFileSECPath3.Text;
            var isProduction = CheckCert_Option.IsChecked == true;
            var result = _encryptionService.CheckCertificate(certificatePath, isProduction);
            // Update status label and response textbox
            UpdateStatusLabel(CheckCert_StatusLabel, CheckCert_StatusText, result.Status);
            CheckCert_ResponseText.Text = result.Response;
        }
        private void UpdateStatusLabel(Border statusLabel, TextBlock statusText, string status)
        {
            switch (status?.ToLower())
            {
                case "success":
                    statusLabel.Background = Brushes.Green;
                    statusText.Text = "Success";
                    break;
                case "failed":
                    statusLabel.Background = Brushes.Red;
                    statusText.Text = "Failed";
                    break;
                default:
                    statusLabel.Background = Brushes.Gray;
                    statusText.Text = "";
                    break;
            }
        }
        private void ClearToEncryptFilePath(object sender, RoutedEventArgs e)
        {
            ToEncryptFilePath.Text = string.Empty;
        }
        private void ClearCertificateFileSECPath(object sender, RoutedEventArgs e)
        {
            CertificateFileSECPath.Text = string.Empty;
        }

        private void ClearOutputPath(object sender, RoutedEventArgs e)
        {
            EncryptFile_OutputPath.Text = string.Empty;
        }
        private void ClearFolderPath(object sender, RoutedEventArgs e)
        {
            FolderPath.Text = string.Empty;
        }
        private void ClearCertificateFileSECPath2(object sender, RoutedEventArgs e)
        {
            CertificateFileSECPath2.Text = string.Empty;
        }
        private void ClearOutputPath2(object sender, RoutedEventArgs e)
        {
            EncryptFolder_OutputPath.Text = string.Empty;
        }
        private void ClearOutputName2(object sender, RoutedEventArgs e)
        {
            EncryptFolder_OutputName.Text = string.Empty;
        }
        private void ClearCertificateFileSECPath3(object sender, RoutedEventArgs e)
        {
            CertificateFileSECPath3.Text = string.Empty;
        }

    }
}