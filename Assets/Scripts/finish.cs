using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finish : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ReloadAfterTime(12.7f));
        Debug.Log("TESTTTT");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReloadAfterTime(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Level1End");


    }
}
