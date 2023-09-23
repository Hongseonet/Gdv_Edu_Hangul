using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] bool isDev;
    [SerializeField] Transform btnRoot, tabRoot;
    bool isCloseApp;
    Button curTab;
    int curTabIdx, pravcurTabIdx;


    // Start is called before the first frame update
    void Awake()
    {
        CONST_VALUE.ISDEV = isDev;


        /*
        //test
        var videoPlayer = Camera.main.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.url = Application.streamingAssetsPath + "/carrie_kieuk.mp4";

        videoPlayer.Play();
        */


        //test

        curTabIdx = 0;
        
        //add tab btn
        tabRoot.GetChild(curTabIdx).gameObject.SetActive(true); //open default tab

        curTab = btnRoot.GetChild(curTabIdx).GetComponent<Button>();

        for (int i=0; i< btnRoot.childCount; i++)
        {
            Button btn = btnRoot.GetChild(i).GetComponent<Button>();
            if (btn == null) continue;
            btn.onClick.AddListener(delegate { EventLIstener(btn); });
        }

        TabEvent(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isCloseApp)
                StartCoroutine(CloseTimer());
            else
                Application.Quit();
        }
    }

    void EventLIstener(Button btn)
    {
        switch (btn.name.Split('_')[1].ToLower())
        {
            case "close": //app close
                isCloseApp = !isCloseApp; //false -> true
                if (!isCloseApp) //on false
                    Application.Quit();
                break;
            case "tab":
                TabEvent(false);

                if (int.TryParse(btn.name.Split('_')[2], out curTabIdx)){
                    curTab = btnRoot.GetChild(curTabIdx).GetComponent<Button>();
                    TabEvent(true);
                }
                break;
        }
    }

    void TabEvent(bool isSet)
    {
        Color color;

        if(isSet)
            color = new Color(214f, 255f, 0f, 255f); //set remark
        else
            color = new Color(255f, 255f, 255f, 255f); //set white
        
        Common.Instance.SetColor(curTab.GetComponent<Image>(), color); //set white
        tabRoot.GetChild(curTabIdx).gameObject.SetActive(isSet);
    }

    IEnumerator CloseTimer()
    {
        isCloseApp = true;
        yield return new WaitForSecondsRealtime(2f);
        isCloseApp = false;
    }
}