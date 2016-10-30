using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
//using System.ServiceModel.Web;
//using System.Runtime.Serialization.Json;

namespace YandexDisc
{
    class Disc
    {
        String _tokenID;
        String _urlAPI;
        WebClient _client;

        public event EventHandler<UploadProgressChangedEventArgs> UploadFileChangedEvent;
        public event EventHandler<UploadFileCompletedEventArgs> UploadFileCompleteEvent;
        public event EventHandler<DownloadProgressChangedEventArgs> DownloadFileChangedEvent;
        public event EventHandler<AsyncCompletedEventArgs> DownloadFileCompleteEvent;
        

        public Disc(String _token)
        {
            _tokenID = _token;
            _urlAPI = "https://cloud-api.yandex.net:443";
            _client = GetWebClient();
        }


        private WebClient GetWebClient()
        {        
            WebClient TempClient = new WebClient();
            TempClient.Headers[HttpRequestHeader.Authorization] = String.Format("OAuth {0}", _tokenID);

            return TempClient;
        }

        private void WebClientUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            EventHandler<UploadProgressChangedEventArgs> temp = UploadFileChangedEvent;
            if (UploadFileChangedEvent != null)
                temp(sender, e);
        }

        private void WebClientUploadComplete(object sender, UploadFileCompletedEventArgs e)
        {
            EventHandler<UploadFileCompletedEventArgs> temp = UploadFileCompleteEvent;
            if (UploadFileChangedEvent != null)
                temp(sender, e);
        }

        private void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            EventHandler<DownloadProgressChangedEventArgs> temp = DownloadFileChangedEvent;
            if (DownloadFileChangedEvent != null)
                temp(sender, e);
        }

        private void WebClientDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            EventHandler<AsyncCompletedEventArgs> temp = DownloadFileCompleteEvent;
            if (DownloadFileChangedEvent != null)
                temp(sender, e);
        }



        String RequestApi(string URL)
        {
            string ansewer = "";
            try
            {

                Stream data = _client.OpenRead(URL);
                StreamReader reader = new StreamReader(data);
                ansewer = reader.ReadToEnd();
            }
            catch (WebException e)
            {
                ansewer = String.Format("Error: {0}", e.Message);
            }
            return ansewer;
        }

        public TypeDisc GetDiscInfo()
        {
            string s = RequestApi(_urlAPI + "/v1/disk");

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(TypeDisc));
            TypeDisc info = (TypeDisc)json.ReadObject(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(s)));
            return info;
        }

        public TypeResource GetResource(string path = "/")
        {
            string s = RequestApi(_urlAPI + "/v1/disk/resources?path=" + path);

            DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(TypeResource));
            TypeResource resource = (TypeResource)json.ReadObject(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(s)));
            return resource;
        }

        private String RequestApi(string path, string method)
        {
            String ansewer = "";
            try
            {
                ansewer = _client.UploadString(path, method, "");
                //_client.UploadDataAsync()
            }
            catch (WebException e)
            {
                MessageBox.Show(String.Format("Действие не выполнено ({0})", e.Message, "Ошибка!"));
                ansewer = String.Format("Error: {0}", e.Message);
                //String s = e.Status.ToString();
            }
            return ansewer;
        }

        public void CreateDirectory(string path)
        {
            RequestApi(_urlAPI + "/v1/disk/resources?path=" + path, "PUT");
        }
        public void DeleteDirectory(string path)
        {
            RequestApi(_urlAPI + "/v1/disk/resources?path=" + path + "&permanently=true", "DELETE");
        }

        public void UploadFile(String InPath, String OutPath)
        {
            //string path = "https://uploader11h.disk.yandex.net:443/upload-target/20151214T235214.068.utd.c5fecc335nw2vvry75rcl5fzp-k11h.619884";
            //_client.UploadString(path, "PUT", "Test");
            String ansewer = RequestApi(_urlAPI + "/v1/disk/resources/upload?path=" + InPath);

            if (!Regex.IsMatch(ansewer, "Error", RegexOptions.IgnoreCase))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(TypeLink));
                TypeLink Link = (TypeLink)json.ReadObject(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(ansewer)));
                //_client.UploadString(Link.href, Link.method, File.ReadAllBytes(OutPath))
                // _client.UploadFile(Link.href, Link.method, OutPath);
                //_client.UploadFileAsync()
                WebClient UploadClient = GetWebClient();
                UploadClient.UploadFileAsync(new System.Uri(Link.href), Link.method, OutPath);
                UploadClient.UploadProgressChanged += WebClientUploadProgressChanged;
                UploadClient.UploadFileCompleted += WebClientUploadComplete;
            }

        }

        public void DownloadFile(String InPath, String OutPath)
        {
            String ansewer = RequestApi(_urlAPI + "/v1/disk/resources/download?path=" + OutPath);
            if (!Regex.IsMatch(ansewer, "Error", RegexOptions.IgnoreCase))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(TypeLink));
                TypeLink Link = (TypeLink)json.ReadObject(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(ansewer)));
                //_client.DownloadFile(Link.href, InPath);
                WebClient DownloadClient = GetWebClient();
                DownloadClient.DownloadFileAsync(new System.Uri(Link.href), InPath);
                DownloadClient.DownloadProgressChanged += WebClientDownloadProgressChanged;
                DownloadClient.DownloadFileCompleted += WebClientDownloadComplete;
            }

        }

    }
}
