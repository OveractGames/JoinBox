using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryManager : Singleton<LibraryManager>
{
    public ScrollRect rect;
    public LeanWindow lean;
    public int index = 1;
    public ScreenShootLoader[] childrens;
    public RectTransform lastChild;
    public GameObject no_image;
    void Start()
    {
        childrens = GetComponentsInChildren<ScreenShootLoader>(true);
        Verify();
    }
    public void Verify()
    {
        for (int i = 1; i < childrens.Length; i++)
        {
            if (childrens[i].gameObject.activeSelf)
                continue;
            var path = Application.persistentDataPath + "/Screenshot_" + i + ".png";
            if (System.IO.File.Exists(path))
            {
                transform.GetChild(i - 1).gameObject.SetActive(true);
                lastChild = transform.GetChild(i - 1).GetComponent<RectTransform>();
            }
        }
        no_image.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }

    public bool isLibraryDone()
    {
        return transform.GetComponentsInChildren<ScreenShootLoader>(false).Length > 6; ;
    }
    public void DisableAll()
    {
        foreach (ScreenShootLoader child in childrens)
            child.gameObject.SetActive(false);
    }
    public void setScroll()
    {
        if (lastChild != null)
            StartCoroutine(ScrollToTop());
    }
    IEnumerator ScrollToTop()
    {
        yield return new WaitForSeconds(0.5f);
        ScrollRectCenterItem.Instance.SnapTo(lastChild);
    }
}
