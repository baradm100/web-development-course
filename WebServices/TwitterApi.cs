using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreTweet;
using Tweetinvi;

namespace web_development_course.WebServices
{
    public class TwitterApi
    {
            private const string API_URL = @"https://api.twitter.com";
            private const string API_VERSION = "1.1";
            private readonly HMACSHA1 _sigHasher;
            private readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            private readonly string _apiKey = "K4lBFKr3tQLgPsylAWCPxj7B9";
            private readonly string _apiSecret = "8qpdssKjeJEHXbGxn0OOnv1bFcnG4sBTxp2ziefrYksRb2kyIv";
            private readonly string _oauthToken = "1440563452007747586-m5nvkmIRxeK0jYGFg7YH7Zfw4GVwb7";
            private readonly string _oauthTokenSecret = "JJZsvbv9evKD86wJTAuaVR2mGzZg5csIxKxojn6ANIloq";

        public TwitterApi()
        {
            _sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes($"{_apiSecret}&{_oauthTokenSecret}"));
        }

        public Task<string> PostTweetAsync(string text)
            {
                var data = new Dictionary<string, string> {
                { "status", text }
            };
                return this.SendRequest("statuses/update.json", data);
            }

            private Task<string> SendRequest(string urlMethod, Dictionary<string, string> data)
            {
                // the request url
                string url = ($"{API_URL}/{API_VERSION}/{urlMethod}");
            
                // Adding all headers we need to use when constructing the hash.
                int timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);
                data.Add("oauth_consumer_key", _apiKey);
                data.Add("oauth_signature_method", "HMAC-SHA1");
                data.Add("oauth_timestamp", timestamp.ToString());
                data.Add("oauth_nonce", Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString())));
                data.Add("oauth_token", _oauthToken);
                data.Add("oauth_version", "1.0");
                data.Add("oauth_signature", this.CreateOauthSignature(url, data));


                // Builds the OAuth HTTP Header from the data.
                string oAuthHeader = this.CreateOAuthHeader(data);
                var formData = new FormUrlEncodedContent(data.Where(kvp => !kvp.Key.StartsWith("oauth_")));
                return this.SendRequest(url, oAuthHeader, formData);
            }


            private string CreateOauthSignature(string url, Dictionary<string, string> data)
            {
                string sigString = string.Join(
                    "&",
                    data.Union(data).Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value))).OrderBy(s => s)
                );
                string fullSigData = string.Format("{0}&{1}&{2}", "POST", Uri.EscapeDataString(url), Uri.EscapeDataString(sigString.ToString()));
                return Convert.ToBase64String(_sigHasher.ComputeHash(new ASCIIEncoding().GetBytes(fullSigData.ToString())));
            }

            private string CreateOAuthHeader(Dictionary<string, string> data)
            {
                return "OAuth " + string.Join(", ",data.Where(kvp => kvp.Key.StartsWith("oauth_")).Select(kvp => string.Format("{0}=\"{1}\"", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                        .OrderBy(s => s)
                );
            }

            private async Task<string> SendRequest(string fullUrl, string oAuthHeader, FormUrlEncodedContent formData)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", oAuthHeader);
                    var response = await httpClient.PostAsync(fullUrl, formData);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }

