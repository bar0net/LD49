using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerUI : MonoBehaviour
{
    public UnityEngine.UI.Image[] flags;

    public Banner.Type type = Banner.Type.None;
    public Banner.Tier tier = Banner.Tier.None;
    public UnityEngine.UI.Image face;

    public void Set(Banner.Type new_type, Banner.Tier new_tier, Sprite sprite = null, bool active = true)
    {
        tier = new_tier;
        type = new_type;

        // Show the appropiate flag
        flags[0].gameObject.SetActive(tier == Banner.Tier.T1 && active);
        flags[1].gameObject.SetActive(tier == Banner.Tier.T2 && active);

        // Paint flag
        string key = "bannertype_" + ((int)type).ToString();
        string color_value = PlayerPrefs.GetString(key, Utils.DefaultBannerColor(type));
        foreach (UnityEngine.UI.Image flag in flags) flag.color = Utils.String2Color(color_value);

        face.enabled = (sprite != null);
        face.sprite = sprite;
    }
}
