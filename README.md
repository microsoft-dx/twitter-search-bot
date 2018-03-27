How to build a Twitter Search Bot using Microsoft Bot Framework and Cognitive Services
--------------------------

This was done in 2016 with technologies from 2016 :sweat_smile:

To see a real demo of this bot, [go to this website and create natural language queries](http://twitter-search-bot.azurewebsites.net/bot.htm). The queries will be processed by [LUIS](http://luis.ai) and if the intent is right, it will search Twitter using the entities it identified.


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


Content
------------------------------
![Microsoft Bot Framework](https://i1.wp.com/bot-framework.azureedge.net/bot-icons-v1/bot-framework-default-7.png?w=672&ssl=1)

> ([Photo source here](https://bot-framework.azureedge.net/bot-icons-v1/bot-framework-default-7.png))

[You can test the completed bot here.](http://twitter-search-bot.azurewebsites.net/bot.htm)

## Introduction

Last week at its annual developer conference, [Build](https://build.microsoft.com/), Microsoft announced the new [Bot Framework](http://dev.botframework.com/) in the attempt to get developers to build _intelligent_ bots using Microsoft technologies.

In this article, we will introduce the concepts of Conversational AI and bots, and will create a bot using the Microsoft Bot Framework that will search Twitter for tweets containing the user query.

We will then integrate it with [LUIS](https://www.luis.ai/) (Language Understanding Intelligent Service) from the new [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services/) which will allow users to input natural language. Then, with the help of Machine Learning, we will extract the intent from the user’s query and search Twitter.

## What is Conversational AI?

The Conversational AI is not a new concept. I was first introduced to the idea of a conversational bot back in the early 2000s when I was playing with [ELIZA](https://en.wikipedia.org/wiki/ELIZA), an early example of how to use natural language processing to achieve a very near human-like conversational bot.

The idea behind conversational AI is to have a computer program respond to the user as close as possible to a real conversation.

While early programs like ELIZA were based on predefined scripts, the newer ones are based on Artificial Intelligence and Machine Learning.

![ELIZA](https://i1.wp.com/www.scaruffi.com/mind/ai/eliza.jpg?w=672)

> ([Photo source here](http://www.scaruffi.com/))  
> ELIZA is a computer program and an early example of primitive natural language processing. ELIZA operated by processing user’s responses to scripts, the most famous of which was DOCTOR, a simulation of a Rogerian psychotherapist.
> 
> Using almost no information about human thought or emotion, DOCTOR sometimes provided a startlingly human-like interaction. ELIZA was written at the **MIT Artificial Intelligence Laboratory** by Joseph Weizenbaum **between 1964 and 1966.**
> 
> [More information about ELIZA on the Wikipedia page.](https://en.wikipedia.org/wiki/ELIZA)

After having read a lot of Medium and TechCrunch articles in the past months about how bots are the new apps ([here](https://medium.com/@Lewwwk/web-apps-bots-7fb3d733a082#.21254o783), [here](https://medium.com/@msg/we-should-all-have-our-own-bot-c8faa781f0a8#.cuoxrf3mv) or [here](https://medium.com/truth-labs/where-does-conversational-ui-leave-design-7044c395be9f#.tawu7f4oi)), it became pretty clear that the next step in the user interaction is represented by _intelligent_ bots.

## What is a bot?

A bot is a piece of software designed to automate a specific task. When talked about in the context of _conversation as a platform_, a bot becomes the chat interface of a regular app.

So you should allow tasks that required full UI to be performed by the user _only through conversation._

> You just send and receive messages from the bot. **No need to learn, understand and navigate disparate interfaces or languages**. Users will be able to interact with bots just as they interact with other humans. It’s **the most natural way to communicate** and transact.
> 
> [The full TechCrunch article here.](http://techcrunch.com/2015/09/29/forget-apps-now-the-bots-take-over/)

The goal is to have the user input natural language and your bot to perfectly understand and execute the action the user wants.

Of course, you can do this by using terminal-like commands (much like inviting a user to a Slack channel), but then you compel the user into remembering commands and arguments (then, every bot will have specific commands and arguments).

## Microsoft Bot Framework

> Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services.
> 
> [More on the Bot Framework official website](https://dev.botframework.com/)

Microsoft Bot Framework has three main components: Bot Connector, Bot Builder SDK and Bot Directory.

![Microsoft Bot Framework](https://docs.botframework.com/images/bot_framework_wht_bgrnd.png?w=672)

> (Photo from the [official Bot Framework documentation](http://docs.botframework.com/))

The image above is pretty much self explanatory, but in a few words:

-   the **Bot Conenctor** allows you easily to connect your bot to Slack, Skype, via SMS or web.

![Bot Connector](https://i0.wp.com/docs.botframework.com/images/bot_connector_diagram.png?w=672)

> ([Photo source here](http://docs.botframework.com/images/bot_connector_diagram.png))

-   the **Bot Builder** is an SDK that allows you to develop bots using .NET (C#) or Node JS. It is open-source and you can [browse the repository here](https://github.com/Microsoft/BotBuilder).  
    ![enter image description here](https://i0.wp.com/docs.botframework.com/images/bot_builder_sdk.png?w=672)
    
    > ([Photo source here](http://docs.botframework.com/images/bot_builder_sdk.png))
    
-   the **Bot Directory** is a collection of all approved bots connected through the Bot Connector. It is a _marketplace_ where users can search for bots to add in their chat applications.

![enter image description here](https://i2.wp.com/docs.botframework.com/images/bot_directory_mock_comingsoon.png?w=672)

> ([Photo source here)](http://docs.botframework.com/images/bot_directory_mock_comingsoon.png)

In our tutorial, we are going to use the **Bot Builder C# SDK** to create the bot and the **Bot Connector** to test it in a web application.

We will also use the Microsoft Cognitive Services, more exactly the [Language Understanding Intelligent Service (LUIS)](https://www.microsoft.com/cognitive-services/en-us/language-understanding-intelligent-service-luis) which will allow us to compute natural language queries from the user.

## Building the Twitter Bot

> [You can test the complete bot here.](http://twitter-search-bot.azurewebsites.net/bot.htm)

At this point, we can create a new Visual Studio 2015 solution. In order to do this, we follow the step-by-step instructions from [the official documentation.](http://docs.botframework.com/connector/getstarted/#getting-started-in-net)

> This is a step-by-step guide to writing an Bot in C# using the Bot Framework Connector SDK .Net template.
> 
> -   Visual Studio 2015 Update 1 – you can downlodad the community version here for free: [https://www.visualstudio.com](https://www.visualstudio.com)
> 
> Important: Please update all VS extensions to their latest versions Tools->Extensions and Updates->Updates
> 
> -   Download and install the Bot Application template
> -   Download the file from the direct download link [here](http://aka.ms/bf-bc-vstemplate):  
>     Save the zip file to your Visual Studio 2015 templates directory which is traditionally in %USERPROFILE%\\Documents\\Visual Studio 2015\\Templates\\ProjectTemplates\\Visual C#
> -   Open Visual Studio

After completing these steps, we can create a new project based on the Bot template.

![TwitterBot template](https://izgouq.bn1301.livefilestore.com/y3mqQEnRrECtoLwO6hstJti1K36K90Npu2zExHPZWSyMfJz3mzQN9tWVgTLJKN540YTZRTFOjfcs20rFbKc9l1TI_pU4fniuMHL2MRtc7dbwZk15ZHmIdYmdRetPMUzdD6_b98CG1rWiAK4AqHhZ1bF8g?width=1273&height=725&cropmode=none)

As you can see, this is mainly a `WebApi` solution that has some custom attributes, with the `MessagesController'`s `Post` method as entry point.

![Bot Builder solution](https://tb4klw.bn1301.livefilestore.com/y3mMPSULmy0ylrYgoVLY2mP0PBo5YP6EyHVJLX9-X-6LdLNRB9Hk81j3hOoM5Q-nBNFSOCa2f5tasaOGD7Q3JpUqp7nzDO7RJJHNk_-mME-3llYfrIUMoqnkWksQUodFbtNVK8jHalDjh512YwYwzrW8A?width=347&height=402&cropmode=none)

At this point, you can [install the Bot Framework Emulator from here](https://aka.ms/bf-bc-emulator), run the application and start the emulator.

If we look at the code, we see that it simply returns the number of characters from the user input.

![Bot Emulator](https://8u6foq.bn1301.livefilestore.com/y3m2KWHPo9-f414NgO3QA5ijzlWnR-Q1Cqb5BIOMJ8jZYzF6Dej0TujjRJeqalW5ufl45pc17YgcEwWWG8GhJHnK37vNI98Hqmlbxt9XrRndtTjBKdXeFtcTQBw1RxWHnZLpg6taFO1KBZh0YM7hCvS-Q?width=1275&height=751&cropmode=none)

## Searching Twitter

Since we are going to use Twitter data, we need to create and authenticate and a Twitter application.

In order to do this, go to the [Twitter Application Management console](https://apps.twitter.com/) and create an application.

After you create it, you will be able to find the consumer key, consumer secret, the access token access token secret that we will use in order to authenticate the calls to the Twitter API.

![Twitter Keys](https://v3r3qg.bn1301.livefilestore.com/y3m-bfTLpGPCHMr0ovgIozRHdzcXFbHh4GcJDUU_D_wy6mBAUpL3cAtwpvVND19Pu9z6CTVhIc6i1PrenbuQNddzZdTBUJAHcsUzj66U3ZQ5a-wzr1PT6UQXza5OXWxqKkwS5rBHOUuBFSl5owEYb3oJQ?width=1093&height=202&cropmode=none)

We will store the tokens Twitter generated for our app in order to use them when searching. Since we don’t want credentials in a Git repository, I am created a file called `appSettings.config` in which I placed the tokens from Twitter and the authentication tokens from LUIS (we will see this later).

```xml
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
</appSettings>
```

We are going to use a library called [Linq2Twitter](https://linqtotwitter.codeplex.com/) in order to make Twitter queries easier, and we will install int from NuGet.

![Linq2Twitter NuGet](https://jwnhra.bn1301.livefilestore.com/y3m0LDLdsMIzHMIXVGrLPUxEbbUVgoQTQE79FRvlTsPZeP0hPKWIsGwIifZpzs4s8VCAqC93ZuVOKHyNZuMK2MSSivEdJIM9LYaMKsOCLIX7W5kzgzXAhYMw0wf5BzbHq61cL04zqGpTrxHaWyrsSVT-w?width=1909&height=1135&cropmode=none)

## Creating the Twitter Client

At this point, we are ready to create the class that will be responsible for actually executing Twitter searches. At first, we will only execute _un-intellingent_ queries, meaning that we will search Twitter exactly for the input from the user.

As I said, we are implementing a wrapper over Linq2Twitter that is simply going to get tweets based on the user query.

```csharp
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
```

And here is the `Post` method from the controller, with the `HandleSystemMessage` method unchanged.

```csharp
[BotAuthentication]
public class MessagesController : ApiController
{
    public async Task<Message> Post([FromBody]Message message)
    {
        if (message.Type == "Message")
        {
            return message.CreateReplyMessage(TwitterClient.GetTweets(message.Text));
        }
        else
        {
            return HandleSystemMessage(message);
        }
    }
```

As we can see, we simply create the reply message using the `string` that contains the tweets received from the search.

> I limited the results to 3 tweets because the message was rather large. You can have up to 200 tweets using the [Search API](https://dev.twitter.com/rest/public/search). For more tweets, consider using the [Streaming API](https://dev.twitter.com/streaming/overview).

When running the application and testing it with the Bot Framework Emulator, if we input `Satya Nadella`, we can see 3 tweets that satisfy the query:

![Testing the application](https://knju5g.bn1301.livefilestore.com/y3msthGThXUGYghZDswzApVQnDzhp_nN2CNWIGzui0vCL0sG1Jx9Trrz4ZvJ5K4DFUPZNNvndUkfR3mcuE_CRznz-Oxqoe_tvAqHwwRljhlPxpGC0PihKZuJ-4RfXCkWo7e5O-jUPx8DTVF1oukwVGgyQ?width=1014&height=761&cropmode=none)

Now let’s try to input some natural language and see how the system behaves: `what does the world think about bill gates?`

![](https://w0ywiq.bn1301.livefilestore.com/y3mFvqD0KP3H5h4qqynyCyUGTbUEuFmmm1hKF-RdR40Cet9AgVNB0bmz5s6cMHi_miPtcaKgqLCgO9RViiisMcXwc3_WjkOjK4ARtTWmzVoPRkvdDCZ1Mu2VBFBsqSuZ2--HYRjqlWXX-bnivFmmPVWJg?width=1277&height=759&cropmode=none)

Let’s see what happened here: in the first query, the input was simply: `satya nadella`, so the `TwitterClient` just made a query for `satya nadella`.

The second time, the `TwitterClient` did the same thing, this time with a much more complicated query: `what does the world think about bill gates?`, query that returned no results from Twitter.

## Adding support for Natural Language Queries – LUIS

So far, we managed to create a bot that receives queries and makes Twitter searches based on the exact text of the query.

The goal is to have the user input natural language and for the system to extract the intention of the user, as well as the entities of the intention.

For example if a user had the following input: `what does the world think about bill gates?`, the system must understand that the user wants to do a Twitter search for `bill gates`.

For the following input: `how is apple doing?`, it should perform a Twitter search on `apple`.

> One of the key problems in human-computer interactions is the ability of the computer to understand what a person wants, and to find the pieces of information that are relevant to their intent. For example, in a news-browsing app, you might say “Get news about virtual reality companies”, in which case there is the intention to “FindNews”, and “virtual reality companies” is the topic.
> 
> **LUIS is designed to enable you to very quickly deploy an HTTP endpoint that will take the sentences you send it, and interpret them in terms of the intention they convey, and the key entities** like “virtual reality companies” that are present.
> 
> LUIS lets you custom design the set of intentions and entities that are relevant to the application, and then guides you through the process of building a language understanding system.
> 
> Once your application is deployed and traffic starts to flow into the system, LUIS uses active learning to improve itself. In the active learning process, LUIS identifies the interactions that it is relatively unsure of, and asks you to label them according to intent and entities.
> 
> [For the full documentation on LUIS, click here.](https://www.luis.ai/Help)

Before going any further, you should [watch the tutorial from the LUIS Help section](https://www.luis.ai/Help#Video), since it contains all steps in creating a new LUIS application and how to setup entities and a training model.

When ready to start working with LUIS, [create a free subscription here.](https://www.microsoft.com/cognitive-services/en-us/linguistic-analysis-api)

Then, [go to the LUIS website](https://www.luis.ai) and create a new app.

![LUIS new app](https://hhwcfw.bn1301.livefilestore.com/y3m1g9hY141V27XYuF3zlO1AE1rIBisC87--wtnwJc4Myw2C2hGV-sud6WMo73rUL9NpjCqyfP8tfYjeBvMM3-dZOxFz1D2eUDm6_9oiSbo0I4QWhh0Lczm_BYjMu9L60R6RWVPNmXco70b80_87evZRg?width=1265&height=703&cropmode=none)

At this point, when you can see something like this, it means you are ready to define intents and start training your model.

![LUIS app](https://izgpuq.bn1301.livefilestore.com/y3mIwa_bboC9iJ-n4Cn60p0PM5Fqm3AYOvbU_XeiL778LtwgWRnxUv36asZ50WjMODzBrlaYgrFXDOF0D2cMceTb4eJtj2Z7buafqgt7T1WKBpW0VJiIAD5AudXdXL8rOpCJlaoXlgdHiXsvvB_nQ2dwQ?width=1275&height=705&cropmode=none)

Now we add an intent.  
![Intent](https://v3r4qg.bn1301.livefilestore.com/y3mP1-Km6iWi8Ej7Aw8dfupymiHcTPV3Z9kKA-DsYlLTZYi-QaKc3e8Ek2KX-McS_5i4vIQNueCg0a9yZTuCT34PY3KVC_hXTafSy3c13kdERpI-_PKtssRL56RbfYegEX_BJSKBVi3apl2XO_7n9z7aQ?width=1279&height=691&cropmode=none)

Then, we add an entity.  
![Entity](https://vavrxa.bn1301.livefilestore.com/y3mzQJx27LjDGEMz-Tte4F-Yx4Fiot5jOntQAhIqoFNg8tBsDPYkhjrSgUHzhGebsmLBkAt5CZFqb78ksw82QxxNuOCPTS21Bu8qS0l4hYZ3Ar7WrYqtA9hx80lZsyLFUFJ9wkX7ceVsragB6hQDA7J9w?width=1280&height=800&cropmode=none)

Now we can start training the model by feeding it queries and identifying the intent and the entities.

![Train model](https://gkpkja.bn1301.livefilestore.com/y3mWVFC1woichZvEBBT8aO3UM3NLe6ZqZAydItHUTEuYLOVs842bfzHWGIbyGoUISmkH-9uFI7sHH1fF4zP_ROmtHlMiOdxUD6adAI5GnYJWXBiYUhytStDb0xI8bsckq21kNYWvTqynKY10bWfvm1wbQ?width=1275&height=697&cropmode=none)

Since this is a machine learning model, at first you should feed it and classify enough queries (in my case I gave and identified 32 queries).

When we are satisfied with how our model responds to queries, we are ready to publish the model to be accessible programatically.

After hitting publish, we can actually test the model against queries and see how it handles them.

![enter image description here](https://tb4llw.bn1301.livefilestore.com/y3m6-xAL82UJakZ6VucNEWe0Lq2Hbbv9xL4JeI4aDfCVBjVBZJskhZg-yHY_cRaf1Z4e39ekSVOnqBbg8I6DJhLgs4yTWn0LEZF2ldn7VXN9EqbeRPDw72MZhDhHjLsJy7yU1eWTud3qYnjXPmtiAfX8A?width=1273&height=703&cropmode=none)

This is the result it gave us. You can see it correctly identified the intent as being `GetTweets` and the entity as `satya nadella`.

![enter image description here](https://8u6yoq.bn1301.livefilestore.com/y3mpyd-2MvIi1FdV12brb50yShhcSjR_WFLlHJ1QJZuLGk71qrnAP7qGZtf7Xs1Gw4xTc0qoLTqk3WJNTZIIgX6Mxjt5c-jnJ7rTr83_fJsfmigfJ1spCuwXmey7k9QeqqwKOHcUD_X0qEbFhhQEu_7-A?width=1253&height=451&cropmode=none)

Now we need to make a C# client to query the LUIS model we just created.

We will create a new class called `LuisResponse`, we will copy the JSON from above and paste it as a class in Visual Studio.

![VS paste special](https://jwnara.bn1301.livefilestore.com/y3meEM84EtXe3apd2Vler5hNVrnceu45gDBjVpSOBwz-oralzP6eoOzno4BzyoMNvTrM_poFLdUm-U-ZDi1nHQLmpBrUgN8e7-_97byHDm9gWYz8pj9vWtyWlzQYhtDJDptvZtypLd9vygYbm-6cMic4Q?width=1271&height=737&cropmode=none)

After doing this, I placed each class in its own file and followed the .NET naming convention by using `JsonProperty`.

The `Entity` class

```csharp
public class Entity
{
    [JsonProperty(PropertyName = "entity")]
    public string EntityName { get; set; }

    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "startIndex")]
    public int StartIndex { get; set; }

    [JsonProperty(PropertyName = "endIndex")]
    public int EndIndex { get; set; }

    [JsonProperty(PropertyName = "score")]
    public float Score { get; set; }
}
```

The `Intent` class

```csharp
public class Intent
{
    [JsonProperty(PropertyName = "intent")]
    public string IntentName { get; set; }

    [JsonProperty(PropertyName = "score")]
    public float Score { get; set; }

    [JsonProperty(PropertyName = "actions")]
    public object Actions { get; set; }
}
```

The `LuisResponse` class

```csharp
public class LuisResponse
{
    [JsonProperty(PropertyName = "query")]
    public string Query { get; set; }

    [JsonProperty(PropertyName = "intents")]
    public Intent\[\] Intents { get; set; }

    [JsonProperty(PropertyName = "entities")]
    public Entity\[\] Entities { get; set; }
}
```

Now we will create a class responsible for communicating with the LUIS HTTP endpoint and we will call it `LuisClient`.

Now you should grab your `applicationId` and `subscriptionKey` from LUIS in App Settings and place them in the `appSettings.config`.

```csharp
public class LuisClient
{
    private static string url = String.Format("https://api.projectoxford.ai/luis/v1/application?id={0}&subscription-key={1}",                     ConfigurationManager.AppSettings["luisApplicationId"], ConfigurationManager.AppSettings["luisSubscriptionKey"]);


    public static async Task<LuisResponse> GetLuisResponse(string message)
    {
        string query = "&q=" + message;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(url + query);
            return await response.Content.ReadAsAsync<LuisResponse>();
        }
    }
}
```

This class is responsible for retrieving the intent and entities from LUIS based on a query. As you can see, the `GetLuisResponse` method accepts a `message`, then makes an HTTP request to your app’s endpoint, then formats the result as `LuisResponse`.

In the `MessagesController,` we will modify the result so it uses the intent from LUIS.

```csharp
public async Task<Message> Post([FromBody]Message message)
{
    if (message.Type == "Message")
    {
        LuisResponse luisResponse = await LuisClient.GetLuisResponse(message.Text);

        return message.CreateReplyMessage((TwitterClient.GetTweets(luisResponse.Entities[0].EntityName)));
    }
    else
    {
        return HandleSystemMessage(message);
    }
}
```

Now let’s see how our app responds to some natural language:

![Natural language query](https://knjn5g.bn1301.livefilestore.com/y3mAK5g5_2IhMFKxoFdbtEUPUYNQktheyp6tkVJFQn3xGbnI16hrEKS01uiy409aeD_qmajSuqurxpVVY7VHPqbrPbHBQJ0hQzHic7h_za-zX5-sQ7Il8svKaG6SwOBhyucxP18vwUevxWURv74-D6ZSw?width=1011&height=765&cropmode=none)

Right now, we can add more stuff to our bot: we can add confirmation, remember the last query, build more complex queries, add more intents and entities and check them, stuff we will make in part 2 of this article.

## Bot Conclusion

We started by building a pretty straightforward bot: the user would put some exact phrase, then our bot would search Twitter for that phrase.

Then, we added support for natural language queries, meaning the user could input rather complex phrases, then, using LUIS, we would extract the intent and the entities from the phrase.

## Conclusion

> You just send and receive messages from the bot. **No need to learn, understand and navigate disparate interfaces or languages**. Users will be able to interact with bots just as they interact with other humans. It’s **the most natural way to communicate** and transact.

This phrase basically states exactly what a bot should be all about, and bots are becoming the new apps.

This way, we can build bots for the web, for Skype, Slack, SMS and many others.

For a step-by-step tutorial on how to deploy your bot and use it with the Bot Connector, [check this resource.](http://docs.botframework.com/connector/getstarted/#publishing-your-bot-application-to-microsoft-azure)

> [You can test the complete bot here.](http://twitter-search-bot.azurewebsites.net/bot.htm)

