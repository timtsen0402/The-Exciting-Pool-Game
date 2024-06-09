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
    //�E���y�W�h
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
            //�즸�I���B�O�����ثe�i���W�̤p�����y(���ǳW)
            if (other.gameObject == Smallest && !hasTouched)
            {
                Debug.Log("�z�����F���W�̤p���y");
            }
            //�즸�I�������D�̤p�����y(�ǳW)
            else if (other.gameObject != Smallest && !hasTouched)
            {
                Debug.Log("�z���O�������W�̤p���y  (�ǳW)");
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