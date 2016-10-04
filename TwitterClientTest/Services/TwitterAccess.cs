using System;
using System.Net;
using System.Text;
using System.IO;

namespace TwitterClientTest
{
	internal class TwitterAccess : IDisposable
	{
		public TwitterAccess(string authConsumerKey, string authConsumerSecret)
		{
			GetAuthorizationToken(authConsumerKey, authConsumerSecret);
		}
		public string AccessToken { get; set; }
		private HttpWebRequest TokenHttpRequest { get; set; }

		private void GetAuthorizationToken(string authConsumerKey, string authConsumerSecret)
		{

			//var oAuthUrl = "https://api.twitter.com/oauth2/token";
			//var screenname = "aScreenName";

			// Do the Authenticate
			var authHeaderFormat = "Basic {0}";

			var authHeader = string.Format(authHeaderFormat,
				Convert.ToBase64String(Encoding.UTF8.GetBytes(Uri.EscapeDataString(authConsumerKey) + ":" +
				Uri.EscapeDataString((authConsumerSecret)))
			));


			TokenHttpRequest = WebRequest.Create("https://api.twitter.com/oauth2/token") as HttpWebRequest;

			TokenHttpRequest.Method = "POST";
			TokenHttpRequest.ContentType = "application/x-www-form-urlencoded";
			TokenHttpRequest.Headers[HttpRequestHeader.Authorization] = authHeader;
			var reqbody = Encoding.UTF8.GetBytes("grant_type=client_credentials");
			TokenHttpRequest.ContentLength = reqbody.Length;
			using (var req = TokenHttpRequest.GetRequestStream())
			{
				req.Write(reqbody, 0, reqbody.Length);
			}
			try
			{
				string respbody = null;
				using (var resp = TokenHttpRequest.GetResponse().GetResponseStream())//there request sends
				{
					var respR = new StreamReader(resp);
					respbody = respR.ReadToEnd();
				}

				AccessToken = respbody.Substring(respbody.IndexOf("access_token\":\"") + "access_token\":\"".Length, respbody.IndexOf("\"}") - (respbody.IndexOf("access_token\":\"") + "access_token\":\"".Length));
			}
			catch (Exception ex) //if credentials are not valid (403 error)
			{
				throw ex;
			}
		}

		private void DisposeAuthorizationToken()
		{

			var inv = WebRequest.Create("https://api.twitter.com/oauth2/invalidate_token") as HttpWebRequest;
			inv.Method = "POST";
			inv.ContentType = "application/x-www-form-urlencoded";
			//inv.Headers[HttpRequestHeader.Authorization] = authHeader;
			var reqbodyinv = Encoding.UTF8.GetBytes("access_token=" + AccessToken);
			inv.ContentLength = reqbodyinv.Length;
			using (var req = inv.GetRequestStream())
			{
				req.Write(reqbodyinv, 0, reqbodyinv.Length);
			}
			try
			{
				TokenHttpRequest.GetResponse();
			}
			catch //token not invalidated
			{
				throw;
			}
		}


		public void Dispose()
		{
			DisposeAuthorizationToken();
		}

	}
}
