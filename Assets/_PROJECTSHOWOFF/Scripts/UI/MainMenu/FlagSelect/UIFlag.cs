using UnityEngine.UI;
using UnityEngine;

public class UIFlag : MonoBehaviour
{
    [SerializeField]string language = "English";

    Image myImage = null;
    [SerializeField]Color unselectedAlpha = new Color(1,1,1, 0.1f);
    [SerializeField]Color selectedAlpha = new Color(1,1,1, 1f);


    public void SetAsSelected()
    {
        myImage.raycastTarget = false;
        myImage.color = selectedAlpha;
        PlayerPrefs.SetString("Language", language);
    }

    public void SetAsNotSelected()
    {
        myImage.raycastTarget = true;
        myImage.color = unselectedAlpha;
    }
    

    void Awake()
    {
        TryGetComponent<Image>(out myImage);       
    }
}
