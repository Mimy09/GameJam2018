using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


enum EVENT_PLAYER {
    CLICKED_ON_PLANET,
    SONAR_1, SONAR_2, SONAR_3,
    SONAR_1_LONG, SONAR_2_LONG, SONAR_3_LONG,
    MOVING
}

public class Player : MonoBehaviour {

    // acceleration
    public float m_velocity;
    float m_max_velocity = 3;
    float m_acceleration = 0.1f;
    float m_speed = 20;

    public float m_fual = 600;
    public Slider m_fual_slider;

    public Image m_interaction_image;
    private float m_intaction_timer = 2;
    private bool m_interacting = false;
    public GameObject m_interacting_object;

    // private vars
    private bool m_moving;

    public int m_resources;
    public int m_artifacts;

    void Start () {
        m_interaction_image.transform.parent.gameObject.SetActive(false);
    }
    
    void Update () {
        
        m_fual_slider.value = m_fual / 600;
        
        if (Input.GetKey(KeyCode.E)) {
            if (m_interacting_object != null) {
                m_interacting = true;
                m_interaction_image.transform.parent.gameObject.SetActive(true);

                if (m_intaction_timer > 0) {
                    m_intaction_timer -= Time.deltaTime;
                    m_interaction_image.fillAmount = m_intaction_timer / 2;
                } else {
                    m_interacting = false;
                    m_interaction_image.transform.parent.gameObject.SetActive(false);
                    // interaction code...
                    if (m_interacting_object.tag == "Planet") {
                        m_artifacts += m_interacting_object.GetComponent<Planet>().m_artifacts;
                        m_interacting_object.GetComponent<Planet>().m_artifacts = 0;

                        m_resources += m_interacting_object.GetComponent<Planet>().m_resources;
                        m_interacting_object.GetComponent<Planet>().m_resources = 0;

                        m_fual += m_interacting_object.GetComponent<Planet>().m_fual;
                        m_interacting_object.GetComponent<Planet>().m_fual = 0;
                        if (m_fual > 600) m_fual = 600;
                    }
                }
            }
        } else {
            m_intaction_timer = 2;
            m_interaction_image.transform.parent.gameObject.SetActive(false);
        }

        // Rotate
        if (Input.GetKey(KeyCode.A)) {
            this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * m_speed));
            
        }
        if (Input.GetKey(KeyCode.D)) {
            this.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * m_speed));
        }

        // Acceleration
        if (Input.GetKey(KeyCode.W) && m_fual > 0 && !m_interacting) {
            if (m_velocity < m_max_velocity)
                m_velocity += m_acceleration;
            if (m_fual > 0) {
                m_fual -= Time.deltaTime;
            }
        } else {
            if (Input.GetKey(KeyCode.S) && m_fual > 0 && !m_interacting) {
                m_velocity -= m_acceleration;
                if (m_fual > 0) {
                    m_fual -= Time.deltaTime;
                }
            } else {
                if (m_velocity < 0.06 && m_velocity > 0) {
                    m_velocity = 0;
                }
                if (m_velocity > 0)
                    m_velocity -= Time.deltaTime;
                if (m_velocity < 0)
                    m_velocity += Time.deltaTime;
            }
        }
                        

        if (m_velocity == 0) {
            if (!m_moving) {
                __event<EVENT_PLAYER>.InvokeEvent(this, EVENT_PLAYER.MOVING, true);
                m_moving = true;
            }
        } else {
            if (m_moving) {
                __event<EVENT_PLAYER>.InvokeEvent(this, EVENT_PLAYER.MOVING, false);
                m_interacting = false;
                m_moving = false;
            }
        }

        this.transform.position += transform.up * m_velocity * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D c) {
        if (c.tag == "Planet") {
            m_interacting_object = c.gameObject;
        }
    }

    void OnTriggerExit2D (Collider2D c) {
        //if (c.tag != "Planet") {
            m_interacting_object = null;
        //}
    }
}
