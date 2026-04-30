using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
public class CutSceneController : MonoBehaviour
{

    public PlayableDirector director;
    bool playing = false;
    public PlayableAsset introTimeline;
    public PlayableAsset endingTimeline;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            if (!playing)
            {
                director.playableAsset = introTimeline;
                PlaycutScene();
                playing = true;
            }
         else
            {
                StopcutScene();
                playing = false;
            }
        }

        if (director.state !=PlayState.Playing)
        {
            //Debug.Log("Cutscene finished");
        }

    }

    public void PlaycutScene()
    {
        director.Play();
    }

    public void StopcutScene()
    {
        director.Stop();
    }


}
