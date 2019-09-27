using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;
using TweetSharp;
using System.Diagnostics;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices
{
    public class TwitterUserDomainServices : ITwitterUserDomainServices
    {

        public ResultOfType<TwitterUserModel> VerifyCredentials()
        {
            try
            {
                const string oauthconsumerkey = "XFcUpJrXdFb5wNBjJa4iwfaLH";
                const string oauthconsumersecret = "dFjfz1FkG1uGBLPRjjhQRNyPpBgMnxuRKiuz7S9JRuy07EQ3xy";
                const string oauthsignaturemethod = "HMAC-SHA1";
                const string oauthversion = "1.0";
                const string oauthtoken = "2900651746-NReB7jdE4nzKO53TmIKtqN3sl1WUgMKtcx1xh5z";
                const string oauthtokensecret = "6cYa7BEz98vfFSPs0fD8hmmZBT9DiCWNn93Jlt5AR0g1M";
                string oauthnonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)));


                // Pass your credentials to the service
                TwitterService service = new TwitterService(oauthconsumerkey, oauthconsumersecret);

                // Step 1 - Retrieve an OAuth Request Token
                //OAuthRequestToken requestToken = service.GetRequestToken();
                OAuthRequestToken requestToken = new OAuthRequestToken { OAuthCallbackConfirmed = true, Token = oauthtoken, TokenSecret = oauthtokensecret };
                // Step 2 - Redirect to the OAuth Authorization URL
                Uri uri = service.GetAuthorizationUri(requestToken);
                //Process.Start(uri.ToString());
                // Step 3 - Exchange the Request Token for an Access Token
                string verifier = "123456"; // <-- This is input into your application by your user
                OAuthAccessToken access = service.GetAccessToken(requestToken, verifier);

                // Step 4 - User authenticates using the Access Token
                service.AuthenticateWith(access.Token, access.TokenSecret);
              //  IEnumerable<TwitterStatus> mentions = service.ListTweetsMentioningMe();

                //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                //string oauthtimestamp = Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);
                //var basestringParameters = new SortedDictionary<string, string>
                //{
                //    {"oauth_version", "1.0"},
                //    {"oauth_consumer_key", oauthconsumerkey},
                //    {"oauth_nonce", oauthnonce},
                //    {"oauth_signature_method", "HMAC-SHA1"},
                //    {"oauth_timestamp", oauthtimestamp},
                //    {"oauth_token", oauthtoken}
                //};
                ////GS - Build the signature string
                //var baseString = new StringBuilder();
                //baseString.Append("GET" + "&");
                //baseString.Append(Uri.EscapeDataString("https://api.twitter.com/1.1/account/verify_credentials.json") );
                //foreach (KeyValuePair<string, string> entry in basestringParameters)
                //{
                //    baseString.Append(Uri.EscapeDataString(entry.Key + "=" + entry.Value + "&"));
                //}

                ////Since the baseString is urlEncoded we have to remove the last 3 chars - %26
                //string finalBaseString = baseString.ToString().Substring(0, baseString.Length - 3);

                ////Build the signing key
                //string signingKey = Uri.EscapeDataString(oauthconsumersecret) + "&" +Uri.EscapeDataString(oauthtokensecret);

                ////Sign the request
                //HMACSHA1 hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));
                //string oauthsignature = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(finalBaseString)));

                ////Tell Twitter we don't do the 100 continue thing
                //ServicePointManager.Expect100Continue = false;

                ////authorization header
                //var hwr = (HttpWebRequest)WebRequest.Create(
                //  @"https://api.twitter.com/1.1/account/verify_credentials.json");
                //var authorizationHeaderParams = new StringBuilder();
                //authorizationHeaderParams.Append("OAuth ");
                //authorizationHeaderParams.Append("oauth_nonce=" + "\"" + Uri.EscapeDataString(oauthnonce) + "\",");
                //authorizationHeaderParams.Append("oauth_signature_method=" + "\"" + Uri.EscapeDataString(oauthsignaturemethod) + "\",");
                //authorizationHeaderParams.Append("oauth_timestamp=" + "\"" + Uri.EscapeDataString(oauthtimestamp) + "\",");
                //authorizationHeaderParams.Append("oauth_consumer_key=" + "\"" + Uri.EscapeDataString(oauthconsumerkey) + "\",");
                //if (!string.IsNullOrEmpty(oauthtoken))
                //    authorizationHeaderParams.Append("oauth_token=" + "\"" + Uri.EscapeDataString(oauthtoken) + "\",");
                //authorizationHeaderParams.Append("oauth_signature=" + "\"" + Uri.EscapeDataString(oauthsignature) + "\",");
                //authorizationHeaderParams.Append("oauth_version=" + "\"" + Uri.EscapeDataString(oauthversion) + "\"");
                //hwr.Headers.Add("Authorization", authorizationHeaderParams.ToString());
                //hwr.Method = "GET";
                //hwr.ContentType = "application/x-www-form-urlencoded";

                ////Allow us a reasonable timeout in case Twitter's busy
                //hwr.Timeout = 3 * 60 * 1000;
                //var postBody = "grant_type=XFcUpJrXdFb5wNBjJa4iwfaLH";
                //using (Stream stream = hwr.GetRequestStream())
                //{
                //    byte[] content = ASCIIEncoding.ASCII.GetBytes(postBody);
                //    stream.Write(content, 0, content.Length);
                //}
                //hwr.Headers.Add("Accept-Encoding", "gzip");
                //WebResponse authResponse = hwr.GetResponse();
                //// deserialize into an object
                //TwitAuthenticateResponse twitAuthResponse;
                ////using (authResponse)
                ////{
                ////    using (var reader = new StreamReader(hwr.GetResponseStream()))
                ////    {
                ////        JavaScriptSerializer js = new JavaScriptSerializer();
                ////        var objectText = reader.ReadToEnd();
                ////        twitAuthResponse = JsonConvert.DeserializeObject<TwitAuthenticateResponse>(objectText);
                ////    }
                ////}
                ////// Do the timeline
                ////var timelineFormat = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}&include_rts=1&exclude_replies=1&count=5";
                ////var timelineUrl = string.Format(timelineFormat, screenname);
                ////HttpWebRequest timeLineRequest = (HttpWebRequest)WebRequest.Create(timelineUrl);
                ////var timelineHeaderFormat = "{0} {1}";
                ////timeLineRequest.Headers.Add("Authorization", string.Format(timelineHeaderFormat, twitAuthResponse.token_type, twitAuthResponse.access_token));
                ////timeLineRequest.Method = "Get";
                ////WebResponse timeLineResponse = timeLineRequest.GetResponse();
                ////var timeLineJson = string.Empty;
                ////using (timeLineResponse)
                ////{
                ////    using (var reader = new StreamReader(timeLineResponse.GetResponseStream()))
                ////    {
                ////        timeLineJson = reader.ReadToEnd();
                ////    }
                ////}

            }
            catch (Exception ex)
            {
                
            }
            throw new System.NotImplementedException();
        }

        public class TwitAuthenticateResponse
        {
            public string token_type { get; set; }
            public string access_token { get; set; }
        }
    }
}
