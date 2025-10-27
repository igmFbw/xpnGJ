using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class teachUI : MonoBehaviour
{
    private int cuIndex;
    [SerializeField] private List<GameObject> pages;
    private void OnEnable()
    {
        cuIndex = 0;
        pages[cuIndex].SetActive(true);
    }
    private void OnDisable()
    {
        pages[cuIndex].SetActive(false);
    }
    public void nextPage()
    {
        if (cuIndex != pages.Count - 1)
        {
            pages[cuIndex].SetActive(false);
            cuIndex++;
            pages[cuIndex].SetActive(true);
        }
    }
    public void prevPage()
    {
        if (cuIndex != 0)
        {
            pages[cuIndex].SetActive(false);
            cuIndex--;
            pages[cuIndex].SetActive(true);
        }
    }
}
