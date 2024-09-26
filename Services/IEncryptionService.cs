using System.Threading.Tasks;
using HybridEncryption.Models;
using Microsoft.AspNetCore.Http;

namespace HybridEncryption.Services;
public interface IEncryptionService
{
    ServiceResult EncryptFile(string filePath, string certificatePath, string outputPath, bool isProduction);
    ServiceResult EncryptFolder(string folderPath, string certificatePath, string outputPath, string outputName, bool isProduction);
    ServiceResult CheckCertificate(string certificatePath, bool isProduction);
}