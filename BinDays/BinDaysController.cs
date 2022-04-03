using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BinScrape;

[ApiController]
[Route("[controller]")]
public class BinDaysController : ControllerBase
{
    private readonly ILogger<BinDaysController> _logger;

    private readonly string _calendarLink;

    private readonly Dictionary<BinType, string> _binPatterns;

    private BinCollectionWebPage? _lazyCachedWebPage;

    public BinDaysController(ILogger<BinDaysController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _calendarLink = "https://www.huntingdonshire.gov.uk/refuse-calendar/10023829590";

        _binPatterns = new Dictionary<BinType, string>();
        _binPatterns.Add(BinType.BlackBin, "The next collection for your domestic waste in your 240lt wheeled bin is on <strong>(.*)<\\/strong>");
        _binPatterns.Add(BinType.BlueBin, "The next collection for your dry recycling in your 240lt wheeled bin is on <strong>(.*)<\\/strong>");
        _binPatterns.Add(BinType.GreenBin, "The next collection for your garden waste in your 240lt wheeled bin is on <strong>(.*)<\\/strong>");
    }

    private async Task<BinCollectionWebPage> GetBinCollectionWebPageAsync(bool ignoreCache = false)
    {
        var getFromCache = false;

        if(ignoreCache || !getFromCache)
        {
            var client = new HttpClient();
            var html = await client.GetStringAsync(_calendarLink);
            _lazyCachedWebPage = new BinCollectionWebPage(DateTime.Now, html);
        }
        
        return _lazyCachedWebPage ?? throw new Exception("Well Shit.");
    }

    public IEnumerable<BinCollectionDay> Get()
    {
        var collectionPage = GetBinCollectionWebPageAsync().Result;

        var binDays = _binPatterns.Select(pattern => 
        {
            var regexMatch = Regex.Match(collectionPage.Html, pattern.Value);

            if(!regexMatch.Success)
                throw new Exception("Shit son.");

            var collectionDate = regexMatch.Groups[1].ToString();

            var binTypeLongForm = pattern
                .Key
                .GetType()
                .GetMember(pattern.Key.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()
                .Name;

            return new BinCollectionDay(binTypeLongForm, DateTime.Parse(collectionDate));
        });

        return binDays;
    }
}
