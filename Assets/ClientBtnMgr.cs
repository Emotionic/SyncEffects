using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;

public class ClientBtnMgr : MonoBehaviour
{
    public WSClient Client;
    public LogViewer Viewer;

	public void BtnSend_OnClick ()
    {
        var ws = Client.GetComponent<WSClient>().ws;
        ws.Send("REQ TEST EFF");
        Viewer.GetComponent<LogViewer>().AddLine("REQ TEST EFF");

    }

}
