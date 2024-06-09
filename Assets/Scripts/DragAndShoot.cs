using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameController;
using static CameraControl;
using static ForCueBall;

public class DragAndShoot : MonoBehaviour
{
    Slider forceBar;
    Image forceBar_BKimage;
    RectTransform minForce_img;
    Vector3 mousePressDownPos;
    Vector3 mouseReleasePos;
    Vector3 mouseHoldPos;

    public static float mag_force;
    float minForce = 0.75f;

    public LineRenderer dragLine;

    void Start()
    {

        forceBar = GameObject.Find("Canvas/forceBar").GetComponent<Slider>();
        forceBar_BKimage = GameObject.Find("Canvas/forceBar/Background").GetComponent<Image>();
        minForce_img = GameObject.Find("Canvas/forceBar/Background/minForceImg").GetComponentInChildren<RectTransform>();
    }
    void FixedUpdate()
    {

        if (!camera1.enabled || cueIsMove || a)
            return;
        #region Drag and Shoot
        if (Input.GetMouseButtonDown(0))
        {

            mousePressDownPos = Input.mousePosition;

            mousePressDownPos.z = 10f;
            Vector3 pos = camera1.ScreenToWorldPoint(mousePressDownPos);
            dragLine.enabled = true;

            dragLine.SetPosition(0, pos);
        }
        if (Input.GetMouseButton(0))
        {
            mouseHoldPos = Input.mousePosition;
            mouseHoldPos.z = 10f;
            Vector3 pos = camera1.ScreenToWorldPoint(mouseHoldPos);

            dragLine.SetPosition(1, pos);

            //拉動球桿
            transform.position = CueBall.transform.position + offset2 * (1 + Vector3.Magnitude(mousePressDownPos - mouseHoldPos) * 0.01f);
            mag_force = Vector3.Magnitude((mousePressDownPos - mouseHoldPos) * 0.01f);
            forceBar.value = mag_force;
            forceBar_BKimage.color = new Color(mag_force * 2, 2 - mag_force * 2, 0, 1);


        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseReleasePos = Input.mousePosition;
            dragLine.enabled = false;

            if (mag_force > minForce)
            {
                //力上限為1
                if (mag_force > 1)
                    mag_force = 1;
                //打出去！
                CueBall_RigidBody.velocity = -offset2.normalized * mag_force * 300f;
                print(CueBall_RigidBody.velocity.magnitude);
                //桿子向前督
                transform.position -= offset2 * 0.2f;

                a = true;
                b = false;
                isNewTurn = false;
                Hit_n_times++;

                //cam1.enabled = false;
                //cam2.enabled = true;

            }
            forceBar.value = 0;
            forceBar_BKimage.color = Color.white;
        }
#endregion
        if (Hit_n_times == 0)
            return;
        minForce = 0.13f;
        minForce_img.anchoredPosition = new Vector2(0, -328f);


    }
}
/*
 
    bool W_isHold = false;
    bool S_isHold = false;
    bool A_isHold = false;
    bool D_isHold = false;


//擊母球上下左右四點
if (CameraControl.isRotating == false)
{
    W_isHold = false;
    S_isHold = false;
    A_isHold = false;
    D_isHold = false;
    if (Input.GetKey(KeyCode.W))
    {
        transform.position = _cue_ball.transform.position + CameraControl.offset2 + new Vector3(0, 0.016f, 0);
        W_isHold = true;
    }
    if (Input.GetKey(KeyCode.S))
    {
        transform.position = _cue_ball.transform.position + CameraControl.offset2 - new Vector3(0, 0.016f, 0);
        S_isHold = true;
    }
    if (Input.GetKey(KeyCode.A))
    {
        transform.position = _cue_ball.transform.position + CameraControl.offset2 + (CameraControl.offset2_orth * 0.01f);
        A_isHold = true;
    }
    if (Input.GetKey(KeyCode.D))
    {
        transform.position = _cue_ball.transform.position + CameraControl.offset2 - (CameraControl.offset2_orth * 0.01f);
        D_isHold = true;
    }

}

//特殊擊打
if (W_isHold == true)
{
    transform.position = _cue_ball.transform.position + CameraControl.offset2 + new Vector3(0, 0.016f, 0) +
            CameraControl.offset2 * (mouseHoldPos - mousePressDownPos).magnitude * 0.005f;
}
if (S_isHold == true)
{
    transform.position = _cue_ball.transform.position + CameraControl.offset2 - new Vector3(0, 0.016f, 0) +
            CameraControl.offset2 * (mouseHoldPos - mousePressDownPos).magnitude * 0.005f;
}
if (A_isHold == true)
{
    transform.position = _cue_ball.transform.position + CameraControl.offset2 + (CameraControl.offset2_orth * 0.01f) +
            CameraControl.offset2 * (mouseHoldPos - mousePressDownPos).magnitude * 0.005f;
}
if (D_isHold == true)
{
    transform.position = _cue_ball.transform.position + CameraControl.offset2 - (CameraControl.offset2_orth * 0.01f) +
            CameraControl.offset2 * (mouseHoldPos - mousePressDownPos).magnitude * 0.005f;
}
*/