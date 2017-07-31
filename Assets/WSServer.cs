using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

public class WSServer : MonoBehaviour
{
    public LogViewer Viewer;
    public EffectManager EffMgr;
    private WebSocketServer server;

    private void Start()
    {
        server = new WebSocketServer(3000);
        server.AddWebSocketService("/", () => new Echo(Viewer));
        server.Start();

        if (server.IsListening)
        {
            Viewer.GetComponent<LogViewer>().AddLine("< SERVER LISTENED >");
        }

    }

    private void Update()
    {
        if (Echo.SendQ != null)
        {
            foreach (var eff in Echo.SendQ)
            {
                var json = JsonUtility.ToJson(eff);
                var ws = new WebSocket("ws://localhost:3000/");
                ws.Connect();
                ws.Send("EFFDATA\n" + json);
                ws.Close();
            }

            Echo.SendQ.Clear();
        }

        if (Echo.RecvQ != null)
        {
            foreach (var eff in Echo.RecvQ)
            {
                EffMgr.GetComponent<EffectManager>().PlayEffect(eff);
            }

            Echo.RecvQ.Clear();
        }

    }

    private void OnDestroy()
    {
        server.Stop();
        server = null;
    }

}

// エコーサーバ (だが、後ろにちょっち付ける)
public class Echo : WebSocketBehavior
{
    public static Queue<EffectData> SendQ;
    public static Queue<EffectData> RecvQ;

    private LogViewer Viewer;
    private static int GreatLimit; // いつまで派手にするのか

    public Echo(LogViewer _viewer)
    {
        Viewer = _viewer;
        GreatLimit = 0;
        SendQ = new Queue<EffectData>();
        RecvQ = new Queue<EffectData>();
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        Viewer.AddLine("< RECIEVED >");
        Viewer.AddLine("Recieved: " + e.Data);

        var res = e.Data + ";" + System.DateTime.Now;
        if (e.Data.Split()[0] == "EFFDATA")
        {
            var effdata = JsonUtility.FromJson<EffectData>(e.Data.Split()[1]);
            if (GreatLimit > Environment.TickCount)
            {
                effdata.LocalScale *= 100.0f;
            }

            var json = JsonUtility.ToJson(effdata);
            res = "EFFDATA\n" + json;

            RecvQ.Enqueue(effdata);
        }
        if (e.Data == "REQ TEST EFF")
        {
            Viewer.AddLine("Return test effect.");
            var data = new EffectData();

            data.EffectName = "Laser01";
            data.Position = new Vector3(0.0f, 0.0f, 70.0f);
            data.Rotation = Quaternion.Euler(0, -90, 0);
            if (GreatLimit > Environment.TickCount)
            {
                data.LocalScale = new Vector3(100.0f, 100.0f, 100.0f);
            }
            else
            {
                data.LocalScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            RecvQ.Enqueue(data);

            var json = JsonUtility.ToJson(data);

            res = "EFFDATA\n" + json;

        }
        else if (e.Data == "LIKE")
        {
            Viewer.AddLine("Received LIKE. Make Effect Great Again.");

            GreatLimit = Environment.TickCount + 30 * 1000; // 30s for DEBUG

            var data = new EffectData();

            data.EffectName = "Laser01";
            data.Position = new Vector3(-3.0f, -2.0f, -1.0f);
            data.Rotation = Quaternion.Euler(0, -90, 0);
            if (GreatLimit > Environment.TickCount)
            {
                data.LocalScale = new Vector3(100.0f, 100.0f, 100.0f);
            }
            else
            {
                data.LocalScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            RecvQ.Enqueue(data);
            var json = JsonUtility.ToJson(data);

            res = "EFFDATA\n" + json;
        }

        Sessions.Broadcast(res);
        Viewer.AddLine("< BROADCASTED >");
    }
}

[Serializable]
public class EffectData
{
    public string EffectName;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 LocalScale;

    public EffectData() : this("Laser01", Vector3.zero, Quaternion.identity, Vector3.zero)
    { }

    public EffectData(string effectName, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        EffectName = effectName;
        Position = position;
        Rotation = rotation;
        LocalScale = localScale;

    }

}
