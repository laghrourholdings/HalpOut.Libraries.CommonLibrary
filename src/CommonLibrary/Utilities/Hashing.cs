using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.Utilities;

public static class Hashing
{
    public static string GenerateMD5Hash(string s)
    {
        StringBuilder str = new StringBuilder();
        var md5 = MD5.Create();
        byte[] bytedata;
        bytedata = md5.ComputeHash(new UTF8Encoding().GetBytes(s));
        for(int i=0;i<bytedata.Length;i++)
        {
            str.Append(bytedata[i].ToString("x2"));
        }
        return str.ToString();
    }
}