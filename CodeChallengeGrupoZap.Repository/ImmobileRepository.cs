using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using CodeChallengeGrupoZap.Domain.Entities;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace CodeChallengeGrupoZap.Repository
{
    public class ImmobileRepository : IImmobileRepository
    {
        private readonly IConfiguration _config;
        public IList<Immobile> Properties { get; set; }

        public ImmobileRepository(IConfiguration config)
        {
            _config = config;
            Properties = LoadProperties();
        }

        private IList<Immobile> LoadProperties()
        {
            string json = LoadJson();

            return JsonConvert.DeserializeObject<IList<Immobile>>(json);
        }

        private string LoadJson()
        {
            string json = "";
            string url = _config.GetSection("Urls:Source-2.json").Value;

            try
            {
                WebRequest request = WebRequest.Create(url);  
                WebResponse response = request.GetResponse();

                Stream stream = response.GetResponseStream();
                StreamReader readStream = new StreamReader (stream, Encoding.UTF8);

                json = readStream.ReadToEnd();

                response.Close ();
                readStream.Close ();
            }
            catch
            {
            }

            return json;
        }
    }
}