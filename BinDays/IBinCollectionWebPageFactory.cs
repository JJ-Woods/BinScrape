namespace BinScrape;

public interface IBinCollectionWebPageFactory
{
    public Task<BinCollectionWebPage> CreateAsync(string webpageLink);
}