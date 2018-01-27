using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowResize : MonoBehaviour {

    public RenderTexture _texture;

    void Start () {
        _texture.width = Screen.width;
        _texture.height = Screen.height;
    }
}
