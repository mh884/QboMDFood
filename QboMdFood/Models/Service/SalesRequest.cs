using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using QboMdFood.Models.API;
using RestSharp;

namespace QboMdFood.Models.Service
{
    public class SalesRequest
    {

        private static string Url()
        {
            return ConfigurationManager.AppSettings["APIURL"];
        }

        private static T Execute<T>(RestRequest request) where T : new()
        {
            string url = Url() + "/webapi/getsales.pxp";
            var client = new RestClient(url);

            var result = client.Execute(request);
            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("response"));
            using (var stringReader = new StringReader(result.Content))
            using (var reader = XmlReader.Create(stringReader))
            {
                return (T)serializer.Deserialize(reader);

            }


        }

        public Response GetCall()
        {
            var request = new RestRequest();
            request = new RestRequest(Method.POST);

            string body = "<root>" + "<header api_key=\"z35zpey2s8sbj4d3g3fxsqdx\" company_code=\"MDCOLD\"/>\r\n" + "</root>";


            request.AddParameter("text/xml", body, ParameterType.RequestBody);

            Response r = Execute<Response>(request);

            return r;
        }

    }

}