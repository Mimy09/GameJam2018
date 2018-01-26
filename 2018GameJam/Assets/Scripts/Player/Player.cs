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
    public float m_speed;
    public float m_acceleration;

    // rotation
    public float m_rot_velocity;
    public float m_rot_max_velocity;
    public float m_rot_acceleration;

    // private vars
    private bool m_moving;
    

    void Awake () {

    }
    void Update () {

        // Rotate
        if (Input.GetKey(KeyCode.A)) {
            if (m_rot_velocity < (m_rot_max_velocity / 100))
                m_rot_velocity += (m_rot_acceleration * m_speed) / 100;

            
        }
        if (Input.GetKey(KeyCode.D)) {
            if (m_rot_velocity > -(m_rot_max_velocity / 100))
                m_rot_velocity -= (m_rot_acceleration * m_speed) / 100;
        }

        // Acceleration
        if (Input.GetKey(KeyCode.W)) {
            if (m_velocity < (m_max_velocity/100))
                m_velocity += (m_acceleration * m_speed) / 100;
        }
        if (Input.GetKey(KeyCode.S)) {
            if (m_velocity > 0)
                m_velocity -= (m_acceleration * m_speed) / 100;
        }                   

        if (m_velocity == 0) {
            if (!m_moving) {
                m_moving = true;
            }
        } else {
            if (m_moving) {
                m_moving = false;
            }
        }

        if (m_velocity > 0) {
            this.transform.position += transform.up * m_velocity;
        }
        this.transform.Rotate(new Vector3(0, 0, Time.deltaTime * m_rot_velocity));
    }
}
