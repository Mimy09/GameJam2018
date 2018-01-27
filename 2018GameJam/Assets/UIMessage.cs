using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMessage : MonoBehaviour
{
    private Animator m_animator;
    private UnityEngine.UI.Text m_title = null;
    private UnityEngine.UI.Text m_message = null;
    private UnityEngine.UI.Text[] m_actionTXT = new UnityEngine.UI.Text[2];
    private UnityEngine.UI.Text[] m_actionKEY = new UnityEngine.UI.Text[2];
    private UnityEngine.UI.Button[] m_actionBTN = new UnityEngine.UI.Button[2];

    public string Title { get { return m_title.text; } set { m_title.text = value; } }
    public string Message { get { return m_message.text; } set { m_message.text = value; } }
    public string Action1Key { get { return m_actionKEY[0].text; } set { m_actionKEY[0].text = value; } }
    public string Action2Key { get { return m_actionKEY[1].text; } set { m_actionKEY[1].text = value; } }
    public string Action1Text { get { return m_actionTXT[0].text; } set { m_actionTXT[0].text = value; } }
    public string Action2Text { get { return m_actionTXT[1].text; } set { m_actionTXT[1].text = value; } }
    public bool isAction1ButtonVisible { get { return m_actionBTN[0].gameObject.activeInHierarchy; } set { m_actionBTN[0].gameObject.SetActive(value); } }
    public bool isAction2ButtonVisible { get { return m_actionBTN[1].gameObject.activeInHierarchy; } set { m_actionBTN[1].gameObject.SetActive(value); } }

    // Use this for initialization
    void Start ()
    {
        // fetch components
        m_animator = GetComponent<Animator>();
        UnityEngine.UI.Text[] componentsTXT = GetComponentsInChildren<UnityEngine.UI.Text>();
        UnityEngine.UI.Button[] componentsBTN = GetComponentsInChildren<UnityEngine.UI.Button>();
        // store components
        m_title = componentsTXT[0];
        m_message = componentsTXT[1];
        m_actionTXT[0] = componentsTXT[2];
        m_actionKEY[0] = componentsTXT[3];
        m_actionTXT[1] = componentsTXT[4];
        m_actionKEY[1] = componentsTXT[5];
        m_actionBTN[0] = componentsBTN[0];
        m_actionBTN[1] = componentsBTN[1];
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void FadeIn()
    {
        // start fade in animation
        m_animator.SetBool("Display", true);
    }

    public void FadeOut()
    {
        // start fade out animation
        m_animator.SetBool("Display", false);
    }
}