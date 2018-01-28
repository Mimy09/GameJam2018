using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChoice : MonoBehaviour
{
    // Required variables
    private Animator m_animator;
    private UnityEngine.UI.Text[] m_texts = null;
    private UnityEngine.UI.Image[] m_images = null;
    private static int UI_CHOICE_TITLE = 0;
    private static int UI_CHOICE_ITEM_1_KEY = 2;
    private static int UI_CHOICE_ITEM_2_KEY = 4;
    private static int UI_CHOICE_ITEM_1_TEXT = 1;
    private static int UI_CHOICE_ITEM_2_TEXT = 3;
    private static int UI_CHOICE_ITEM_1_COLOR = 1;
    private static int UI_CHOICE_ITEM_2_COLOR = 4;
    private static int UI_CHOICE_ITEM_1_IMAGE = 2;
    private static int UI_CHOICE_ITEM_2_IMAGE = 5;
    private static int UI_CHOICE_ACTION_1_KEY = 6;
    private static int UI_CHOICE_ACTION_1_TEXT = 5;

    // Getters and Setters
    public string Tile { get { return m_texts[UI_CHOICE_TITLE].text; } set { m_texts[UI_CHOICE_TITLE].text = value; } }
    public bool Display { get { return m_animator.GetBool("Display"); } set { m_animator.SetBool("Display", value); } }
    public Color Item1Color { get { return m_images[UI_CHOICE_ITEM_1_COLOR].color; } set { m_images[UI_CHOICE_ITEM_1_COLOR].color = value; } }
    public Color Item2Color { get { return m_images[UI_CHOICE_ITEM_2_COLOR].color; } set { m_images[UI_CHOICE_ITEM_2_COLOR].color = value; } }
    public string Item1Text { get { return m_texts[UI_CHOICE_ITEM_1_TEXT].text; } set { m_texts[UI_CHOICE_ITEM_1_TEXT].text = value; } }
    public string Item2Text { get { return m_texts[UI_CHOICE_ITEM_2_TEXT].text; } set { m_texts[UI_CHOICE_ITEM_2_TEXT].text = value; } }
    public Sprite Item1Image { set { m_images[UI_CHOICE_ITEM_1_IMAGE].sprite = value; } }
    public Sprite Item2Image { set { m_images[UI_CHOICE_ITEM_2_IMAGE].sprite = value; } }
    public string Action1Key { get { return m_texts[UI_CHOICE_ITEM_1_KEY].text; } set { m_texts[UI_CHOICE_ITEM_1_KEY].text = value; } }
    public string Action2Key { get { return m_texts[UI_CHOICE_ITEM_2_KEY].text; } set { m_texts[UI_CHOICE_ITEM_2_KEY].text = value; } }
    public string Action3Key { get { return m_texts[UI_CHOICE_ACTION_1_KEY].text; } set { m_texts[UI_CHOICE_ACTION_1_KEY].text = value; } }
    public string Action3Text { get { return m_texts[UI_CHOICE_ACTION_1_TEXT].text; } set { m_texts[UI_CHOICE_ACTION_1_TEXT].text = value; } }

    // Use this for initialization
    void Start ()
    {
        // fetch components
        m_texts = GetComponentsInChildren<UnityEngine.UI.Text>();
        m_images = GetComponentsInChildren<UnityEngine.UI.Image>();
        m_animator = GetComponent<Animator>();

        Debug.Log(m_texts.Length);
	}
}