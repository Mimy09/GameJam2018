using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMessage : MonoBehaviour
{
    private UnityEngine.UI.Text m_title;
    private UnityEngine.UI.Text m_message;
    public string Title { get { return m_title.text; } set { m_title.text = value; } }
    public string Message { get { return m_message.text; } set { m_message.text = value; } }

    // Use this for initialization
    void Start ()
    {
        UnityEngine.UI.Text[] components = GetComponentsInChildren<UnityEngine.UI.Text>();
        m_title = components[0];
        m_message = components[1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}