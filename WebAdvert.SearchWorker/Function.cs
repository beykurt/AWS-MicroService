using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Nest;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using AdvertApi.Models.Messages;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace WebAdvert.SearchWorker;

public class Function
{
    public Function() : this(ElasticSearchHelper.GetInstance(ConfigurationHelper.Instance))
    {

    }

    private IElasticClient _client;
    public Function(IElasticClient client)
    {
        _client = client;
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
    {
        foreach (var record in snsEvent.Records)
        {
            context.Logger.LogLine(record.Sns.Message);

            var message = JsonConvert.DeserializeObject<AdvertConfirmedMessage>(record.Sns.Message);
            var advertDocument = MappingHelper.Map(message);
            await _client.IndexDocumentAsync(advertDocument);
        }
    }
}