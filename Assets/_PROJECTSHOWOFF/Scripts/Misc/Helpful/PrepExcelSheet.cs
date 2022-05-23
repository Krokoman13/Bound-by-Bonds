using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;



public class PrepExcelSheet : MonoBehaviour
{
    [SerializeField]string language = "English";
    [SerializeField]List<string> columnNames = new List<string>() {"ENGLISH", "Subtitles", "Subtitle Descriptions", "MenuItem", "MenuDescription"};
    [SerializeField] int lineAmount = 150;

    [ContextMenu("Start")]
    void bro()
    {

        string path = $"{Application.streamingAssetsPath}/Languages/{language}.csv";
        StringBuilder sb = new StringBuilder("");
        StreamWriter writer = new StreamWriter(path, false);

        foreach(string s in columnNames)
        {
            sb.Append($"{s};");
        }

        //sb.Append($"{language.ToUpper()};Subtitles;SubtitleDescriptions;MenuItem;MenuDescription\n");        
        sb.Append("\n");
        for(int i = 1; i < lineAmount; i++)
        {
            sb.Append($"Line #{i}\n");
        }

        writer.WriteLine(sb.ToString());
        writer.Close();

        print("BRO");

    }
}