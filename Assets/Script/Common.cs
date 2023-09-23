using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Common : MonoSingleton <Common>
{
    /// <summary>
    /// print Log for DEV
    /// </summary>
    public void PrintLog(char type, string msg1, object msg2 = null, string color = null)
    {
        if (CONST_VALUE.ISDEV)
        {
            bool isLegacy = false;
            if (string.IsNullOrEmpty(color))
                isLegacy = true;

            string str;;
            if(msg2 != null)
                str = msg1 + " : " + msg2.ToString();
            else
                str = msg1;

            switch (type)
            {
                case 'w':
                    //Debug.LogWarning(str);
                    color = isLegacy ? "yellow" : color;
                    Debug.LogWarningFormat("<color={0}>{1}</color>", color, str);
                    break;
                case 'e':
                    //Debug.LogError(str);
                    color = isLegacy ? "red" : color;
                    Debug.LogErrorFormat("<color={0}>{1}</color>", color, str);
                    break;
                default:
                    //Debug.Log(str);
                    color = isLegacy ? "white" : color;
                    Debug.LogFormat("<color={0}>{1}</color>", color, str);
                    break;
            }
        }
    }

    public List<string> ReadStream(string fileName)
    {
        StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/" + fileName);
        List<string> rtnValue = new List<string>();
        string rowItem;

        while((rowItem = sr.ReadLine()) != null)
            rtnValue.Add(rowItem);

        return rtnValue;
    }

    /// <summary>
    /// Image component
    /// </summary>
    /// <param name="img">Image</param>
    /// <param name="color">Color RGBA</param>
    public void SetColor(Image img, Color color)
    {
        if(color.r > 1f || color.b > 1f || color.b > 1f)
            img.color = new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
        else
            img.color = color;
    }
    /// <summary>
    /// RawImage component
    /// </summary>
    /// <param name="img">Raw Image</param>
    /// <param name="color">Color RGBA</param>
    public void SetColor(RawImage img, Color color)
    {
        if (color.r > 1f || color.b > 1f || color.b > 1f)
            img.color = new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
        else
            img.color = color;
    }
    /// <summary>
    /// Text component
    /// </summary>
    /// <param name="txt">Text</param>
    /// <param name="color">Color RGBA</param>
    public void SetColor(Text txt, Color color)
    {
        if (color.r > 1f || color.b > 1f || color.b > 1f)
            txt.color = new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
        else
            txt.color = color;
    }
    /// <summary>
    /// Button component
    /// </summary>
    /// <param name="btn">Button</param>/.;
    /// <param name="color">Color RGBA</param>
    public void SetColor(Button btn, Color color)
    {
        if (color.r > 1f || color.b > 1f || color.b > 1f)
            btn.gameObject.GetComponent<Image>().color = new Color(color.r / 255, color.g / 255, color.b / 255, color.a / 255);
        else
            btn.gameObject.GetComponent<Image>().color = color;
    }

    public void SetImage(Image img, string resourcePath)
    {
        if (img == null) return;
        if (string.IsNullOrEmpty(resourcePath)) return;

        img.sprite = Resources.Load(resourcePath, typeof (Sprite)) as Sprite;
    }
    public void SetImage(Button btn, string resourcePath)
    {
        if (btn == null) return;
        if (string.IsNullOrEmpty(resourcePath)) return;

        btn.image.sprite = Resources.Load(resourcePath, typeof(Sprite)) as Sprite;
    }

    public void ToastMessage(int type, string msg, Color color)
    {
        //GameObject ab = Instantiate(Resources.Load("")) as GameObject;
    }

    public GameObject LoadRsourceObj(string objPath, Transform parent = null, string name = null)
    {
        GameObject newObj = Instantiate(Resources.Load(objPath) as GameObject, parent);

        if (parent != null)
            newObj.transform.SetParent(parent);

        newObj.transform.localScale = Vector3.one;
        newObj.transform.localPosition = Vector3.zero;

        if (!string.IsNullOrEmpty(name))
            newObj.name = name;

        return newObj;
    }

    public GameObject LoadRsourceObj(GameObject refObj, Transform parent = null, string name = null)
    {
        GameObject newObj = Instantiate(refObj, parent);

        if (parent != null)
            newObj.transform.SetParent(parent);

        newObj.transform.localScale = Vector3.one;
        newObj.transform.localPosition = Vector3.zero;

        if (!string.IsNullOrEmpty(name))
            newObj.name = name;

        return newObj;
    }

    public void RectPosition(GameObject target, Vector3 position)
    {
        target.GetComponent<RectTransform>().anchoredPosition3D = position;
    }
    public void RectSize(GameObject target, Vector3 size)
    {
        target.GetComponent<RectTransform>().sizeDelta = size;
    }

    public void AppQuit()
    {
        System.GC.Collect();
        PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}