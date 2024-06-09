using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ForCueBall;
using static CameraControl;
using static GameController;

public class AimSystem : MonoBehaviour
{
    GameObject currentHitObject;
    public GameObject cueBall;
    public GameObject testBall;
    public LineRenderer predictLine;
    public LayerMask layerMask;
    SphereCollider ball_sc;
    MeshRenderer test_mr;
    float currentHitDistance;
    Vector3 currentHitPoint;
    Vector3 currentHitNormal;

    bool temp;
    void Start()
    {
        ball_sc = cueBall.GetComponent<SphereCollider>();
        test_mr = testBall.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //print(currentHitNormal);
        RayCast();
    }
    void RayCast()
    {
        RaycastHit hit;
        if (Input.GetMouseButton(1))
        {
            if (Physics.SphereCast(cueBall.transform.position, ball_sc.radius, -offset2.normalized, out hit, 200f, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                currentHitObject = hit.transform.gameObject;
                currentHitDistance = hit.distance;
                currentHitPoint = hit.point;
                currentHitNormal = hit.normal;
                testBall.transform.position = cueBall.transform.position - offset2.normalized * currentHitDistance;
                
            }
            else
            {
                currentHitObject = null;
                currentHitDistance = 200f;
            }
        }
        if (currentHitObject == null)
            return;
        Vector3 PredictedDirection = currentHitObject.transform.position - testBall.transform.position;
        predictLine.enabled = true;
        test_mr.enabled = true;
        predictLine.SetPosition(0, testBall.transform.position);
        predictLine.SetPosition(1, currentHitObject.transform.position + PredictedDirection * 500f);
        if (a || !b )
        {
            predictLine.enabled = false;
            test_mr.enabled = false;
        }
    }
}
