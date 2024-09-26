using HybridEncryption.Models;
using System.IO.Abstractions;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;

namespace HybridEncryption.Services;
public class CertificateService : ICertificateService
{
    private readonly IFileSystem _fileSystem;


    public CertificateService(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public async Task<string> IsExpired(IFormFile certificateFile, bool isProduction, string baseDirectory)
    {
        string inputFilePath = Path.Combine(baseDirectory, "InputFiles/");

        if (!System.IO.Directory.Exists(inputFilePath)){
            System.IO.Directory.CreateDirectory(inputFilePath);
        }

        //อัพโหลด Certificate มาเก็บไว้ใน local
        string decryptedFullName = Path.Combine(inputFilePath, certificateFile.FileName);
        
        if (certificateFile.Length > 0)
        {
            using (Stream fileStream = new FileStream(decryptedFullName, FileMode.Create, FileAccess.Write))
            {
                certificateFile.CopyTo(fileStream);
            }
        }

        // อ่าน Public key แล้วใส่ไปใน RSA
        byte[] publicPemBytes = _fileSystem.File.ReadAllBytes(inputFilePath+certificateFile.FileName);

        //Check Valid PublicKey

        //เช็ควันหมดอายุในไฟล์
        try {
            using var publicX509 = new X509Certificate2(publicPemBytes);
            if (publicX509.NotAfter < DateTime.UtcNow){
                return $"Certificate หมดอายุไปแล้วเมื่อวันที่ {publicX509.NotAfter.ToString("dd/MM/yyyy")}";
            };
        }
        catch {
            return "ไฟล์ certificateFileSEC ที่แนบมา ไม่ใช่ Certificate";
        }
        //เช็ควันหมดอายุในระบบ
        // hash publicKey
        byte[] hashPublicKey = null;
        using (MD5 myMD5 = MD5.Create())
        {
            hashPublicKey = myMD5.ComputeHash(publicPemBytes);
        }
        //แปลง byte[] เป็น string
        StringBuilder strPublicKey = new StringBuilder();
        for (int i = 0; i < hashPublicKey.Length; i++)
        {
            strPublicKey.Append(hashPublicKey[i].ToString("x2"));
        }
        //เอา string hashPublicKey เข้าตัวแปร
        RequestCertificate requestCertificate = new RequestCertificate() {
            HashCertificateSEC = strPublicKey.ToString()
        };

        //Check Valid PublicKey
        var client = new HttpClient();
        var checkUrl = "https://web-portal-report-api-iwt.sec.or.th/api/certificate/status";
        if (isProduction){
            checkUrl = "https://web-e-reporting-api.sec.or.th/api/certificate/status";
        }
        HttpResponseMessage response = await client.PostAsJsonAsync(checkUrl, requestCertificate);
        if ((int)response.StatusCode != 200)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
        return null;

    }
    public async Task<string> IsExpiredByPath (string certificateFilePath, bool isProduction)
    {
        // อ่าน Public key แล้วใส่ไปใน RSA
        byte[] publicPemBytes = _fileSystem.File.ReadAllBytes(certificateFilePath);

        //Check Valid PublicKey

        //เช็ควันหมดอายุในไฟล์
        try {
            using var publicX509 = new X509Certificate2(publicPemBytes);
            if (publicX509.NotAfter < DateTime.UtcNow){
                return $"Certificate หมดอายุไปแล้วเมื่อวันที่ {publicX509.NotAfter.ToString("dd/MM/yyyy")}";
            };
        }
        catch {
            return "ไฟล์ certificateFileSEC ที่แนบมา ไม่ใช่ Certificate";
        }

        //เช็ควันหมดอายุในระบบ
        // hash publicKey
        byte[] hashPublicKey = null;
        using (MD5 myMD5 = MD5.Create())
        {
            hashPublicKey = myMD5.ComputeHash(publicPemBytes);
        }
        //แปลง byte[] เป็น string
        StringBuilder strPublicKey = new StringBuilder();
        for (int i = 0; i < hashPublicKey.Length; i++)
        {
            strPublicKey.Append(hashPublicKey[i].ToString("x2"));
        }
        //เอา string hashPublicKey เข้าตัวแปร
        RequestCertificate requestCertificate = new RequestCertificate() {
            HashCertificateSEC = strPublicKey.ToString()
        };

        //Check Valid PublicKey
        var client = new HttpClient();
        var checkUrl = "https://web-portal-report-api-iwt.sec.or.th/api/certificate/status";
        if (isProduction){
            checkUrl = "https://web-e-reporting-api.sec.or.th/api/certificate/status";
        }
        HttpResponseMessage response = await client.PostAsJsonAsync(checkUrl, requestCertificate);
        if ((int)response.StatusCode != 200)
        {
            return response.Content.ReadAsStringAsync().Result;
        }
        return null;
    }
}