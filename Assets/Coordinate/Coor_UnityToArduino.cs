using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;
using static DragAndShoot;

public class Coor_UnityToArduino : MonoBehaviour
{
    public GameObject CueBall;

    float a1 = -160f / 83f;
    float b1 = -1316f / 415f;
    float a2 = 1.89288f;
    float b2 = 39.5611f;

    //float xBoundary1 = -11.19f;
    //float xBoundary2 = 7.9f;
    //float zBoundary1 = -20.9f;
    //float zBoundary2 = 17.56f;
    void Update()
    {
        if (isNewTurn)
            TransToArduinoCoordinate();
    }
    void TransToArduinoCoordinate()
    {
        Transformation("CueBall", CueBall.transform.position.x, CueBall.transform.position.z);
        foreach (GameObject ball in Balls)
        {
            if (ball == null) continue;
            Transformation(ball.tag, ball.transform.position.x, ball.transform.position.z);
        }

    }

    void Transformation(string tag, float x, float z)
    {
        float X = x * a1 + b1;
        X = Mathf.Round(X);
        float Y = z * a2 + b2;
        Y = Mathf.Round(Y);
        print(tag + new Vector2(X, Y));
    }
}
