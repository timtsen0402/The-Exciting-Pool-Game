using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static AudioManager;

public class EffectCenter : MonoBehaviour
{
    [Header("Material ")]
    public Material pool_surface;
    public Material material_ace;

    public Image aceImg_2D;
    List<Color> surfaceColors = new List<Color>();
    int sclength, count = 0;

    void Start()
    {
        surfaceColors.Add(new Color(0.6f, 0, 0, 1));                //·t©]¬õ
        surfaceColors.Add(new Color(0.23f, 0.58f, 0.23f, 1));//ÂOªLºñ
        surfaceColors.Add(new Color(0.14f, 0.66f, 0.7f, 1));  //®ü¬vÂÅ
        surfaceColors.Add(new Color(0.3f, 0.2f, 0.6f, 1));       //ÅåÆAµµ
        surfaceColors.Add(new Color(0.6f, 0.4f, 0, 1));           //¬\¯ó¶À

        sclength = surfaceColors.Count;
    }

    void Update()
    {
        aceImg_2D.color = RGB_lightEffect();
        material_ace.color = RGB_lightEffect();
        SkinChanging();
    }
    public static Color RGB_lightEffect()
    {
        float r;
        float g;
        float b;

        if (Mathf.Floor(Time.time) % 3 == 0)
        {
            r = 1;
            g = Time.time - Mathf.Floor(Time.time);
            b = 1 - (Time.time - Mathf.Floor(Time.time));
        }
        else if (Mathf.Floor(Time.time) % 3 == 1)
        {
            r = 1 - (Time.time - Mathf.Floor(Time.time));
            g = 1;
            b = Time.time - Mathf.Floor(Time.time);
        }
        else
        {
            r = Time.time - Mathf.Floor(Time.time);
            g = 1 - (Time.time - Mathf.Floor(Time.time));
            b = 1;
        }
        return new Color(r, g, b, 1);
    }
    void SkinChanging()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<AudioManager>().Play("Magic");

            pool_surface.color = surfaceColors[count % sclength];
            count++;
        }

    }
}
