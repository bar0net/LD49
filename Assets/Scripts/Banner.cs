using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : MonoBehaviour
{
    public enum Type { None = 0, Happy, Sad, Wrath, Melancoly }
    public enum Tier { None = 0, T1 = 1, T2 = 2 }

    public SpriteRenderer[] flags;
    public Type type = Type.None;
    public Tier tier = Tier.T1;

    [Header("Faces")]
    public GameObject happyFace;
    public GameObject sadFace;
    public GameObject angryFace;
    public GameObject apathyFace;


    [Space]
    public bool interactable = true;

    Manager _m;

    // Start is called before the first frame update
    void Start()
    {
        this.Set(type, tier);
        _m = FindObjectOfType<Manager>();
    }

    public void Set(Type new_type, Tier new_tier)
    {
        type = new_type;
        tier = new_tier;

        // Paint the flag
        string key = Utils.BannerKey(type);
        string color_value = PlayerPrefs.GetString(key, Utils.DefaultBannerColor(type));
        foreach (SpriteRenderer flag in flags) flag.color = Utils.String2Color(color_value);

        // Show the appropiate flag tier
        flags[0].gameObject.SetActive(tier == Tier.T1);
        flags[1].gameObject.SetActive(tier == Tier.T2);


        happyFace.SetActive( type == Type.Happy);
        sadFace.SetActive(type == Type.Sad);
        angryFace.SetActive(type == Type.Wrath);
        apathyFace.SetActive(type == Type.Melancoly);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Utils.PLAYER_TAG)
        {
            switch (type)
            {
                case Type.Happy:
                    _m.UpdatePlayerMood(0, (int)tier);
                    break;
                case Type.Sad:
                    _m.UpdatePlayerMood(0, -(int)tier);
                    break;
                case Type.Wrath:
                    _m.UpdatePlayerMood((int)tier, 0);
                    break;
                case Type.Melancoly:
                    _m.UpdatePlayerMood(-(int)tier, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
