using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public Animator animator;
    public GameObject FadeInOutPanelPrefab;
    public bool bFadeIn = false;
    public bool bFadeOut = false;
    public bool bFadeOutEndFadeIn = false;
    // Start is called before the first frame update
    public void FadeInOutChange()
    {
        if (bFadeIn)
        {
            animator.SetTrigger("FadeIn");
        }
           
        if(bFadeOut)
        {
            animator.SetTrigger("FadeOut");
        }
    }
    public void FadeInEnd()
    {
        bFadeIn = false;        
        Destroy(gameObject);
    }
    public void FadeOutEnd()
    {
        if(!bFadeOutEndFadeIn)
        {
            bFadeOut = false;
            GameObject FadeInOutPanel = Instantiate(FadeInOutPanelPrefab);
            FadeInOutPanel.transform.SetParent(GameObject.Find("Canvas").transform, false);
            FadeInOutPanel.GetComponent<FadeInOut>().bFadeIn = true;
            FadeInOutPanel.GetComponent<FadeInOut>().FadeInOutChange();
            Destroy(gameObject);
        }    
    }
}
