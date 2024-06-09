using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;


public class ForCueBall : MonoBehaviour
{
    private void Update()
    {
        StopConstraint();
    }
    //九號球規則
    private void OnCollisionEnter(Collision other)
    {
        if (Time.time > 0.5f &&
           (other.gameObject.tag == "1" ||
            other.gameObject.tag == "2" ||
            other.gameObject.tag == "3" ||
            other.gameObject.tag == "4" ||
            other.gameObject.tag == "5" ||
            other.gameObject.tag == "6" ||
            other.gameObject.tag == "7" ||
            other.gameObject.tag == "8" ||
            other.gameObject.tag == "9"))
        {
            GameObject Smallest = Balls[0];
            //初次碰撞且是撞擊目前檯面上最小號的球(未犯規)
            if (other.gameObject == Smallest && !hasTouched)
            {
                Debug.Log("您擊打了場上最小的球");
            }
            //初次碰撞撞擊非最小號的球(犯規)
            else if (other.gameObject != Smallest && !hasTouched)
            {
                Debug.Log("您不是擊打場上最小的球  (犯規)");
                isFoul = true;
            }
            hasTouched = true;
            if (!isNewTurn) FindObjectOfType<AudioManager>().Play("Hit");
        }
    }
    void StopConstraint()
    {
        if (CueBall_RigidBody.velocity.magnitude > 0.1f)
            return;

        CueBall_RigidBody.velocity = new Vector3(0, 0, 0);
        CueBall_RigidBody.angularVelocity = new Vector3(0, 0, 0);
    }
}