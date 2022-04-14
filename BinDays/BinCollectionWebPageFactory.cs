namespace BinScrape;

public class BinCollectionWebPageFactory : IBinCollectionWebPageFactory
{
    public async Task<BinCollectionWebPage> CreateAsync(string webpageLink)
    {
        var client = new HttpClient();
        var html = await client.GetStringAsync(webpageLink);
        return new BinCollectionWebPage(DateTime.Now, html);
        
    }
}