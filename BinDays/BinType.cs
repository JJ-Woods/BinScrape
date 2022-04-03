using System.ComponentModel.DataAnnotations;

namespace BinScrape;

public enum BinType
{
    [Display(Name = "Black Bin")]
    BlackBin, 
    
    [Display(Name = "Blue Bin")]
    BlueBin, 
    
    [Display(Name = "Green Bin")]
    GreenBin
}