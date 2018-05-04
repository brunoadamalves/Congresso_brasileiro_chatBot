using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace botCongresso.Utils.LUIS
{
    public static class LuisClassification
    {
        
        private static readonly string luisAppId = " Secret Key :) ";
        private static readonly string subscriptionKey = "Secret Key :) ";

        public static async Task<Model.LUIS.LuisResultModel> luisRequest(string query)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // The request header contains your subscription key
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // The "q" parameter contains the utterance to send to LUIS
            queryString["q"] = query;

            // These optional request parameters are set to their default values
            queryString["timezoneOffset"] = "0";
            queryString["verbose"] = "false";
            queryString["spellCheck"] = "false";
            queryString["staging"] = "false";

            var uri = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/" + luisAppId + "?" + queryString;
            var response = await client.GetAsync(uri);

            var strResponseContent = await response.Content.ReadAsStringAsync();
            
            // Display the JSON result from LUIS
            Model.LUIS.LuisResultModel result = new JavaScriptSerializer().Deserialize<Model.LUIS.LuisResultModel>(strResponseContent.ToString());
            
            return result;
        }
    }
}