using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIRadial : MonoBehaviour
{
    private UnityEngine.UI.Image m_image;
    public float Value { get { return m_image.fillAmount; } set { m_image.fillAmount = value; } }

    // Use this for initialization
    void Start ()
    {
        // get slider from 
        m_image = GetComponentInChildren<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}