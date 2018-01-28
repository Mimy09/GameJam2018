using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    private ItemStatistics m_initial = new ItemStatistics();
    private ItemStatistics m_current = new ItemStatistics();
    private ItemStatistics m_upgrade = new ItemStatistics();

    public Color m_color1;
    public Color m_color2;
    public Player m_player;
    public UIChoice m_choice;
    public List<ItemInformation> m_items;

    // Use this for initialization
    void Start ()
    {
        
	}

    void Update()
    {
        // check if displaying
        if (m_choice.Display)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                Hide();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                m_choice.Item2Color = m_color1;
                m_choice.Item1Color = m_color2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                m_choice.Item2Color = m_color2;
                m_choice.Item1Color = m_color1;
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Show();
        }
    }

    public void Hide()
    {
        m_choice.Display = false;
    }
    public void Show()
    {
        m_choice.Display = true;
        int index = Random.Range(0, m_items.Count);
        DisplayUpgrade2(m_items[index]);
    }

    void DisplayUpgrade2(ItemInformation info)
    {
        m_choice.Item2Image = info.sprite;
        DisplayUpgrade2(info.statistics);
    }
    void DisplayUpgrade2(ItemStatistics stats)
    {
        string text = "";
        m_upgrade.maxFuel = Mathf.Round(Random.Range(0, stats.maxFuel) - m_current.maxFuel);
        m_upgrade.maxVelocity = Mathf.RoundToInt(Random.Range(0, stats.maxVelocity) - m_current.maxVelocity);
        m_upgrade.maxPingSpeed = Mathf.RoundToInt(Random.Range(0, stats.maxPingSpeed) - m_current.maxPingSpeed);
        
        if (m_upgrade.maxFuel != 0) text += (m_upgrade.maxFuel < 0 ? "" : "+") + m_upgrade.maxFuel + " fuel\n";
        if (m_upgrade.maxVelocity != 0) text += (m_upgrade.maxVelocity < 0 ? "" : "+") + m_upgrade.maxVelocity + " velocity\n";
        if (m_upgrade.maxPingSpeed != 0) text += (m_upgrade.maxPingSpeed < 0 ? "" : "+") + m_upgrade.maxPingSpeed + " ping\n";

        m_choice.Item2Text = text;
    }
    
    [System.Serializable]
    public class ItemStatistics
    {
        public float maxFuel = 0;
        public float maxVelocity = 0;
        public float maxPingSpeed = 0;
    };
    [System.Serializable]
    public class ItemInformation
    {
        public string name = "";
        public Sprite sprite = null;
        public ItemStatistics statistics = new ItemStatistics();
    }
}