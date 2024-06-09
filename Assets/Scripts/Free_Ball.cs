using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameController;
using static CameraControl;

public class Free_Ball : MonoBehaviour
{
    bool isOverlap;
    float boundOfz1_now = boundOfpool_z1 + 28.9f;
    Vector3 pos_now;
    SphereCollider sc;
    Rigidbody rb;
    public static string hintOK = "Key   \" Enter \"   To   Continue";
    public static string hintNotOK = "Invalid Position";


    void Start()
    {
        sc = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //�i�J�ۥѲy���`
        if (freeBallTime)
        {
            Put();
            Constraints();
            Enter();
        }
        timer += Time.deltaTime;
        if (hint.enabled && hint.text == hintNotOK)
            return;
        if (timer % 2 > 1.0f)
            hint.text = "";
        else
            hint.text = hintOK;
    }
    void Put()
    {
        if (isAnyoneWin)
            return;
        hint.enabled = true;
        rb.freezeRotation = true;
        Vector3 targetPos = camera2.WorldToScreenPoint(gameObject.transform.position); //���y�b�ù��y�Ъ���m
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetPos.z);
        Vector3 newPos = camera2.ScreenToWorldPoint(mousePos); //�q�ù��y�Ф��^���`�y��

        //�즲��
        if (Input.GetMouseButton(0))
        {
            rb.freezeRotation = true;
            rb.useGravity = false;
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.velocity = new Vector3(0, 0, 0);
            sc.isTrigger = true;
            transform.position = new Vector3(newPos.x, 13.7f, newPos.z);
        }
        //��}��
        if (Input.GetMouseButtonUp(0))
        {
            if (hint.text.Length > 30)
            {
                rb.useGravity = true;
                sc.isTrigger = false;
                pos_now = new Vector3(newPos.x, 13.7f, newPos.z);
                transform.position = pos_now;
            }

        }

    }
    void Constraints()
    {
        //�D�Ĥ@���h�վ�d��
        if (Hit_n_times != 0) boundOfz1_now = boundOfpool_z1;

        if (gameObject.transform.position.x < boundOfpool_x2 &&
            gameObject.transform.position.x > boundOfpool_x1 &&
            gameObject.transform.position.z < boundOfpool_z2 &&
            gameObject.transform.position.z > boundOfz1_now &&
            isOverlap == false)
        {
            hint.text = hintOK;
            hint.color = Color.green;

        }
        else
        {
            hint.text = hintNotOK;
            hint.color = Color.red;
        }

    }
    //ENTER��
    public void Enter()
    {
        if (hint.color == Color.green && Input.GetKeyDown(KeyCode.Return))
        {
            rb.freezeRotation = false;
            rb.useGravity = true;
            sc.isTrigger = false;
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            transform.localEulerAngles = new Vector3(0, 0, 0);
            hint.enabled = false;
            camera2.enabled = false;
            camera1.enabled = true;
            freeBallTime = false;
            isFoul = false;

            offset2 = CueStick.transform.position - transform.position;
        }
    }

    //�\�񤣥i���|
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Untagged")
        {
            hint.text = hintNotOK;
            hint.color = Color.red;
            isOverlap = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        hint.text = hintOK;
        hint.color = Color.green;
        isOverlap = false;
    }

}
