using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace NoiseCalculator.UI.Web.Helpers
{
    public class JsonHelper
    {
        public static ContentResult SerializeObjectToContentResult(object obj)
        {
            var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
            
            var contentResult = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings),
                    ContentType = "application/json"
                };

            return contentResult;
        }
    }
}