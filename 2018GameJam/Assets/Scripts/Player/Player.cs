using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EVENT_PLAYER {
    CLICKED_ON_PLANET,
    SONAR_1, SONAR_2, SONAR_3,
    SONAR_1_LONG, SONAR_2_LONG, SONAR_3_LONG,
    MOVING
}

public class Player : MonoBehaviour {

    // acceleration
    public float m_velocity;
    public float m_max_velocity;
    public float m_acceleration;
    public float m_speed;

    // rotation
    public float m_rot_velocity;
    public float m_rot_max_velocity;
    public float m_rot_acceleration;

    // private vars
    private bool m_moving;
    
    
    void Update () {

        // Rotate
        if (Input.GetKey(KeyCode.A)) {
            this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * m_speed));
        }
        if (Input.GetKey(KeyCode.D)) {
            this.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * m_speed));
        }

        // Acceleration
        if (Input.GetKey(KeyCode.W)) {
            if (m_velocity < m_max_velocity)
                m_velocity += m_acceleration;
        } else {
            if (Input.GetKey(KeyCode.S)) {
                m_velocity -= m_acceleration;
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
                m_moving = false;
            }
        }

        this.transform.position += transform.up * m_velocity * Time.deltaTime;
        this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * m_rot_velocity));
    }

    void OnDrawGizmos () {
        
    }
}
