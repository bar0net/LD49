using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    const int HAPPY = 0;
    const int SAD = 1;
    const int WRATH = 2;
    const int APATHY = 3;
    const float FADE_TIME = 0.1f;
    const float MAX_VOLUME = 0.25f;

    public bool hola = false;
    public AudioSource happy;
    public AudioSource sad;
    public AudioSource wrath;
    public AudioSource apathy;

    Manager _m;
    LevelManager _lm;
    AudioSource neutral;

    bool[] source_active = { false, false, false, false };
    float[] fade = { -10.0f, -10.0f, -10.0f, -10.0f };

    private void Awake()
    {
        neutral = GetComponent<AudioSource>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _m = FindObjectOfType<Manager>();
        _lm = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        neutral.mute = _lm.muted;
        wrath.mute = _lm.muted;
        apathy.mute = _lm.muted;
        happy.mute = _lm.muted;
        sad.mute = _lm.muted;
        if (_lm.muted) return;
        
        Vector2Int mood = _m.GetMood();

        if (mood.x > 0 && !source_active[WRATH])        { source_active[WRATH] = true; fade[WRATH] = Time.time; }
        else if (mood.x <= 0 && source_active[WRATH])   { source_active[WRATH] = false; fade[WRATH] = Time.time; }

        if (mood.x < 0 && !source_active[APATHY])       { source_active[APATHY] = true; fade[APATHY] = Time.time; }
        else if (mood.x >= 0 && source_active[APATHY])  { source_active[APATHY] = false; fade[APATHY] = Time.time; }


        if (mood.y > 0 && !source_active[HAPPY])        { source_active[HAPPY] = true; fade[HAPPY] = Time.time; }
        else if (mood.y <= 0 && source_active[HAPPY])   { source_active[HAPPY] = false; fade[HAPPY] = Time.time; }

        if (mood.y < 0 && !source_active[SAD])          { source_active[SAD] = true; fade[SAD] = Time.time; }
        else if (mood.y >= 0 && source_active[SAD])     { source_active[SAD] = false; fade[SAD] = Time.time; }

        float ratio = 0;

        ratio = Mathf.Clamp((Time.time - fade[WRATH]) / FADE_TIME, 0, 1);
        wrath.volume = 1.5f * MAX_VOLUME * (source_active[WRATH] ? ratio : (1 - ratio));

        ratio = Mathf.Clamp((Time.time - fade[APATHY]) / FADE_TIME, 0, 1);
        apathy.volume = 1.5f* MAX_VOLUME * (source_active[APATHY] ? ratio : (1 - ratio));

        ratio = Mathf.Clamp((Time.time - fade[HAPPY]) / FADE_TIME, 0, 1);
        happy.volume = MAX_VOLUME * (source_active[HAPPY] ? ratio : (1 - ratio));

        ratio = Mathf.Clamp((Time.time - fade[SAD]) / FADE_TIME, 0, 1);
        sad.volume = MAX_VOLUME * (source_active[SAD] ? ratio : (1 - ratio));
    }
}

