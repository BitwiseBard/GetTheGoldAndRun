using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {
    public Image panel;

    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private float hangLength;

    void Start() {
        Fade(true);
    }

    public void Fade(bool skipIn) {
        StartCoroutine(FadeCo(skipIn));
    }

    IEnumerator FadeCo(bool skipIn) {
        if(!skipIn) {
            while(panel.color.a < 1) {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, (panel.color.a + (fadeSpeed * Time.deltaTime)));
                yield return null;  
            }
        }
        else {
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 1f);
        }

        yield return new WaitForSeconds(hangLength); 

        while(panel.color.a > 0) {
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, (panel.color.a - (fadeSpeed * Time.deltaTime)));
            yield return null;  
        }
    }
}
