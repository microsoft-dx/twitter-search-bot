using LinqToTwitter;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TwitterBot
{
    public class TwitterClient
    {
        private static TwitterContext TwitterContext { get; set; }

        static TwitterClient()
        {
            TwitterContext = new TwitterContext(new SingleUserAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"],
                    OAuthToken = ConfigurationManager.AppSettings["twitterAccessToken"],
                    OAuthTokenSecret = ConfigurationManager.AppSettings["twitterAccessTokenSecret"]
                }
            });
        }

        public static string GetTweets(string query)
        {
            var search = TwitterContext.Search.Where(t => t.Type == SearchType.Search)
                                  .Where(t => t.Query == query)
                                  .SingleOrDefault();

            return GetStringTweets(search.Statuses.Take(3));
        }

        private static string GetStringTweets(IEnumerable<Status> statuses)
        {
            string result = "";
            foreach (var tweet in statuses)
                result += tweet.ScreenName + "\n\n" + tweet.Text + "\n\n\n";

            return result;
        }
    }
}