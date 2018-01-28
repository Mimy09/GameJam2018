using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPostRender : MonoBehaviour {

    public Material _material;
    public float timer;
    public Player ply;

    public void Update () {
        if (timer >= -3.141) {
            timer -= Time.deltaTime*2;
        }
        if (ply.m_canping) {
            timer = 3.141f;
        }

    }

    public void OnRenderImage (RenderTexture source, RenderTexture destination) {
        _material.SetFloat("_Timer", timer);
        _material.SetVector("_Resolution", new Vector4(Screen.width, Screen.height, 0, 0));
        Graphics.Blit(source, destination, _material);
    }
}
