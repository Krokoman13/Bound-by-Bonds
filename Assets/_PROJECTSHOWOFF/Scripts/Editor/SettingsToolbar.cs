using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SettingsToolbar : Editor
{
    static string languagePrefName = "Language";
    [MenuItem("LeadTheWay/Settings/Current_Language/English")]
    static void SelectLanguage_English()
    {
        PlayerPrefs.SetString(languagePrefName, "English");        
    }

    [MenuItem("LeadTheWay/Settings/Current_Language/Ukrainian")]
    static void SelectLanguage_Ukrainian()
    {
        PlayerPrefs.SetString(languagePrefName, "Ukrainian");        
    }

    [MenuItem("LeadTheWay/Settings/Current_Language/Dutch")]
    static void SelectLanguage_Dutch()
    {
        PlayerPrefs.SetString(languagePrefName, "Dutch");
    }

}