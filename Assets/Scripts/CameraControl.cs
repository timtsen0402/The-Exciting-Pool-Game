using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class CameraControl : MonoBehaviour
{

    public float rotateSpeed = 2f;
    public float scrollSpeed_cam1 = 30f;
    public float scrollSpeed_cam2 = 70f;
    bool temp = false;

    void Start()
    {
        table = GameObject.Find("PoolTable");
        //初始化球桿方向角度
        CueStick.transform.position = new Vector3(CueBall.transform.position.x, CueBall.transform.position.y, CueBall.transform.position.z + 0.65f);
        CueStick.transform.rotation = new Quaternion(0, 180f, 0, 0);
        //初始化相機方向角度
        transform.position = new Vector3(CueBall.transform.position.x, CueBall.transform.position.y + 130f, CueBall.transform.position.z + 190f);
        transform.rotation = new Quaternion(35f, 180f, 0, 0);

        offset3 = camera2.transform.position - table.transform.position;
    }
    void Update()
    {
        //Easter eggs
        if (camera2.transform.position.y > 310f && !temp)
        {
            FindObjectOfType<AudioManager>().Play("Scream");
            temp = true;
        }
//



        //主相機距離球保持一致
        transform.position = CueBall.transform.position + offset1;
        camera2.transform.position = table.transform.position + offset3;
        //主相機視角永遠跟隨球
        transform.LookAt(CueBall.transform.position);
        //副相機主導時球桿失效
        if (camera1.enabled == false) CueStick.SetActive(false);
        else CueStick.SetActive(true);

        if (camera1.enabled == true)
        {
            //縮放視角
            offset1 = Scrollview(offset1, scrollSpeed_cam1, 2, 80);
            //旋轉視角
            Rotateview();
        }
        else //camera2.enabled == true
        {
            offset3 = Scrollview(offset3, scrollSpeed_cam2, 10, 300);
        }
        //球停了(桿子置於球前)
        ResetCue();

    }

    Vector3 Scrollview(Vector3 offset, float scrollSpeed, float start, float end) //縮放只跟相機有關
    {
        float distance = offset.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, start, end);
        offset = offset.normalized * distance;
        return offset;
    }

    void Rotateview()
    {
        Vector3 pos_above_cue_ball = new Vector3(CueBall.transform.position.x, CueBall.transform.position.y + 1, CueBall.transform.position.z);
        Vector3 up = pos_above_cue_ball - CueBall.transform.position;

        if (Input.GetMouseButton(1))
        {
            float camAngle = transform.eulerAngles.x;
            //以球為準旋轉
            if (camAngle > 85 && camAngle < 90)
            {
                CueStick.transform.RotateAround(CueBall.transform.position, up, rotateSpeed * Input.GetAxis("Mouse X") - (camAngle - 85));
                transform.RotateAround(CueBall.transform.position, up, rotateSpeed * Input.GetAxis("Mouse X") - (camAngle - 85));
                transform.RotateAround(CueBall.transform.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y") - (camAngle - 85));
            }
            else
            {
                CueStick.transform.RotateAround(CueBall.transform.position, up, rotateSpeed * Input.GetAxis("Mouse X"));
                transform.RotateAround(CueBall.transform.position, up, rotateSpeed * Input.GetAxis("Mouse X"));
                transform.RotateAround(CueBall.transform.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            }

            offset1 = transform.position - CueBall.transform.position;
            offset2 = CueStick.transform.position - CueBall.transform.position;
        }
    }
    void ResetCue()
    {
        float multiplier = 19 / new Vector3(offset1.normalized.x, 0, offset1.normalized.z).magnitude;
        //

        if (isNewTurn && !Input.GetMouseButton(0))
        {
            //自己抓出來的值
            CueStick.transform.position = CueBall.transform.position +
                new Vector3(offset1.normalized.x * multiplier, 0, offset1.normalized.z * multiplier);
            CueStick.transform.LookAt(CueBall.transform.position);
        }

    }

}
