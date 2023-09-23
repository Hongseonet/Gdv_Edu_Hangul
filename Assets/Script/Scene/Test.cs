using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using TMPro;

public class Test : MonoBehaviour
{
    [SerializeField]
    Transform canvas, btnRoot;

    [SerializeField]
    Transform tabEleRoot;

    [SerializeField]
    RawImage rawImg;

    VideoPlayer videoPlayer;

    Dictionary<string, string> dicCategories;


    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = rawImg.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.targetTexture = rawImg.GetComponent<RawImage>().texture as RenderTexture;

        foreach(Transform child in btnRoot)
        {
            if (!child.gameObject.activeSelf) continue;

            Button btn = child.GetComponent<Button>();
            if (btn == null) continue;
            btn.onClick.AddListener(delegate { BtnEvent(btn); });
        }

        SqliteMgr.Instance.Init("hangul.db");
        dicCategories = SqliteMgr.Instance.ReadDicData("select key, value from HangulJaum;");


        foreach(KeyValuePair<string, string> ele in dicCategories)
        {
            GameObject eleCpy = Instantiate(tabEleRoot.GetChild(0).gameObject, tabEleRoot);
            eleCpy.name = "Btn_Ele_" + ele.Key;
            eleCpy.SetActive(true);
            eleCpy.GetComponentInChildren<TextMeshProUGUI>().text = ele.Value;


            Button btn = eleCpy.GetComponent<Button>();
            btn.onClick.AddListener(() => BtnEvent(btn));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //slider.value = videoPlayer.time;
    }

    void DrpEvent(Dropdown drp)
    {
        //Debug.Log("dd " + drp.options[drp.value].text);

        try
        {
            string jaumCode = dicCategories.FindFirstKeyByValue(drp.options[drp.value].text);
            if (jaumCode.Equals("=====")) return;

            videoPlayer.url = Application.streamingAssetsPath + "/carrie_" + jaumCode + ".mp4";
            videoPlayer.Play();
        }
        catch(Exception e)
        {
            Debug.LogError("err : " + e.Message);
        }
    }
    

    void BtnEvent(Button btn)
    {
        Debug.Log("dd " + btn.name);

        switch (btn.name.Split('_')[1].ToLower())
        {
            case "play":
                if (string.IsNullOrEmpty(videoPlayer.url))
                    return;
                videoPlayer.Play();
                break;
            case "stop":
                videoPlayer.Stop();
                break;
            case "playx1":
                videoPlayer.playbackSpeed = 1f;
                break;
            case "playx1-2":
                videoPlayer.playbackSpeed = 0.2f;
                break;
            case "playx1-5":
                videoPlayer.playbackSpeed = 0.25f;
                break;
            case "playx2":
                videoPlayer.playbackSpeed = 0.5f;
                break;
            case "quit":
                Application.Quit();
                break;
            case "ele":

                videoPlayer.url = Application.streamingAssetsPath + "/carrie_" + btn.name.Split('_')[2] + ".mp4";
                videoPlayer.Play();
                break;
        }
    }
}

public static class Extension
{
    public static K FindFirstKeyByValue<K, V>(this Dictionary<K, V> dict, V val)
    {
        return dict.FirstOrDefault(entry =>
            EqualityComparer<V>.Default.Equals(entry.Value, val)).Key;
    }
}