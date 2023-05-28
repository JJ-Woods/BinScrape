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

    private IBinCollectionWebPageFactory _binCollectionWebPageFactory;

    public BinDaysController(
        ILogger<BinDaysController> logger
        , BinDaysConfig binDaysConfig
        , IBinCollectionWebPageFactory binCollectionWebPageFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _binDaysConfig = binDaysConfig ?? throw new ArgumentNullException(nameof(binDaysConfig));
        _binCollectionWebPageFactory = binCollectionWebPageFactory ?? throw new ArgumentNullException(nameof(binCollectionWebPageFactory));
    }

    public IEnumerable<BinCollectionDay> Get()
    {
        var collectionPage = _binCollectionWebPageFactory.CreateAsync(_binDaysConfig.BinCalendarLink).Result;

        var test = _binDaysConfig.BinPatterns.First().Pattern;
        var regexMatch = Regex.Match(collectionPage.Html, test);

        var binDays = _binDaysConfig.BinPatterns.Select(pattern =>
        {
            var regexMatch = Regex.Match(collectionPage.Html, pattern.Pattern);

            if (!regexMatch.Success)
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
