using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;
using SpotifyFinder_v2.Model;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;


namespace SpotifyFinder.Data
{
    

    public class HttpGrabber
    {
        private string BaseAddress = "https://api.spotify.com/v1/";

        // serialize root object

        public async Task <RootObject> MakeStringGreatAgain(string search)

        {
            RootObject root = new RootObject();
            
            try
            {

                root = JsonConvert.DeserializeObject<RootObject>(await GetFromStream(search));

            }
            catch(Exception ex)
            {
                var a = ex.Message.ToString();
            }

            return root;
        }

        //get token method 

        public async Task<AccessToken> GetToken()
        {
            AccessToken token = new AccessToken();
            string clientId = "";
            string clientSecret = "";
            string credentials = String.Format("{0}:{1}", clientId, clientSecret);

            
                using (var client = new HttpClient())
                {
                    //Define Headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

                    //Prepare Request Body
                    List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                    requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));

                    FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                    //Request Token
                    var request = await client.PostAsync("https://accounts.spotify.com/api/token", requestBody);
                    var response = await request.Content.ReadAsStringAsync();

                    return token = JsonConvert.DeserializeObject<AccessToken>(response);
                     
                    
                }

           
        }

        //Get test data from http
        private async Task<string> GetFromStream(string search)
        {
            string testRequest = search;

            // Get HttpWebRequest
            try
            {
               var response = await GetRequestAsync(testRequest);

                using (var responseReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return await responseReader.ReadToEndAsync();
                }

            }
            catch (Exception)
            {
                // just pass

            }
            return testRequest;
        }

        // Get HttpWebResult
        private async Task <WebResponse> GetRequestAsync(string testRequest)
        {
            try
            { 
            var token = await GetToken();
            var accessik = token.access_token;
            var request = HttpWebRequest.CreateHttp(BaseAddress + "search?q=" + testRequest + "&type=track%2Cartist&market=US&limit=10&offset=5");
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization: Bearer " + accessik);

                return await request.GetResponseAsync();
            }

            catch(WebException wex)
            {
                return wex?.Response as WebResponse;
            }
            catch(Exception)
            {
            }
            return null;
        }
    }
}

