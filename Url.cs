using System.Text.RegularExpressions;
using EFT;
using UnityEngine;

namespace WikiLinks;

public static class Url
{
    public static void OpenWiki(string id)
    {
        var locale = Settings.UseLocalizedLinks.Value ? LocalizationManager.Instance.Culture : "en";

        LocalizationManager.Instance.TryGetLocalization($"{id} Name", locale, out var itemName);
        
        var wikiName = WikiEncode(itemName);

        var localePath = locale == "en" ? string.Empty : $"{locale}/";

        Application.OpenURL($"https://escapefromtarkov.fandom.com/{localePath}wiki/{wikiName}");
    }

    // This is NOT standard url encoding. This is what the wiki does with names.
    public static string WikiEncode(string input)
    {
        return Regex.Replace(input, "<[^>]+>", string.Empty) // Remove xml-style tags (added by mods like ItemInfo)
            .Replace("[K] ", string.Empty) // KappaMarker mod
            .Replace(' ', '_') // Replace spaces with underscore
            .Replace("#", string.Empty); // Remove # character
    }
}