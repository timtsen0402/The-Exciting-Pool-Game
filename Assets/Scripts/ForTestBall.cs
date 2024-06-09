using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ForCueBall;
using static CameraControl;
using static GameController;


public class ForTestBall : MonoBehaviour
{
    public GameObject cueball;
    public LineRenderer predictLine;
    public Transform empty_touchpoint;

    public static bool trigged;
    public static float aimTime = 0 ;
    Rigidbody rb;
    private void Start()
    {
        trigged = false;
        rb = GetComponent<Rigidbody>();
    }

    //private void FixedUpdate()
    //{
    //    aimTime += 0.005f;
    //    if (!trigged && !Input.GetMouseButton(1))
    //    {
    //        transform.position = cueball.transform.position - offset2.normalized * aimTime * 30f;
    //    }
    //    if (a || !b || Input.GetMouseButton(1))
    //        predictLine.enabled = false;
    //}
    private void Update()
    {
        //print(trigged);
        if (!trigged && !Input.GetMouseButton(1))
        {
            rb.velocity = -offset2.normalized * 100f;
            
        }
        if (a || !b || Input.GetMouseButton(1))
            predictLine.enabled = false;
    }

    void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.CompareTag("1") ||
            other.gameObject.CompareTag("2") ||
            other.gameObject.CompareTag("3") ||
            other.gameObject.CompareTag("4") ||
            other.gameObject.CompareTag("5") ||
            other.gameObject.CompareTag("6") ||
            other.gameObject.CompareTag("7") ||
            other.gameObject.CompareTag("8") ||
            other.gameObject.CompareTag("9") )
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            //print("freeze");
            //predictLine.enabled = true;

            Vector3 pos_above_other_ball = new (other.transform.position.x, other.transform.position.y + 1, other.transform.position.z);
            Vector3 up = pos_above_other_ball - other.transform.position;

            empty_touchpoint.position = other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position);
            Vector3 vectorOftouch = empty_touchpoint.position - transform.position;
            float Angle = Vector3.SignedAngle(offset2, vectorOftouch, up);

            //修正偏移量(根據角度)
            if(Angle < 0)
                empty_touchpoint.RotateAround(other.transform.position, up, (180 + Angle) * 0.22f);
            else
                empty_touchpoint.RotateAround(other.transform.position, -up, (180 - Angle) * 0.22f);
            

            vectorOftouch = empty_touchpoint.position - transform.position;

            //畫預測線
            if (other.gameObject.tag == "wall" || Hit_n_times == 0)
            {
                predictLine.enabled = false;
            }
            else
            {
                predictLine.enabled = true;
                predictLine.SetPosition(0, transform.position);
                predictLine.SetPosition(1, empty_touchpoint.position + vectorOftouch * 500f);
            }
                

            trigged = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("1") ||
            other.gameObject.CompareTag("2") ||
            other.gameObject.CompareTag("3") ||
            other.gameObject.CompareTag("4") ||
            other.gameObject.CompareTag("5") ||
            other.gameObject.CompareTag("6") ||
            other.gameObject.CompareTag("7") ||
            other.gameObject.CompareTag("8") ||
            other.gameObject.CompareTag("9"))
        {
            trigged = false;
            rb.constraints = RigidbodyConstraints.None;
            transform.position = cueball.transform.position;
        }
    }

}
