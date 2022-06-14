using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Localizations
{
    public static LocalizedStringTable _stringTable = new LocalizedStringTable { TableReference = "Main" };
    public static LocalizedString GetLocalizedString(string key)
    {
        return new LocalizedString() { TableReference = _stringTable.TableReference, TableEntryReference = key };
    }
}
