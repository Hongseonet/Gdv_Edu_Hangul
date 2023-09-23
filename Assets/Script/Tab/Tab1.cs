using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Tab1 : TabManager
{
    [SerializeField] Dropdown drpTheme, drpSubTheme;
    [SerializeField] Text txtView;
    [SerializeField] Image imgDraw;

    Dictionary<string, string> words;
    List<string> drpOption;

    int wordIdx;
    

    string curTheme;


    // Start is called before the first frame update
    public void Start()
    {
        wordIdx = 0;
        SqliteMgr.Instance.Init("hangul.db");

        words = SqliteMgr.Instance.ReadDicData("select eng, kor from Language"); //get language code
        drpTheme.onValueChanged.AddListener(delegate { EventLIstener(drpTheme); });
        BtnRoot = transform.GetChild(0);

        for (int i = 0; i < btnRoot.childCount; i++)
        {
            if (btnRoot.GetChild(i).GetComponent<Button>() != null)
            {
                Button btn = btnRoot.GetChild(i).GetComponent<Button>();
                btn.onClick.AddListener(() => EventLIstener(btn));
            }
        }

        //drpTheme.AddOptions(Enum.GetNames(typeof(theme)).ToList());
        drpOption = words.Values.ToList();
        drpTheme.AddOptions(drpOption);
        txtView.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void OnEnable()
    {
        base.OnEnable();
    }

    private void OnDisable()
    {
        base.OnDisable();
    }

    void EventLIstener(Dropdown drp)
    {
        if (drp.gameObject.name.Contains("Sub")) //sub theme
        {
            Debug.Log("dfa");
            //words = SqliteMgr.Instance.ReadData("select key, value from HangulCombine where option = " + drp.options[drp.value].text, 2); //get theme

            //drpSubTheme.AddOptions(words);
        }
        else
        {
            if (drp.options[drp.value].text.ToLower().Equals("none")) return;

            wordIdx = 0;

            //get DB data by tableName
            drpOption = SqliteMgr.Instance.ReadData("select key, value from " + drp.options[drp.value].text, 2); //get theme
            curTheme = drp.options[drp.value].text;

            if (drp.value == 4) //show sub theme when combine selected
            {
                drpSubTheme.gameObject.SetActive(true);
                Common.Instance.SetImage(imgDraw, "Images/empty");

                List<string> combineJaum = SqliteMgr.Instance.ReadData("select value from HangulJaum", 1); //get sub theme
                drpSubTheme.AddOptions(combineJaum);

                drpSubTheme.onValueChanged.AddListener(delegate { EventLIstener(drpSubTheme); });
            }
            else //jaum, moum
            {
                drpSubTheme.gameObject.SetActive(false);
                Common.Instance.SetImage(imgDraw, "Images/empty");

                SetSubTheme();
            }

            if (words == null) return;
            txtView.text = drpOption[wordIdx].Split('\\')[1]; //giyeok \\ ¤¡
            
        }
    }
    void EventLIstener(Button btn)
    {
        switch (btn.name.Split('_')[1].ToLower())
        {
            case "next":
                ++wordIdx;
                if (wordIdx < words.Count)
                {
                    txtView.text = drpOption[wordIdx].Split('\\')[1];
                    SetSubTheme();
                }
                else
                    wordIdx = words.Count;
                break;
            case "prev":
                --wordIdx;
                if (wordIdx > -1)
                {
                    txtView.text = drpOption[wordIdx].Split('\\')[1];
                    SetSubTheme();
                }
                else
                    wordIdx = 0;
                break;
            case "rnd":
                wordIdx = (int)UnityEngine.Random.Range(0f, words.Count);
                txtView.text = drpOption[wordIdx].Split('\\')[1];
                SetSubTheme();
                break;
            case "play":
                if (string.IsNullOrEmpty(curTheme) || string.IsNullOrEmpty(txtView.text))
                    return;

                string voiceIdx = SqliteMgr.Instance.ReadData("select key from " + curTheme + " where value == '" + txtView.text + "'");
                //select key from HangulJaum where value == '¤¡'

                AudioSource audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
                AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + curTheme + "/" + voiceIdx);
                audioSource.PlayOneShot(audioClip);

                break;
            case "value":

                break;
        }
    }

    void SetSubTheme()
    {
        string subTheme;
        if (curTheme.ToLower().Contains("jaum"))
            subTheme = "j_";
        else if (curTheme.ToLower().Contains("moum"))
            subTheme = "m_";
        else
            subTheme = null;

        if (!string.IsNullOrEmpty(subTheme))
        {
            Common.Instance.SetImage(imgDraw, "Images/Draw/" + subTheme + drpOption[wordIdx].Split('\\')[0]);
            if(imgDraw.sprite == null)
                Common.Instance.SetImage(imgDraw, "Images/empty");
        }
            
    }
}