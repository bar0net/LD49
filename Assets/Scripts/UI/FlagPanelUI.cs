using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagPanelUI : MonoBehaviour
{
    public Button playButton;
    public BannerUI[] banners;
    [Space]
    public Sprite happyFace;
    public Sprite sadFace;
    public Sprite angryFace;
    public Sprite apathyFace;


    Manager _m;
    Character _player;
    Image[] borders;

    int selected_banner = 0;
    Color solid;
    Color transparent;

    private void Start()
    {
        _m = FindObjectOfType<Manager>();
        _player = FindObjectOfType<Character>();

        solid = Utils.String2Color(PlayerPrefs.GetString(Utils.FOREGROUND_KEY, Utils.FOREGROUND_DEFAULT));
        transparent = new Color(solid.r, solid.g, solid.b, 0.5f);

        borders = new Image[banners.Length];
        for (int i = 0; i < borders.Length; i++) borders[i] = banners[i].GetComponent<Image>();

        Redraw();
    }

    private void Update()
    {
        if ((Input.mouseScrollDelta.y > 0 || Input.GetKeyDown(KeyCode.Q)) && selected_banner < banners.Length - 1 && banners[selected_banner+1].type != Banner.Type.None)
        {
            borders[selected_banner].color = transparent;
            selected_banner++;
            _m.dragged_banner_index++;
            borders[selected_banner].color = solid;
        }
        else if ( (Input.mouseScrollDelta.y < 0 || Input.GetKeyDown(KeyCode.A)) && selected_banner > 0)
        {
            borders[selected_banner].color = transparent;
            selected_banner--;
            _m.dragged_banner_index--;
            borders[selected_banner].color = solid;
        }
    }

    public void Redraw()
    {
        if (selected_banner >= _m.availableBanners.Count)
        {
            selected_banner = _m.availableBanners.Count - 1;
            _m.dragged_banner_index = selected_banner;
        }
        if (selected_banner < 0 && _m.availableBanners.Count > 0)
        {
            selected_banner = 0;
            _m.dragged_banner_index = selected_banner;
        }

        for (int i = 0; i < _m.availableBanners.Count; i++)
        {
            if (i >= banners.Length) break;

            Manager.BannerData data = _m.availableBanners[i];

            if (data.available)
            {
                Sprite s = null;
                switch (data.type)
                {
                    case Banner.Type.Happy:
                        s = happyFace;
                        break;
                    case Banner.Type.Sad:
                        s = sadFace;
                        break;
                    case Banner.Type.Wrath:
                        s = angryFace;
                        break;
                    case Banner.Type.Melancoly:
                        s = apathyFace;
                        break;
                }
                banners[i].Set(data.type, data.tier, s);
            }
            else banners[i].Set(Banner.Type.None, data.tier);
        }

        for (int i = _m.availableBanners.Count; i < banners.Length; i++)
        {
            banners[i].Set(Banner.Type.None, Banner.Tier.None);
        }
        
        for (int i = 0; i < borders.Length; i++)
        {
            if (i == selected_banner && banners[i].type != Banner.Type.None) borders[i].color = solid;
            else borders[i].color = transparent;
        }
    }

    public void Play()
    {
        _player.Play();
    }

    public void PlayButtonInteractable(bool value)
    {
        playButton.interactable = value;
    }
}
