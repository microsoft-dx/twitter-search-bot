Twitter Search Bot
--------------------------

For a complete tutorial on how to build this bot using Microsoft Bot Framework and how to integrate it with Microsoft Cognitive Services,  [see this blog post.](https://radu.microsoft.pub.ro/how-to-build-a-twitter-search-bot-using-microsoft-bot-framework-and-cognitive-services/)

To see a real demo of this bot, go to this website and create natural language queries. The queries will be processed by [LUIS](http://luis.ai) and if the intent is right, it will search Twitter using the entities it identified.


Building the solution
------------------------------

In order to build the solution, you need a LUIS application and subscription and a Twitter developer app.

After you get these, create a file called `appSettings.config` in the root of the solution and replace the placeholders here with your keys.


    <?xml version="1.0" encoding="utf-8" ?>
    <appSettings>
      <add key="AppId" value="YourAppId" />
      <add key="AppSecret" value="YourAppSecret" />
    
      <add key="luisApplicationId" value="luisAppId"/>
      <add key="luisSubscriptionKey" value="luisSubscriptionKey"/>
    
      <add key="twitterAccessToken" value="twitterAccessToken"/>
      <add key="twitterAccessTokenSecret" value="twitterAccessTokenSecret"/>
      <add key="twitterConsumerKey" value="twitterConsumerKey"/>
      <add key="twitterConsumerSecret" value="twitterConsumerSecret"/>
    </appSettings



