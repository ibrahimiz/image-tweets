using System;
namespace TwitterClientTest
{
	public class TwitterSearchResult
	{

		public Status[] statuses { get; set; }
		public Search_Metadata search_metadata { get; set; }
	}

	public class Search_Metadata
	{
		public float completed_in { get; set; }
		public long max_id { get; set; }
		public string max_id_str { get; set; }
		public string query { get; set; }
		public string refresh_url { get; set; }
		public int count { get; set; }
		public int since_id { get; set; }
		public string since_id_str { get; set; }
	}

	public class Status
	{
		public string created_at { get; set; }
		public long id { get; set; }
		public string id_str { get; set; }
		public string text { get; set; }
		public Entities entities { get; set; }
		public User user { get; set; }
	}

	public class Entities
	{
		public Medium[] media { get; set; }
	}

	public class Medium
	{
		public long id { get; set; }
		public string id_str { get; set; }
		public string media_url { get; set; }
		public string type { get; set; }
	}

	public class User
	{
		public long id { get; set; }
		public string id_str { get; set; }
		public string name { get; set; }
		public string screen_name { get; set; }
	}
}
