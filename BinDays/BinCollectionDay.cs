namespace BinScrape;

public class BinCollectionDay
{
    public string BinType { get; }

    public DateTime CollectionDate { get; }

    public BinCollectionDay(string binType, DateTime collectionDate)
    {
        BinType = binType;
        CollectionDate = collectionDate;
    }
}