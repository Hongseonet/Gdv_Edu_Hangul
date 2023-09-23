using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField]
    Transform BtnRoot;

    // Start is called before the first frame update
    void Start()
    {
        //TEST();

        //StartCoroutine(AudioPlay(NextScene));
        
        foreach(Transform btns in BtnRoot)
        {
            Button btn = btns.GetComponent<Button>();
            btn.onClick.AddListener(() => BtnEvent(btn));
        }
    }

    IEnumerator AudioPlay(Action action)
    {
        string[] audioFile = new string[] { "intro_hello_1", "intro_hello_2" };
        /*
        AudioSource audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + audioFile[0]);
        audioSource.PlayOneShot(audioClip);
        
        yield return new WaitForSecondsRealtime(audioClip.length + 2f);
        */

        yield return new WaitForSecondsRealtime(2f);

        if (action != null)
            action();
    }

    void BtnEvent(Button btn)
    {
        Invoke("NextScene", 2f);
    }

    void NextScene()
    {
        SceneManager.LoadScene("Main");
    }


    public void TEST()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        //do what you want with the IP here... add it to a list, just get the first and break out. Whatever.
                        Debug.Log(ip.Address.ToString());
                    }
                }
            }
        }
    }
}