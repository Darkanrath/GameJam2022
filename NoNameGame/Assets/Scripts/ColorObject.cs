using UnityEngine;

public class ColorObject : MonoBehaviour
{
    public Texture coloredTexture;
    public Texture darkTexture;
    public Texture normalMap;

    [Space(20)]
    public Color matColor;

    private Renderer objectMatRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (coloredTexture)
        {
            objectMatRenderer = gameObject.GetComponent<Renderer>();
            objectMatRenderer.material.SetTexture("_MainTex", darkTexture);
            if (normalMap)
            {
                objectMatRenderer.material.SetTexture("_BumpMap", normalMap);
            }
        }
    }

    public void ChangeColor()
    {
        objectMatRenderer.material.SetTexture("_MainTex", coloredTexture);
        if (normalMap)
        {
            objectMatRenderer.material.SetTexture("_BumpMap", normalMap);
        }
    }
}
