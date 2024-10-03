using System.Threading.Tasks;

namespace PKHeX.Core;

public static class LocalizeUtil
{
    /// <summary>
    /// 将PKHeX的运行时字符串初始化为指定语言。
    /// </summary>
    /// <param name="lang">2-char language ID</param>
    /// <param name="sav">Save data (optional)</param>
    /// <param name="hax">Permit illegal things (items, only)</param>
    public static void InitializeStrings(string lang, SaveFile? sav = null, bool hax = false)
    {
        var str = GameInfo.Strings = GameInfo.GetStrings(lang);
        if (sav != null)
            GameInfo.FilteredSources = new FilteredGameDataSource(sav, GameInfo.Sources, hax);

        // Update Legality Analysis strings
        ParseSettings.ChangeLocalizationStrings(str.movelist, str.specieslist);

        // Update Legality Strings
        Task.Run(() =>
        {
            RibbonStrings.ResetDictionary(str.ribbons);
            LocalizationUtil.SetLocalization(typeof(LegalityCheckStrings), lang);
            LocalizationUtil.SetLocalization(typeof(MessageStrings), lang);
        });
    }
}
