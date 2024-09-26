using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using HybridEncryption.Models;

namespace HybridEncryption.Services;
public interface ISecurityService
{
    bool IsDirectoryTraversing(string fileName);
    string CheckIsExecutableByByte(IFormFile file);
    bool CheckIsExecutableByExtension(string filePath);
    List<string> CheckIsExecutableByExtensionInFolder(string[] folderPath);
    List<string> CheckIsExecutableByBytePath(List<string> filePath);
}