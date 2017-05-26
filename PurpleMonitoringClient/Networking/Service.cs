using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.Networking
{
    using Model.Executing;
    using Newtonsoft.Json;

    class Service
    {
        HttpClient httpClient = new HttpClient();

        static Service instance;

        public static Service Instance
        {
            get
            {
                if (instance == null)
                    instance = new Service();
                return instance;
            }
        }

        Service() {}

        public Task<Program[]> GetAvailableProgramsAsync(string hostName)
        {
            return Task<Program>.Run(() =>
            {
                Uri url = new UriBuilder("http", hostName, 8080, "programs").Uri;
                var response = httpClient.GetAsync(url.AbsoluteUri).Result;
                return JsonConvert.DeserializeObject<Program[]>(response.Content.ReadAsStringAsync().Result);
            });
            
        }

    }
}
