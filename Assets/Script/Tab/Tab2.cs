using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab2 : TabManager
{
    Dropdown drp1, drp2; //jaum, moum
    string strJaum, strMoum; //for combine
    string result;


    // Start is called before the first frame update
    public void Start()
    {
        BtnRoot = transform.GetChild(0);


    }

    private void OnDisable()
    {
        base.OnDisable();
    }

    void EventLIstener(Dropdown drp)
    {
        /*
        //get DB data by tableName
        words = SqliteMgr.Instance.ReadData("select value from " + drp.options[drp.value].text, 1); //get theme

        if (drp.value == 4) //show sub theme when combine selected
        {
            drpSubTheme.gameObject.SetActive(true);
            List<string> abc = SqliteMgr.Instance.ReadData("select value from " + drp.options[drp.value].text, 1); //get sub theme


        }
        else
            drpSubTheme.gameObject.SetActive(false);

        if (words == null) return;

        wordIdx = 0;
        txtView.text = words[wordIdx];

        curTheme = drp.options[drp.value].text;

        */
    }
    void EventLIstener(Button btn)
    {
        /*
        switch (btn.name.Split('_')[1].ToLower())
        {
            case "play":
                string voiceIdx = SqliteMgr.Instance.ReadData("select key from " + curTheme + " where value == '" + txtView.text + "'");
                //select key from HangulJaum where value == '¤¡'

                AudioSource audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
                AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + curTheme + "/" + voiceIdx);
                audioSource.PlayOneShot(audioClip);

                break;
        }*/
    }

}