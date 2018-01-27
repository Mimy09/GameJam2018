using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public int max_resources;
    public int min_resources;
    public int m_resources;

    public int max_artifacts;
    public int min_artifacts;
    public int m_artifacts;

    public int max_fual;
    public int min_fual;
    public int m_fual;

    void Start () {
        m_resources = Random.Range(min_resources, max_resources);
        m_artifacts = Random.Range(min_artifacts, max_artifacts);
        m_fual = Random.Range(min_fual, max_fual);
    }
}
