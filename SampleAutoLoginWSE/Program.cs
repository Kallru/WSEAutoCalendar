using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SampleAutoLoginWSE
{
    class Program
    {
        static void Main(string[] args)
        {
            String callUrl = "https://iamapi.wallstreetenglish.com/api/connect/token";
            
            String postData = "username=sl-kn.32366.kr&amp;password=Hwijae1234&amp;clientId=WSE-FrontEnd&amp;clientSecret=bnNlLXBhc3N3b3Jk";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
            // 인코딩 UTF-8
            byte[] sendData = UTF8Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            httpWebRequest.Method = "POST";            
            httpWebRequest.ContentLength = sendData.Length;
            httpWebRequest.Accept = "application/json, text/plain, */*";

            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(sendData, 0, sendData.Length);
            requestStream.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            var aa = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();

            Console.Write("return: " + aa);
        }
    }
}
