using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static AudioManager;

public class GameController : MonoBehaviour
{

    #region The most basic objects for every script
    public static GameObject table;
    public static GameObject Panel;
    public static GameObject Audio;

    public static Camera camera1;
    public static Camera camera2;
    public static GameObject CueStick;
    public static GameObject CueBall;
    public static Rigidbody CueBall_RigidBody;

    public static Ball[] balls = new Ball[10];
    public static List<GameObject> Balls = new List<GameObject>();
    public static List<Rigidbody> Balls_rb = new List<Rigidbody>();
    public static List<GameObject> Balls_images2D = new List<GameObject>();

    public static Vector3 offset1;          // the distance of ball & cam1
    public static Vector3 offset2;          // the distance of ball & cue
    public static Vector3 offset3;          // the distance of ball & cue

    public static Vector3 Zero3 = new Vector3(0, 0, 0);
    #endregion
    #region Definition & Initialize Settings
    //Boundary (Inner Round)
    public static float boundOfpool_x1 = -10.55f;
    public static float boundOfpool_x2 = 7.23f;
    public static float boundOfpool_z1 = -20.37f;
    public static float boundOfpool_z2 = 16.92f;
    //Game Logic
    public int loseFoulCount = 3;
    public static int Hit_n_times = 0;
    public static bool hasTouched = false;
    public static bool isOut = false;
    public static bool isInHole = false;
    public static bool isFoul = false;
    public static bool freeBallTime = true;
    public static bool cueIsMove = false;
    public static bool otherIsMove = false;
    public static bool isNewTurn = true;
    public static bool isAnyoneWin = false;
    public static bool a = true; //���F���s�^�X���O�u���@��
    public static bool b = true; //�קK���X�᪽���ǳW
    public static bool c = true; //���F���������O�u���@��

    bool Player1_turn = true;
    bool Player2_turn = false;
    int P1_foulCount = 0;
    int P2_foulCount = 0;
    #endregion
    #region  Boring Definitions
    public static Text hint;
    public static Text designer;
    public static float timer;

    Text turn;
    Text foulCount;
    LineRenderer start_line;

    GameObject Ace;
    Rigidbody Ace_rb;
    GameObject Ace_image2D;

    List<GameObject> outBalls = new List<GameObject>();
    List<Rigidbody> outBalls_rb = new List<Rigidbody>();
    List<GameObject> outBalls_image2D = new List<GameObject>();

