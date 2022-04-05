using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BinScrape;

[ApiController]
[Route("")]
public class BinDaysController : ControllerBase
{
    private readonly ILogger<BinDaysController> _logger;

    private readonly BinDaysConfig _binDaysConfig;

    private BinCollectionWebPage? _lazyCachedWebPage;

    public BinDaysController(
        ILogger<BinDaysController> logger
        , BinDaysConfig binDaysConfig)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _binDaysConfig = binDaysConfig ?? throw new ArgumentNullException(nameof(binDaysConfig));
    }

    private async Task<BinCollectionWebPage> GetBinCollectionWebPageAsync(bool ignoreCache = false)
    {
        var getFromCache = false;

        if(ignoreCache || !getFromCache)
        {
            var client = new HttpClient();
            var html = await client.GetStringAsync(_binDaysConfig.BinCalendarLink);
            _lazyCachedWebPage = new BinCollectionWebPage(DateTime.Now, html);
        }
        
        return _lazyCachedWebPage ?? throw new Exception("Well Shit.");
    }

    public IEnumerable<BinCollectionDay> Get()
    {
        var collectionPage = GetBinCollectionWebPageAsync().Result;

        var binDays = _binDaysConfig.BinPatterns.Select(pattern => 
        {
            var regexMatch = Regex.Match(collectionPage.Html, pattern.Pattern);

            if(!regexMatch.Success)
                throw new Exception("Shit son.");

            var collectionDate = regexMatch.Groups[1].ToString();

            var binTypeLongForm = pattern
                .BinType
                .GetType()
                .GetMember(pattern.BinType.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()
                .Name;

            return new BinCollectionDay(binTypeLongForm, DateTime.Parse(collectionDate));
        });

        return binDays;
    }
}
