using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectPanel : MonoBehaviour
{
    public GameObject foregroundPanel;
    public GameObject backgroundPanel;
    public GameObject wrathPanel;
    public GameObject joyPanel;
    public GameObject sorrowPanel;
    public GameObject apathyPanel;

    ColorEditPanel editPanel;

    // Start is called before the first frame update
    void Start()
    {
        editPanel = FindObjectOfType<ColorEditPanel>();
        editPanel.gameObject.SetActive(false);

        UpdateAll();
    }

    public void UpdateAll()
    {
        Camera.main.backgroundColor = Utils.String2Color(PlayerPrefs.GetString(Utils.BACKGROUND_KEY, Utils.BACKGROUND_DEFAULT));
        foreach (Image image in FindObjectsOfType<Image>()) image.color = Utils.String2Color(PlayerPrefs.GetString(Utils.FOREGROUND_KEY, Utils.FOREGROUND_DEFAULT));

        UpdatePanel(foregroundPanel, Utils.FOREGROUND_KEY, Utils.FOREGROUND_DEFAULT);
        UpdatePanel(backgroundPanel, Utils.BACKGROUND_KEY, Utils.BACKGROUND_DEFAULT);

        UpdatePanel(wrathPanel, Utils.BannerKey(Banner.Type.Wrath), Utils.DefaultBannerColor(Banner.Type.Wrath));
        UpdatePanel(joyPanel, Utils.BannerKey(Banner.Type.Happy), Utils.DefaultBannerColor(Banner.Type.Happy));
        UpdatePanel(sorrowPanel, Utils.BannerKey(Banner.Type.Sad), Utils.DefaultBannerColor(Banner.Type.Sad));
        UpdatePanel(apathyPanel, Utils.BannerKey(Banner.Type.Melancoly), Utils.DefaultBannerColor(Banner.Type.Melancoly));
    }


    void UpdatePanel(GameObject panel, string key, string defaultValue)
    {
        Color color = Utils.String2Color(PlayerPrefs.GetString(key, defaultValue));

        panel.transform.GetChild(0).GetComponent<Image>().color = color;
        panel.transform.GetChild(1).GetComponent<Text>().color  = color;
    }

    public void Select(int i)
    {
        editPanel.gameObject.SetActive(true);

        if (i == 0)      editPanel.Set("Background", Utils.String2Color(PlayerPrefs.GetString(Utils.BACKGROUND_KEY, Utils.BACKGROUND_DEFAULT)), Utils.BACKGROUND_KEY);
        else if (i == 1) editPanel.Set("Foreground", Utils.String2Color(PlayerPrefs.GetString(Utils.FOREGROUND_KEY, Utils.FOREGROUND_DEFAULT)), Utils.FOREGROUND_KEY);
        else if (i == 2) editPanel.Set("Wrath Banner", Utils.String2Color(PlayerPrefs.GetString(Utils.BannerKey(Banner.Type.Wrath), Utils.DefaultBannerColor(Banner.Type.Wrath))), Utils.BannerKey(Banner.Type.Wrath));
        else if (i == 3) editPanel.Set("Joy Banner", Utils.String2Color(PlayerPrefs.GetString(Utils.BannerKey(Banner.Type.Happy), Utils.DefaultBannerColor(Banner.Type.Happy))), Utils.BannerKey(Banner.Type.Happy));
        else if (i == 4) editPanel.Set("Sorrow Banner", Utils.String2Color(PlayerPrefs.GetString(Utils.BannerKey(Banner.Type.Sad), Utils.DefaultBannerColor(Banner.Type.Sad))), Utils.BannerKey(Banner.Type.Sad));
        else if (i == 5) editPanel.Set("Apathy Banner", Utils.String2Color(PlayerPrefs.GetString(Utils.BannerKey(Banner.Type.Melancoly), Utils.DefaultBannerColor(Banner.Type.Melancoly))), Utils.BannerKey(Banner.Type.Melancoly));
    }
}
