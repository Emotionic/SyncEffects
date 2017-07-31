using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Text))]
public class TitleBtnMgr : MonoBehaviour
{
    public Button BtnServer;
    public InputField inputField;
    public static string IPAddress = null;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            BtnServer.GetComponent<Button>().interactable = false;
        }

    }

    public void BtnClient_OnClick ()
    {
        IPAddress = inputField.GetComponent<InputField>().text;
        SceneManager.LoadScene("client");
    }

    public void BtnServer_OnClick()
    {
        SceneManager.LoadScene("server");
    }

}
