using UnityEngine;


public class Ball_Initialization : MonoBehaviour
{
    Vector3 spawnPos = new Vector3(-1.645f, 13.55f, -11.275f);
    int n = 0;

    void Start()
    {
        Vector3 temp;
        Vector3 original = spawnPos;
        Vector3 moving;
        Vector3 radius = GameController.Balls[0].GetComponent<Renderer>().bounds.size;
        

        //9 balls
        for (int i = 0; i < 3; i++)
        {
            for (int j = i; j < i + 3; j++)
            {
                moving = new Vector3(original.x - (j - n) * (radius.x / 2), original.y, original.z - j * Mathf.Sqrt(3) * (radius.z / 2));
                spawnPos = moving;
                GameController.Balls[2*i + j].transform.position = spawnPos;
            }
            n += 2;
        }
        //將9號球擺至中間位置
        temp = GameController.Balls[4].transform.position;
        GameController.Balls[4].transform.position = GameController.Balls[8].transform.position;
        GameController.Balls[8].transform.position = temp;

    }

}