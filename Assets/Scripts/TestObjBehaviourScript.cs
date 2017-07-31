using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjBehaviourScript : MonoBehaviour
{
    public EffectManager effectManager;
    private EffectManager _effmgr;

    // Use this for initialization
    private void Start()
    {
        _effmgr = effectManager.GetComponent<EffectManager>();

        // Create Effect
        var obj = new GameObject();
        obj.transform.position = new Vector3(0.0f, 0.0f, -25.0f);
        obj.transform.rotation = Quaternion.Euler(0, -90, 0);
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        _effmgr.PlayEffect(obj, "Laser01", true);

    }

    // Update is called once per frame
    private void Update()
    {



    }
}
