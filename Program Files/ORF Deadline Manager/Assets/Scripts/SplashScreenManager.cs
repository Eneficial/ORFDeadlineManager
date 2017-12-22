using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SplashScreenManager : MonoBehaviour {

    public float fadeInDuration = 1f, fadeOutDuration = 1f, showDuration = 5f;

    ScrollbarAutoScroller scroller;
    ScrollbarAutoScroller.ScrollMode buffer;
    bool executing = false;
	
    //Controls splash screen display length, and message customization
	void Start () {
        scroller = FindObjectOfType<ScrollbarAutoScroller>();
        if (scroller == null)
        {
            Destroy(this);
        }
        foreach (Graphic cv in GetComponentsInChildren<Graphic>())
        {
            cv.CrossFadeAlpha(0, 0, false);
        }

        Text splashText = GetComponentInChildren<Text>();
        try
        {
            using (StreamReader reader = new StreamReader(Application.dataPath + "/StreamingAssets/splashMessage.txt"))
            {
                splashText.text = reader.ReadToEnd();
            }
        }
        catch
        {
            Debug.LogWarning("Could not read from file! (does file exist?)");
            splashText.text = "Sample Announcement";
        }
    }
	
	void Update () {

        if (scroller.scrollbar.value <= (1 - scroller.percentageDone) && scroller.speed > 0 && !executing)
        {
            executing = true;
            StartCoroutine(Toggle());
            
        }
    }

    //Controls specifics of splash screen:
    //How long the transition fade is
    //How long does the transition occur after the list reaches the bottom
    //How long the splash screen is displayed for
    IEnumerator Toggle()
    {
        Debug.Log(buffer);
        buffer = scroller.mode;
        scroller.mode = ScrollbarAutoScroller.ScrollMode.none;
        yield return new WaitForSeconds(3);
        foreach (Graphic cv in GetComponentsInChildren<Graphic>())
        {
            cv.CrossFadeAlpha(1, fadeInDuration, true);
        }

        yield return new WaitForSeconds(fadeInDuration + showDuration);

        foreach (Graphic cv in GetComponentsInChildren<Graphic>())
        {
            cv.CrossFadeAlpha(0, fadeOutDuration, true);
        }
       
        scroller.mode = buffer;
        scroller.ReachedEnd(); 
       
        executing = false;
        
    }
}
