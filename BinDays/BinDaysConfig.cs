namespace BinScrape;

public class BinDaysConfig
{
    public string BinCalendarLink { get; set; }

    public IEnumerable<BinPattern> BinPatterns { get; set; }
}