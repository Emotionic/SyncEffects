using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // ====================
    // PUBLIC
    // ====================

    public void PlayEffect(EffectData _effdata)
    {
        var obj = new GameObject();
        obj.transform.position = _effdata.Position;
        obj.transform.rotation = _effdata.Rotation;
        obj.transform.localScale = _effdata.LocalScale;

        PlayEffect(obj, _effdata.EffectName);

    }

    public void PlayEffect(GameObject _obj, string _effname, bool _doloop = false)
    {
        var eff = new EffectObject(_obj, _effname, _doloop);
        Play(eff);
        effects.Add(eff);
    }

    public void StopAll()
    {
        foreach(var eff in effects)
        {
            if (eff.Handle.HasValue)
            {
                eff.Handle.Value.Stop();
                eff.Handle = null;
            }
        }

    }

    public bool DEBUGMODE = false;
    public LogViewer Viewer;

    // ====================
    // PRIVATE
    // ====================

    private List<EffectObject> effects;
    private const float debugLogTiming = 0.5f;
    private float timeleft = debugLogTiming;

    // --------------------
    // Unity Events
    // --------------------

    private void Awake()
    {
        effects = new List<EffectObject>();
    }

    private void Update()
    {
        foreach (var eff in effects)
        {
            if (!eff.Handle.HasValue) continue;

            var h = eff.Handle.Value;
            var tran = eff.Obj.transform;

            if (h.exists)
            {
                h.SetLocation(tran.position);
                h.SetRotation(tran.rotation);
                h.SetScale(tran.localScale);
            }
            else if (eff.DoLoop)
            {
                Play(eff);
            }
            else
            {
                h.Stop();
                eff.Handle = null;
            }
        }

        effects.RemoveAll(eff => eff.Handle == null);

        timeleft -= Time.deltaTime;
        if (DEBUGMODE && Viewer != null)
        {
            if (timeleft <= 0.0f)
            {
                timeleft = debugLogTiming;
                foreach (var eff in effects)
                {
                    Viewer.GetComponent<LogViewer>().AddLine(string.Format("N:{0} P:{1} R:{2} S:{3}", eff.EffectName, eff.Obj.transform.position, eff.Obj.transform.rotation, eff.Obj.transform.localScale));
                }

            }

        }

    }

    // --------------------
    // Effect Functions
    // --------------------

    private void Play(EffectObject _eff)
    {
        var tran = _eff.Obj.transform;
        var h = EffekseerSystem.PlayEffect(_eff.EffectName, tran.position);
        h.SetRotation(tran.rotation);
        h.SetScale(tran.localScale);

        _eff.Handle = h;
    }

}

/// <summary>
/// エフェクトを管理するためのオブジェクト
/// </summary>
public class EffectObject
{
    public GameObject Obj;

    public EffekseerHandle? Handle;
    public string EffectName;
    public bool DoLoop;

    public EffectObject(GameObject _obj, string _effectname, bool _doloop)
    {
        Obj = _obj;
        EffectName = _effectname;
        DoLoop = _doloop;
    }

}
