using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMgr : MonoBehaviour
{

    public LogViewer Viewer;
    public EffectManager EffMgr;

    public void OnButtonClicked()
    {
        Viewer.GetComponent<LogViewer>().AddLine("Button Clicked. " + DateTime.Now);

        var obj = new EffectData();

        obj.EffectName = "Laser01";
        obj.Position = new Vector3(8.0f, -16.0f, 27.0f);
        obj.Rotation = Quaternion.Euler(0, -90, 0);
        obj.LocalScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (Echo.SendQ != null)
        {
            Echo.SendQ.Enqueue(obj);
        }

    }

    public void OnLogSTOPClicked()
    {
        Viewer.GetComponent<LogViewer>().StopLog = !Viewer.GetComponent<LogViewer>().StopLog;
    }

    public void OnLogCLRClicked()
    {
        Viewer.GetComponent<LogViewer>().Clear();
    }

    public void BtnGenEff_OnClick()
    {
        var _x = GameObject.FindGameObjectsWithTag("GenEffX")[0].GetComponent<InputField>().text;
        var _y = GameObject.FindGameObjectsWithTag("GenEffY")[0].GetComponent<InputField>().text;
        var _z = GameObject.FindGameObjectsWithTag("GenEffZ")[0].GetComponent<InputField>().text;

        try
        {
            var obj = new EffectData();

            obj.EffectName = "Laser01";
            obj.Position = new Vector3(float.Parse(_x), float.Parse(_y), float.Parse(_z));
            obj.Rotation = Quaternion.Euler(0, -90, 0);
            obj.LocalScale = new Vector3(1.0f, 1.0f, 1.0f);

            if (Echo.SendQ != null)
            {
                Echo.SendQ.Enqueue(obj);
            }

        }
        catch
        {
            Viewer.GetComponent<LogViewer>().AddLine("Position Value is not valid.");
            return;
        }

    }

}
