using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using static GameController;

public class TCP_Receiver : MonoBehaviour
{
    //秨币TCP port
    Thread receiveThread;
    TcpClient client;
    TcpListener listener;
    int port;
    int temp = 0;
    string data;
    public GameObject[] balls;
    float[] transformPosition = new float[20];
    float a1 = -1909f / 48000f;
    float b1 = 7.9f;
    float a2 = -1923f / 32000f;
    float b2 = 17.56f;
    int X_changed;
    int Y_changed; 
    float ball_height = 13.7f;

    void Start()
    {
        InitTcp();
    }
    void Update()
    {
        if (data == null) return;
        if (Hit_n_times == 0)
        {
            balls[0].transform.position = new Vector3(transformPosition[0], ball_height, transformPosition[1]);
            balls[1].transform.position = new Vector3(transformPosition[2], ball_height, transformPosition[3]);
            balls[2].transform.position = new Vector3(transformPosition[4], ball_height, transformPosition[5]);
            balls[3].transform.position = new Vector3(transformPosition[6], ball_height, transformPosition[7]);
            balls[4].transform.position = new Vector3(transformPosition[8], ball_height, transformPosition[9]);
            balls[5].transform.position = new Vector3(transformPosition[10], ball_height, transformPosition[11]);
            balls[6].transform.position = new Vector3(transformPosition[12], ball_height, transformPosition[13]);
            balls[7].transform.position = new Vector3(transformPosition[14], ball_height, transformPosition[15]);
            balls[8].transform.position = new Vector3(transformPosition[16], ball_height, transformPosition[17]);
            balls[9].transform.position = new Vector3(transformPosition[18], ball_height, transformPosition[19]);
        }

    }
    void InitTcp()
    {
        port = 5066;
        print("TCP Initialized");
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        listener = new TcpListener(anyIP);
        listener.Start();
        //秨穝盿Τ把计thread肚よ猭讽把计
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    //private void OnDestroy()
    //{
    //    receiveThread.Abort();
    //}
    void ReceiveData()
    {
        print("received something...");
        try
        {
            while (true)
            {
                client = listener.AcceptTcpClient();
                NetworkStream stream = new NetworkStream(client.Client);
                StreamReader sr = new StreamReader(stream);
                data = sr.ReadLine();
                string[] data_ = data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var d in data_)
                {
                    if (d == "x")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));
                        transformPosition[0] = X_changed;
                        transformPosition[1] = Y_changed;
                    }
                    if (d == "a")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));
                        transformPosition[2] = X_changed;
                        transformPosition[3] = Y_changed;
                    }
                    if (d == "b")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[4] = X_changed;
                        transformPosition[5] = Y_changed;
                    }
                    if (d == "c")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[6] = X_changed;
                        transformPosition[7] = Y_changed;
                    }
                    if (d == "d")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[8] = X_changed;
                        transformPosition[9] = Y_changed;
                    }
                    if (d == "e")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[10] = X_changed;
                        transformPosition[11] = Y_changed;
                    }
                    if (d == "f")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[12] = X_changed;
                        transformPosition[13] = Y_changed;
                    }
                    if (d == "g")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[14] = X_changed;
                        transformPosition[15] = Y_changed;
                    }
                    if (d == "h")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[16] = X_changed;
                        transformPosition[17] = Y_changed;
                    }
                    if (d == "i")
                    {
                        Transformation(Int32.Parse(data_[temp + 1]), Int32.Parse(data_[temp + 2]));

                        transformPosition[18] = X_changed;
                        transformPosition[19] = Y_changed;
                    }
                    temp++;
                }
                data = String.Empty;
            }
        }
        catch (Exception ex)
        {
            print(ex);
        }
    }
    void Transformation(int x, int y)
    {
        float X_ = x * a1 + b1;
        X_ = Mathf.Round(X_);
        float Y_ = y * a2 + b2;
        Y_ = Mathf.Round(Y_);
        int X = (int)X_;
        int Y = (int)Y_;
        X_changed = X;
        Y_changed = Y;
    }

}
