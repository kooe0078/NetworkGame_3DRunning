using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUIManager : MonoBehaviour
{
    public GameObject IntroPanel;
    public GameObject FadeInOutPanelPrefab;
    public GameObject canvas;
   private GameObject FadeInOutPanel;
    // Start is called before the first frame update
    void Start()
    {
        MakeFadeInOutPanel();
        FadeInOutPanel.GetComponent<FadeInOut>().bFadeIn = true;
        FadeInOutPanel.GetComponent<FadeInOut>().FadeInOutChange();
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(3.0f);
        MakeFadeInOutPanel();
        FadeInOutPanel.GetComponent<FadeInOut>().bFadeOut = true;
        FadeInOutPanel.GetComponent<FadeInOut>().FadeInOutChange();
        yield return new WaitForSeconds(2.0f);
        Destroy(IntroPanel);
    }

    void MakeFadeInOutPanel()
    {
        FadeInOutPanel = Instantiate(FadeInOutPanelPrefab);
        FadeInOutPanel.transform.SetParent(canvas.transform, false);
    }
}
