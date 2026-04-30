using UnityEngine;

public class TalkIconController : MonoBehaviour
{
    public GameObject icon;

    void Start()
    {
        if (icon != null)
            icon.SetActive(false);
    }

    public void ShowIcon()
    {
        if (icon != null)
            icon.SetActive(true);
    }

    public void HideIcon()
    {
        if (icon != null)
            icon.SetActive(false);
    }
}