    List<GameObject> holeBalls = new List<GameObject>();
    List<Rigidbody> holeBalls_rb = new List<Rigidbody>();
    List<GameObject> holeBalls_image2D = new List<GameObject>();
    #endregion
    void Awake() { Loading(); }
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (!isAnyoneWin)
            foulCount.text = "Foul Count      P1 : " + P1_foulCount + "/ " + loseFoulCount + "    P2 : " + P2_foulCount + "/ " + loseFoulCount;
        //�}�y������}�y�u
        if (Hit_n_times != 0) start_line.enabled = false;
        //�y�O�_�J�U
        Ball_Statement();
        //�i�J�U�^�X
        New_Turn();
        //�i�J�ӧQUI
        WinUI();
    }
    void Ball_Statement()
    {
        if (isNewTurn)
            return;
        if (CueBall.transform.position.y < 10.5f)
            isFoul = true;

        for (int i = 0; i < Balls.Count; i++)
        {
            //�Y�y�J�U
            if (Balls[i].transform.position.y < 12.5f && Balls[i].transform.position.y > 12f &&
                boundOfpool_x1 - 2.5f < Balls[i].transform.position.x && Balls[i].transform.position.x < boundOfpool_x2 + 2.5f &&
                boundOfpool_z1 - 2.5f < Balls[i].transform.position.z && Balls[i].transform.position.z < boundOfpool_z2 + 2.5f)
            {
                isInHole = true;
                FindObjectOfType<AudioManager>().Play("Drop");

                holeBalls.Add(Balls[i]);
                holeBalls_image2D.Add(Balls_images2D[i]);
                holeBalls_rb.Add(Balls_rb[i]);
            }
            //�Y�y�Q��(�ǳW)
            if (boundOfpool_x1 - 2.5f > Balls[i].transform.position.x || Balls[i].transform.position.x > boundOfpool_x2 + 2.5f ||
                 boundOfpool_z1 - 2.5f > Balls[i].transform.position.z || Balls[i].transform.position.z > boundOfpool_z2 + 2.5f)
            {
                isOut = true;

                outBalls.Add(Balls[i]);
                outBalls_image2D.Add(Balls_images2D[i]);
                outBalls_rb.Add(Balls_rb[i]);

            }
        }
    }
    void New_Turn()
    {
        if (Ball_isMoving())
            return;

        if (a && b) //�����e(�s�^�X�}�l)a,b�Ҭ�true
        {
            //Debug.Log("New Turn");
            isNewTurn = true;
            if (isInHole)
            {
                for (int i = 0; i < holeBalls.Count; i++)
                {
                    holeBalls[i].SetActive(false);
                    holeBalls_image2D[i].SetActive(false);
                    holeBalls_rb[i].Sleep();

                    Balls_images2D.Remove(holeBalls_image2D[i]);
                    Balls.Remove(holeBalls[i]);
                    Balls_rb.Remove(holeBalls_rb[i]);

                }
            }

            //�X�y�᥼�I����y(�ǳW)
            if (!hasTouched && Hit_n_times > 0)
            {
                Debug.Log("foul (no touching)");
                isFoul = true;
                FoulCounting();
                FreeBall();
                Check_Victory();
                TakeTurn();
            }
            //�Y9���y�P���y�P�ɤJ�U(�ǳW�A9���y���m)
            else if (!Ace.activeSelf && CueBall.transform.position.y < 10.5f)
            {
                Debug.Log("foul (9Ball &cueBall dropped down simultaneously)");

                Ace.SetActive(true);
                Ace_rb.WakeUp();
                Ace_image2D.SetActive(true);
                Balls.Add(Ace);
                Balls_rb.Add(Ace_rb);
                Balls_images2D.Add(Ace_image2D);

                Ace.transform.position = new Vector3(-1.645f, 20f, -11.275f);
                FoulCounting();
                FreeBall();
                Check_Victory();
                TakeTurn();

            }

            //���y�����I���w�y(�ǳW)
            else if (hasTouched && isFoul)
            {
                Debug.Log("foul (hit wrong ball)");
                FoulCounting();

                FreeBall();
                Check_Victory();
                TakeTurn();
            }
            else if (isOut)
            {
                Debug.Log("foul (out of bound)");
                isFoul = true;

                for (int i = 0; i < outBalls.Count; i++)
                {
                    outBalls[i].SetActive(false);
                    outBalls_image2D[i].SetActive(false);
                    outBalls_rb[i].Sleep();

                    Balls_images2D.Remove(outBalls_image2D[i]);
                    Balls.Remove(outBalls[i]);
                    Balls_rb.Remove(outBalls_rb[i]);
                }
                FoulCounting();

                FreeBall();
                Check_Victory();
                TakeTurn();

            }
            //���ǳW�B���i -> ���H
            else if ((!isOut && !isInHole) && Hit_n_times != 0)
            {
                Debug.Log("no foul and take turn");
                Check_Victory();
                TakeTurn();
            }
            else
            {
                Debug.Log("good job! keep it up");
                Check_Victory();
            }
            //�C�Ӧ^�X�����ɪ��򥻳]�w
            Check_Victory();
            CueBall.transform.localEulerAngles = Zero3;     //����xyz�b
            hasTouched = false;
            isFoul = false;
            isInHole = false;
            isOut = false;
            holeBalls.Clear();
            holeBalls_image2D.Clear();
            holeBalls_rb.Clear();
            outBalls.Clear();
            outBalls_image2D.Clear();
            outBalls_rb.Clear();
            a = false; //�R��ɥu����@��

            //***�{�b�i�H�X��***//

        }
        b = true;
    }
    #region Functional Subroutine
    public static bool Ball_isMoving()
    {
        if ((CueBall_RigidBody.velocity == Zero3 && CueBall_RigidBody.angularVelocity == Zero3) || Hit_n_times == 0) cueIsMove = false;
        else cueIsMove = true;

        Vector3 temp = Zero3;
        //�v�@�d��
        for (int i = 0; i < Balls_rb.Count; i++)
        {
            if (Balls_rb[i].velocity.magnitude < 0.1f)
                Balls_rb[i].velocity = Zero3;
            temp += Balls_rb[i].velocity;
        }
        if (temp == Zero3) otherIsMove = false;
        else otherIsMove = true;

        if (cueIsMove || otherIsMove)
            return true;
        else
            return false;
    }

    void TakeTurn()
    {
        if (turn.color == Color.red)
            return;
        Player1_turn = !Player1_turn;
        Player2_turn = !Player2_turn;
        if (Player1_turn) turn.text = "P1's Turn";
        else turn.text = "P2's Turn";
    }

    void Check_Victory() //�ӧQ�P�w
    {
        //print("enter Check_Victory");

        if (P1_foulCount == loseFoulCount) Winner(2);
        if (P2_foulCount == loseFoulCount) Winner(1);
        c = true;

        if (!Ace.activeSelf && c)
        {
            print("enter !Ace.activeSelf && c");

            if (Player1_turn)
            {
                //print("enter Player1_turn");
                if (!isFoul)
                    Winner(1);
                else
                    Winner(2);
            }
            else
            {
                if (!isFoul)
                    Winner(2);
                else
                    Winner(1);
            }
        }
    }
    void Winner(int playerNumber)
    {
        isAnyoneWin = true;
        FindObjectOfType<AudioManager>().Play("Victory");
        turn.color = Color.red;
        turn.fontStyle = FontStyle.BoldAndItalic;
        turn.text = "P" + playerNumber + " Wins !!";
        foulCount.text = "Press  Esc  To Restart  Or  Quit";
        foulCount.color = Color.black;
        camera1.enabled = false;
        camera2.enabled = true;

        hint.enabled = false;
        designer.enabled = true;


        c = false;
    }
    void FreeBall()
    {
        camera1.enabled = false;
        camera2.enabled = true;
        freeBallTime = true;
    }
    void FoulCounting()
    {
        if (Player1_turn)
            P1_foulCount++;
        else
            P2_foulCount++;

        //�ھڥǳW���Ƨ��ܦr���C��
        if (P1_foulCount == (loseFoulCount - 1) || P2_foulCount == (loseFoulCount - 1))
            foulCount.color = Color.yellow;

    }
    void WinUI()
    {
    }
    #endregion
    #region void Update and KeyPressDown 
    void Update()
    {
        if (CueBall.transform.position.y < -500f)
        {
            timer += Time.deltaTime;
            designer.enabled = true;
            designer.color = new Color(0, 1, .5f);
            foulCount.color = Color.black;
            turn.color = Color.white;
            turn.text = "~ Enjoy Music ~";
            if (timer % 2 > 1.0f)
                foulCount.text = "";
            else
                foulCount.text = "Press  Esc  To Restart  Or  Quit";
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Panel.activeSelf)
                Time.timeScale = 1f;
            else
                Time.timeScale = 0f;
            Panel.SetActive(!Panel.activeSelf);
        }

        CamSwitch();
        AllStop();

        if (!isAnyoneWin)
            return;
        if (timer % 2 > 1.0f)
            foulCount.text = "";
        else
            foulCount.text = "Press  Esc  To Restart  Or  Quit";

    }
    void CamSwitch()
    {
        if (!(Input.GetKeyDown(KeyCode.V) && !freeBallTime))
            return;
        camera1.enabled = !camera1.enabled;
        camera2.enabled = !camera2.enabled;
    }
    void AllStop()
    {
        //�j���
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < Balls_rb.Count; i++)
            {
                Balls_rb[i].velocity = Zero3;
                Balls_rb[i].angularVelocity = Zero3;
            }
            CueBall_RigidBody.velocity = Zero3;
            CueBall_RigidBody.angularVelocity = Zero3;
            transform.rotation = new Quaternion(0, 0, 0, 0);

        }

    }
    #endregion
    void Loading()
    {
        // for Restart
        Balls_images2D.Clear();
        Balls.Clear();
        Balls_rb.Clear();
        Hit_n_times = 0;
        freeBallTime = true;

        //�[���y�ϥ�
        Balls_images2D.Add(GameObject.Find("Image1"));
        Balls_images2D.Add(GameObject.Find("Image2"));
        Balls_images2D.Add(GameObject.Find("Image3"));
        Balls_images2D.Add(GameObject.Find("Image4"));
        Balls_images2D.Add(GameObject.Find("Image5"));
        Balls_images2D.Add(GameObject.Find("Image6"));
        Balls_images2D.Add(GameObject.Find("Image7"));
        Balls_images2D.Add(GameObject.Find("Image8"));
        Balls_images2D.Add(GameObject.Find("Image9"));
        //�[���C���y
        CueBall = GameObject.Find("CueBall");
        Balls.Add(GameObject.Find("Ball_1"));
        Balls.Add(GameObject.Find("Ball_2"));
        Balls.Add(GameObject.Find("Ball_3"));
        Balls.Add(GameObject.Find("Ball_4"));
        Balls.Add(GameObject.Find("Ball_5"));
        Balls.Add(GameObject.Find("Ball_6"));
        Balls.Add(GameObject.Find("Ball_7"));
        Balls.Add(GameObject.Find("Ball_8"));
        Balls.Add(GameObject.Find("Ball_9"));

        balls[0] = GameObject.Find("CueBall").GetComponent<Ball>();
        balls[1] = GameObject.Find("Ball_1").GetComponent<Ball>();
        balls[2] = GameObject.Find("Ball_2").GetComponent<Ball>();
        balls[3] = GameObject.Find("Ball_3").GetComponent<Ball>();
        balls[4] = GameObject.Find("Ball_4").GetComponent<Ball>();
        balls[5] = GameObject.Find("Ball_5").GetComponent<Ball>();
        balls[6] = GameObject.Find("Ball_6").GetComponent<Ball>();
        balls[7] = GameObject.Find("Ball_7").GetComponent<Ball>();
        balls[8] = GameObject.Find("Ball_8").GetComponent<Ball>();
        balls[9] = GameObject.Find("Ball_9").GetComponent<Ball>();


        //�[���C���y�����z�S��
        CueBall_RigidBody = CueBall.GetComponentInChildren<Rigidbody>();
        for (int i = 0; i < Balls.Count; i++)
        {
            Balls_rb.Add(Balls[i].GetComponent<Rigidbody>());
        }

        CueStick = GameObject.Find("CueStick");
        Audio = GameObject.Find("Audio");

        camera1 = GameObject.Find("Camera").GetComponent<Camera>();
        camera2 = GameObject.Find("Camera2").GetComponent<Camera>();

        hint = GameObject.Find("Canvas/Hint").GetComponent<Text>();
        turn = GameObject.Find("Canvas/turn").GetComponent<Text>();
        foulCount = GameObject.Find("Canvas/foulCount").GetComponent<Text>();
        designer = GameObject.Find("Canvas/designer").GetComponent<Text>();

        start_line = GameObject.Find("Camera").GetComponent<LineRenderer>();
        Panel = GameObject.Find("Canvas/Settings");

        //�S�O�w�q9���y
        Ace = Balls[8];
        Ace_rb = Balls_rb[8];
        Ace_image2D = Balls_images2D[8];

        turn.text = "P1's Turn";
        hint.text = Free_Ball.hintOK;
        isAnyoneWin = false;
        camera1.enabled = false;
        camera2.enabled = true;
        designer.enabled = false;
        Panel.SetActive(false);

        offset1 = camera1.transform.position - CueBall.transform.position;
        offset2 = CueStick.transform.position - CueBall.transform.position;


    }
}



