using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace TwitterClientTest
{
	public class TwitterService
	{
		
        private const string oAuthConsumerKey = "Jy3qlSItDbCCW1TUDsuP0ldjz";
		private const string oAuthConsumerSecret = "YiNY4MtPJ568AONj83Y4JeclzJCddAJKnghGLUODMZLeJ3AGfF";

		public IEnumerable<ImageTweet> GetImageTweets(string query)
		{
			var results= GetSearchResult(query);
			return from twit in results.statuses
				   where twit.entities.media != null
				   select new ImageTweet { MediaUrl = twit.entities.media.First().media_url, Tweet = twit.text };
			
		}

		public TwitterSearchResult GetSearchResult(string query)
		{
			TwitterSearchResult searchResult = null;
			using (var accessToken = new TwitterAccess(oAuthConsumerKey, oAuthConsumerSecret))
			{
				searchResult = FetchSearchResult(accessToken, query);
			}
			return searchResult;
		}

		private TwitterSearchResult FetchSearchResult(TwitterAccess accessToken, string query)
		{

			var getSearchResultRequest = WebRequest.Create(string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}%20filter:images&count=100",WebUtility.HtmlEncode(query))) as HttpWebRequest;
			getSearchResultRequest.Method = "GET";
			getSearchResultRequest.Headers[HttpRequestHeader.Authorization] = "Bearer " + accessToken.AccessToken;
			try
			{
				string respbody = null;
				using (var resp = getSearchResultRequest.GetResponse().GetResponseStream()) //there request sends
				{
					var respR = new StreamReader(resp);
					respbody = respR.ReadToEnd();
				}

				TwitterSearchResult searchResult = null;
				using (var stringReader = new StringReader(respbody))
				{
					using (var jsonReader = new JsonTextReader(stringReader))
					{
						JsonSerializer js = new JsonSerializer();
						searchResult = js.Deserialize<TwitterSearchResult>(jsonReader);
					}
				}

				return searchResult;

				//TODO use a library to parse json
				//MessageBox.Show(respbody);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
    }

}
