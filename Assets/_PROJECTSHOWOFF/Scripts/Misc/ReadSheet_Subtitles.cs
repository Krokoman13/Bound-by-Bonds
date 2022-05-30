using System.IO;
using System.Text;
using UnityEngine;

public class ReadSheet_Subtitles : MonoBehaviour
{    
    [SerializeField] string streamingAssetsPathPrefix = "FMOD_Audio";


    [ContextMenu("Test Read Text")]
    void testReadText()
    {
        GetSubtitleFromIndex(1);        
    }


    public Subtitle GetSubtitleFromIndex(int lineIndex)
    {
        StringBuilder sheetPath = new StringBuilder($"{Application.streamingAssetsPath}/{getLanguagePath}/Subtitles.txt");

        StreamReader sr = new StreamReader(sheetPath.ToString(), true);       

        //Skip lines until we get to the correct one.
        for(int i = 1; i <= lineIndex; i++)        
            sr.ReadLine();        

        //Get the correct line        
        string[] lineTab = sr.ReadLine().Split("\t");        

        //Create a new subtitle based off of the read line.        
        Subtitle sub = new Subtitle();
        sub.text = lineTab[1];
        sub.speaker = lineTab[2];
        sub.appearTime = System.Int32.Parse(lineTab[4]);        
        sub.audioKey = $"Voice_Line_{lineTab[6]}_{lineIndex}";
        return sub;
    }

    string getLanguagePath
    {
        get
        {
            return PlayerPrefs.GetString("Language", "English");
        }
    }
}