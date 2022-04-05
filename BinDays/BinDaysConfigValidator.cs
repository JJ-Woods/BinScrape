using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace BinScrape;

public class BinDaysConfigValidator : IValidateOptions<BinDaysConfig>
{
    public ValidateOptionsResult Validate(string name, BinDaysConfig options)
    {
        if(!NoDuplicateKeys(options))
            return ValidateOptionsResult.Fail("There are multiple configurations for a single bin type. Requires exactly one config per object.");

        //Impliment better config checks later
        foreach(var pattern in options.BinPatterns.Select(bp => bp.Pattern))
        {
            if(!IsValidRegex(pattern))
                return ValidateOptionsResult.Fail("Invalid regex");

            if(!OnlyOneCaptureGroup(pattern))
                return ValidateOptionsResult.Fail("More than one capture group");
        }

        return ValidateOptionsResult.Success;
    }

    private bool NoDuplicateKeys(BinDaysConfig config)
    {
        var uniqueKeys = config.BinPatterns
            .Select(pattern => pattern.BinType)
            .Distinct()
            .Count();

        return config.BinPatterns.Count() == uniqueKeys;
    }

    private bool IsValidRegex(string pattern)
    {
        
        return true;
    }

    private bool OnlyOneCaptureGroup(string pattern)
    {
        return true;
    }
}