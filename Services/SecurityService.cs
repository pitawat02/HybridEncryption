using HybridEncryption.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace HybridEncryption.Services;
public class SecurityService : ISecurityService
{
    public SecurityService(){}
    public static List<string> listExecutable = new List<string> {".PHP",".PHP3",".PHP4",".PHP5",".PHTML",".CGI",".ASP",".ASPX",".ASMX",".JSP",".JSPX",".SH",".BASH",".KSH",".PY",".PYC",".PYO",".PYD",".PL",".PM",".EXE",".DLL",".JAR",".RB",".RBW",".C",".CPP",".H",".HPP",".JAVA",".CLASS",".HTML",".HTM",".SHTML",".JS",".JSX",".PS1",".VBS",".VBE",".JS",".JSE",".SCPT",".APPLESCRIPT",".CMD",".BAT",".M",".MATLAB",".SWIFT",".LUA",".SQL",".FCGI",".R",".TS",".AHK",".V"};

    public bool IsDirectoryTraversing(string fileName)
    {
        bool isTraversing = false;

        if(String.IsNullOrWhiteSpace(fileName))
        {
            return isTraversing;
        }
        List<string> traversalList = new List<string> {"../", @"..\", @"\..", "/..", "$", "..", "?"};
        // Url decode to reveal sneaky encoded attempts e.g. '%2f' (/) or '%2e%2e%2f' (../)
        
        var decodedFileName = HttpUtility.UrlDecode(fileName);
        if (traversalList.Any(x => decodedFileName.Contains(x))){
            isTraversing = true;
        }
        return isTraversing;
    }
    
    public string CheckIsExecutableByByte(IFormFile file)
    {
        var firstBytes = new byte[8];
        using (var memstream = file.OpenReadStream())
        {
            memstream.Read(firstBytes, 0,8);
            memstream.Close();
        }

        if(Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "MZ" || Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "ZM" || Encoding.UTF8.GetString(firstBytes).ToUpper() == "FEEDFACE" || Encoding.UTF8.GetString(firstBytes).ToUpper() == "FEEDFACF" || Encoding.UTF8.GetString(firstBytes).Substring(0,4).ToUpper() == ".ELF")
        {
            return "ห้ามส่งไฟล์ Windows/DOS/Mach/ELF Executable";
        }
        if(Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "#!" || Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "%!")
        {
            return "ห้ามส่งไฟล์ Script";
        }
        if(Encoding.UTF8.GetString(firstBytes).Substring(0,5).ToUpper().Contains("ECHO"))
        {
            return "ห้ามส่งไฟล์ .bat";
        }
        if(Encoding.UTF8.GetString(firstBytes).Substring(0,4).ToLower() == "xar!")
        {
            return "ห้ามส่งไฟล์ .pkg";
        }
        if(Encoding.UTF8.GetString(firstBytes).Substring(0,4).ToLower() == "#@~^")
        {
            return "ห้ามส่งไฟล์ VBScript";
        }
        
        return null;
    }

    public List<string> CheckIsExecutableByBytePath(List<string> filePaths)
    {
        List<string> listError = new List<string>();
        foreach (var filePath in filePaths){
            var firstBytes = new byte[8];
            using(var fileStream = File.Open(filePath, FileMode.Open))
            {
                fileStream.Read(firstBytes, 0, 8);
                fileStream.Close();
            }
            if(Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "MZ" || Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "ZM" || Encoding.UTF8.GetString(firstBytes).ToUpper() == "FEEDFACE" || Encoding.UTF8.GetString(firstBytes).ToUpper() == "FEEDFACF" || Encoding.UTF8.GetString(firstBytes).Substring(0,4).ToUpper() == ".ELF")
            {
                listError.Add( "ที่ : " + filePath + " ห้ามส่งไฟล์ Windows/DOS/Mach/ELF Executable");
            }
            if(Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "#!" || Encoding.UTF8.GetString(firstBytes).Substring(0,2) == "%!")
            {
                listError.Add( "ที่ : " + filePath + " ห้ามส่งไฟล์ Script");
            }
            if(Encoding.UTF8.GetString(firstBytes).Substring(0,5).ToUpper().Contains("ECHO"))
            {
                listError.Add( "ที่ : " + filePath + " ห้ามส่งไฟล์ .bat");
            }
            if(Encoding.UTF8.GetString(firstBytes).Substring(0,4).ToLower() == "xar!")
            {
                listError.Add( "ที่ : " + filePath + " ห้ามส่งไฟล์ .pkg");
            }
        }
        return listError;
    }

    public bool CheckIsExecutableByExtension(string filePath)
    {
        string extension = Path.GetExtension(filePath);
        List<string> extensionList = listExecutable;
        if (extensionList.Contains(extension.ToUpper().Trim())){
            return true;
        }
        return false;
    }

    public List<string> CheckIsExecutableByExtensionInFolder(string[] folderPath)
    {
        List<string> extensionList = listExecutable;
        if (folderPath.Any(f => extensionList.Contains(Path.GetExtension(f).ToUpper().Trim()))){
            return folderPath.Where(f => extensionList.Contains(Path.GetExtension(f).ToUpper().Trim())).ToList();
        }
        return null;
    }

}