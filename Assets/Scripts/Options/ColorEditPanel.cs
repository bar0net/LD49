using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorEditPanel : MonoBehaviour
{
    public UnityEngine.UI.Text sectionTitle;
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.Image image;

    [Header("Sliders")]
    public UnityEngine.UI.Scrollbar redSlider;
    public UnityEngine.UI.Scrollbar greenSlider;
    public UnityEngine.UI.Scrollbar blueSlider;

    [Header("Buttons")]
    public UnityEngine.UI.Button acceptButton;
    public UnityEngine.UI.Button resetButton;

    [Space]
    public Color _color = Color.black;
    string _key = "test_key";
    bool allow_slider_updates = true;
    ColorSelectPanel _csp;

    // Start is called before the first frame update
    void Awake()
    {
        _csp = FindObjectOfType<ColorSelectPanel>();

        sectionTitle.text = "-";

        redSlider.interactable = false;
        greenSlider.interactable = false;
        blueSlider.interactable = false;

        acceptButton.interactable = false;
        resetButton.interactable = false;
    }

    void UpdateUI()
    {
        text.text = Utils.Color2String(_color);
        image.color = _color;

        allow_slider_updates = false;
        redSlider.value   = _color.r;
        greenSlider.value = _color.g;
        blueSlider.value  = _color.b;
        allow_slider_updates = true;
    }

    public void SliderChanged()
    {
        if (!allow_slider_updates) return;

        _color = new Color(redSlider.value, greenSlider.value, blueSlider.value);
        UpdateUI();
    }

    public void Set(string title, Color color, string key)
    {
        sectionTitle.text = title;
        _color = color;
        _key = key;

        redSlider.interactable = true;
        greenSlider.interactable = true;
        blueSlider.interactable = true;

        acceptButton.interactable = true;
        resetButton.interactable = true;

        UpdateUI();
    }

    public void Commit()
    {
        PlayerPrefs.SetString(_key, Utils.Color2String(_color));
        sectionTitle.text = "-";

        redSlider.interactable = false;
        greenSlider.interactable = false;
        blueSlider.interactable = false;

        acceptButton.interactable = false;
        resetButton.interactable = false;

        _csp.UpdateAll();

        this.gameObject.SetActive(false);
    }

    public void ResetDefault()
    {
        //TODO
        PlayerPrefs.DeleteKey(_key);

        if (_key == Utils.FOREGROUND_KEY)                           _color = Utils.String2Color(Utils.FOREGROUND_DEFAULT);
        else if (_key == Utils.BACKGROUND_KEY)                      _color = Utils.String2Color(Utils.BACKGROUND_DEFAULT);
        else if (_key == Utils.BannerKey(Banner.Type.Happy))        _color = Utils.String2Color(Utils.DefaultBannerColor(Banner.Type.Happy));
        else if (_key == Utils.BannerKey(Banner.Type.Wrath))        _color = Utils.String2Color(Utils.DefaultBannerColor(Banner.Type.Wrath));
        else if (_key == Utils.BannerKey(Banner.Type.Sad))          _color = Utils.String2Color(Utils.DefaultBannerColor(Banner.Type.Sad));
        else if (_key == Utils.BannerKey(Banner.Type.Melancoly))    _color = Utils.String2Color(Utils.DefaultBannerColor(Banner.Type.Melancoly));
        else                                                        _color = Utils.String2Color(Utils.DefaultBannerColor(Banner.Type.None));

        UpdateUI();
    }
}
