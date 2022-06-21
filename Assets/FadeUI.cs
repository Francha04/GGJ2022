using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUI : MonoBehaviour
{  

    private CanvasGroup canvasGroup;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        StartCoroutine(Fade(false));
    }

    public void FadeOut()
    {
       StartCoroutine(Fade(true));
    }
 
    IEnumerator Fade(bool fadeAway)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                canvasGroup.alpha = i;
                yield return null;
            }
            gameObject.SetActive(false);
        }
        // fade from transparent to opaque
        else
        {
            gameObject.SetActive(true);
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                canvasGroup.alpha = i;
                yield return null;
            }
        }
    }
}
