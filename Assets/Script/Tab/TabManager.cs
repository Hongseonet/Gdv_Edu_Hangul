using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TabManager : MonoBehaviour
{
    protected Transform btnRoot;


    public void OnDisable()
    {
        
    }

    public void OnEnable()
    {
        
    }

    public Transform BtnRoot
    {
        set => btnRoot = value;
        get => btnRoot;
    }
}