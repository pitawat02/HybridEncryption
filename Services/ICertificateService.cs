using System.Threading.Tasks;
using HybridEncryption.Models;
using Microsoft.AspNetCore.Http;

namespace HybridEncryption.Services;
public interface ICertificateService
{
    Task<string> IsExpired(IFormFile certificateSEC, bool isProduction, string baseDirectory);
    Task<string> IsExpiredByPath (string certificateFilePath, bool isProduction);
}