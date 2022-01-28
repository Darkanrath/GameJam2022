using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    public Texture coloredTexture;
    public Texture darkTexture;
    public Texture normalMap;

    private Renderer objectMatRenderer;

    // Start is called before the first frame update
    void Start()
    {
        objectMatRenderer = gameObject.GetComponent<Renderer>();
        objectMatRenderer.material.SetTexture("_MainTex", darkTexture);
        objectMatRenderer.material.SetTexture("_BumpMap", normalMap);
    }

    public void ChangeColor()
    {
        objectMatRenderer.material.SetTexture("_MainTex", coloredTexture);
        objectMatRenderer.material.SetTexture("_BumpMap", normalMap);
    }
}