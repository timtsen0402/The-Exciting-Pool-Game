using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coor_PyCharmToUnity : MonoBehaviour
{
    public int x = 0;
    public int y = 0;
    float unityY = 13.7f;

    float a1 = -1909f/48000f;
    float b1 = 7.9f;
    float a2 = -1923f/32000f;
    float b2 = 17.56f;


    void Start()
    {

    }
    void Update()
    {
        Transformation(x, y);
    }
    void Transformation(int x, int y)
    {
        float X_ = x * a1 + b1;
        X_ = Mathf.Round(X_);
        float Y_ = y * a2 + b2;
        Y_ = Mathf.Round(Y_);
        int X = (int) X_;
        int Y = (int) Y_;
        print(tag + new Vector2(X, Y));

    }
}
