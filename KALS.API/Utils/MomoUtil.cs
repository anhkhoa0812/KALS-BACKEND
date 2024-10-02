using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace KALS.API.Utils;

public class MomoUtil
{
    public static string signSHA256(string message, string key)
    {
        byte[] keyByte = Encoding.UTF8.GetBytes(key);
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        using (var hmacsha256 = new HMACSHA256(keyByte))
        {
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            string hex = BitConverter.ToString(hashmessage);
            hex = hex.Replace("-", "").ToLower();
            return hex;

        }
    }
    public static string sendPaymentRequest(string endpoint, string postJsonString)
    {

        try
        {
            HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

            var postData = postJsonString;

            var data = Encoding.UTF8.GetBytes(postData);

            httpWReq.ProtocolVersion = HttpVersion.Version11;
            httpWReq.Method = "POST";
            httpWReq.ContentType = "application/json";

            httpWReq.ContentLength = data.Length;
            httpWReq.ReadWriteTimeout = 30000;
            httpWReq.Timeout = 22000;
            Stream stream = httpWReq.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

            string jsonresponse = "";

            using (var reader = new StreamReader(response.GetResponseStream()))
            {

                string temp = null;
                while ((temp = reader.ReadLine()) != null)
                {
                    jsonresponse += temp;
                }
            }


            
            return jsonresponse;
            //return new MomoResponse(mtid, jsonresponse);

        }
        catch (WebException e)
        {
            return e.Message;
        }
    }
}