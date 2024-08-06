using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] pages;
    public Button nextBtn;
    public Button prevBtn;

    private GameObject activePage;
    private int page;



    public int Page
    {
        get { return page; }
        set
        {
            page = value;
            if (Page < 0)
                Page = pages.Length - 1;
            else if (Page > pages.Length - 1)
                Page = 0;
            SetActivePage();
        }
    }

    void Start()
    {
        if (pages.Length == 0)
            return;

        activePage = pages[Page];
        SetActivePage();
    }


    public void Prev()
    {
        Debug.Log("Prev");
        Page--;
    }

    public void Next()
    {
        Debug.Log("Next");
        Page++;
    }

    private void SetActivePage()
    {
        Debug.Log("SetActivePage");

        activePage.SetActive(false);
        activePage = pages[Page];
        activePage.SetActive(true);
    }
}
