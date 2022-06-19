using UnityEngine.UI;
using UnityEngine;

public class LanguageSelect : MonoBehaviour
{
    [SerializeField] UIFlag[] flags = new UIFlag[2];

    public void SelectFlag(UIFlag newSelectedFlag)
    {
        CheckFlagList();

        foreach (UIFlag flag in flags)
        {
            if (flag == newSelectedFlag)
            {
                flag.SetAsSelected();
                continue;
            }            
            flag.SetAsNotSelected();
        }        
    }

    void Start()
    {
        CheckFlagList();

        string currentSelectedLanguage = PlayerPrefs.GetString("Language", "English");
        switch (currentSelectedLanguage)
        {
            case "English":
                SelectFlag(flags[0]);
                break;
            case "Ukrainian":
                SelectFlag(flags[1]);
                break;
            default:
                break;
        }
    }

    void CheckFlagList()
    {
        if (flags.Length == 0)
        {
            Debug.LogError("No Flag Options selected. Can't change language.", this);
            this.enabled = false;
            return;
        }
    }
}