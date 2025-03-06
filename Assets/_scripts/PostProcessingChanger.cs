using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingChanger : MonoBehaviour
{
    private Volume v;
    private Vignette vg;

    public Color defaultVignette;
    public Color flashlightVignette;
    public Color hellVignette;

    // Start is called before the first frame update
    void Start()
    {
        v = GetComponent<Volume>();
        v.profile.TryGet(out vg);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetVignetteIntensity(float intensity)
    {
        vg.intensity.value = intensity;
    }

    public void SetVignetteColour(Color colour)
    {
        vg.color.value = colour;
    }
}
