using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleSender : MonoBehaviour
{
    [SerializeField] Sibling target;
    [SerializeField] float appearTime = 1;
    [SerializeField] string speaker = "Developer";
    [SerializeField] string text = "This is a test message.";

    public void SendSubtitle()
    {
        Subtitle subtitle = new Subtitle();
        subtitle.appearTime = appearTime;
        subtitle.speaker = speaker;
        subtitle.text = text;

        switch (target)
        {
            case Sibling.Dana:
                DanaSubtitlesHandler.instance.AddSubtitleToQueue(subtitle);
                return;

            case Sibling.Denys:
                DenysSubtitlesHandeler.instance.AddSubtitleToQueue(subtitle);
                return;
        }
    }
}
