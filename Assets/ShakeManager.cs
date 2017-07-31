using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeManager : MonoBehaviour
{
    public LogViewer Viewer;

    public WSClient Client;

    public ShakeMeter Meter;
    public int Shake = 30;

    private void Start()
    {
        
    }

    private void Update()
    {
        ShakeCheck();
    }

    // シェイク判定
    private Vector3 Acceleration;
    private Vector3 preAcceleration;

    private int ShakeCount = 0;
    private int LastTick = 0;

    private void ShakeCheck()
    {
        preAcceleration = Acceleration;
        Acceleration = Input.acceleration;

        if (Vector3.Dot(preAcceleration, Acceleration) >= 2.35)
        {
            if (ShakeCount >= Shake)
            {
                var ws = Client.GetComponent<WSClient>().ws;
                ws.Send("LIKE");
                Viewer.GetComponent<LogViewer>().AddLine("Send LIKE!");

                ShakeCount = 0;
            }

            LastTick = Environment.TickCount;
            ShakeCount++;

        }

        if ((Environment.TickCount - LastTick) >= 2 * 1000)
        {
            ShakeCount--;
            LastTick = Environment.TickCount;
        }

        Meter.GetComponent<ShakeMeter>().Value = (float)ShakeCount / Shake;

    }

}
