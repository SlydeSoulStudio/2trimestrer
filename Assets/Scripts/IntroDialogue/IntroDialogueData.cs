using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Intro Dialogue")]
public class IntroDialogueData : ScriptableObject
{
    [TextArea(2, 5)]
    public string[] lines;
}