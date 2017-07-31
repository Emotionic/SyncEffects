using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LogViewer), typeof(EffectManager))]
public class WSClient : MonoBehaviour
{
    public LogViewer Viewer;
    public EffectManager EffMgr;
    public WebSocket ws;

    private Queue<EffectData> effectQ;

    private void Start()
    {
        effectQ = new Queue<EffectData>();
        ws = new WebSocket("ws://" + TitleBtnMgr.IPAddress + ":3000/");

        ws.OnOpen += (sender, e) =>
        {
            Viewer.GetComponent<LogViewer>().AddLine("WebSocket Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            Viewer.GetComponent<LogViewer>().AddLine("Data: " + e.Data);

            var recvs = e.Data.Split();

            if (recvs[0] == "EFFDATA")
            {
                var effdata = JsonUtility.FromJson<EffectData>(recvs[1]);
                effectQ.Enqueue(effdata);
            }

        };

        ws.OnError += (sender, e) =>
        {
            Viewer.GetComponent<LogViewer>().AddLine("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Viewer.GetComponent<LogViewer>().AddLine("WebSocket Close");
        };

        ws.Connect();

    }

    private void Update()
    {

        //if (Input.GetKeyUp("s"))
        //{
        //    ws.Send("REQ TEST EFF");
        //    Viewer.GetComponent<LogViewer>().AddLine("REQ TEST EFF");
        //}

        foreach(var eff in effectQ)
        {
            EffMgr.GetComponent<EffectManager>().PlayEffect(eff);
        }

        effectQ.Clear();

    }

    private void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}
