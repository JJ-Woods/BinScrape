namespace BinScrape;

public class BinCollectionWebPage
{
    public DateTime DateRetrieved { get; }

    public string Html { get; }

    public BinCollectionWebPage(DateTime dateRetrieved, string html)
    {
        DateRetrieved = dateRetrieved;
        Html = html;
    }
}