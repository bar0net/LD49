using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public const int ALIGN_GAP = 48;

    [System.Serializable]
    public struct BannerData
    {
        public BannerData(bool available, Banner.Type type, Banner.Tier tier) { this.available = available; this.tier = tier; this.type = type; }

        public bool available;
        public Banner.Tier tier;
        public Banner.Type type;
    }

    // Inspector Properties
    [Header("Available Banners")]
    public List<BannerData> availableBanners = new List<BannerData>();

    [Header("UI")]
    public RectTransform alignmentIndicator;

    // Private Properties
    Character player;
    Vector2Int mood = Vector2Int.zero;

    FlagPanelUI _flagPanel;
    public int dragged_banner_index = 0;
    bool in_edit_mode = true;

    float end = -1;

    private void Awake()
    {
        Camera.main.backgroundColor = Utils.String2Color(PlayerPrefs.GetString(Utils.BACKGROUND_KEY, Utils.BACKGROUND_DEFAULT));
        Color c = Utils.String2Color(PlayerPrefs.GetString(Utils.FOREGROUND_KEY, Utils.FOREGROUND_DEFAULT));
        foreach (SpriteRenderer sr in FindObjectsOfType<SpriteRenderer>())
        {
            c.a = sr.color.a;
            sr.color = c;
        }

        foreach (UnityEngine.UI.Image img in FindObjectsOfType<UnityEngine.UI.Image>())
        {
            c.a = img.color.a;
            img.color = c;
        }

        foreach (UnityEngine.UI.Text txt in FindObjectsOfType<UnityEngine.UI.Text>())
        {
            c.a = txt.color.a;
            txt.color = c;
        }

        foreach (UnityEngine.UI.Button btn in FindObjectsOfType<UnityEngine.UI.Button>())
        {
            c = Utils.String2Color(PlayerPrefs.GetString(Utils.BACKGROUND_KEY, Utils.BACKGROUND_DEFAULT));
            UnityEngine.UI.Text txt = btn.GetComponentInChildren<UnityEngine.UI.Text>();
            c.a = txt.color.a;
            txt.color = c;
        }
    }

    private void Start()
    {
        _flagPanel = FindObjectOfType<FlagPanelUI>();

        player = FindObjectOfType<Character>();
        player.transform.position = transform.GetChild(0).position;
    }

    private void Update()
    { 
        if (end > 0)
        {
            end -= Time.deltaTime;
            if (end < 0) ResetLevel();
        }

    }

    public void UpdatePlayerMood(int delta_wrath, int delta_happy)
    {
        mood.x += delta_wrath;
        mood.y += delta_happy;

        if (Utils.ManhattanDistance(mood.x, mood.y) > 2)
        {
            mood.x -= delta_wrath;
            mood.y -= delta_happy;
            return;
        }

        if      (mood.x ==  1) player.SetSpeed(Character.MoveType.Run);
        else if (mood.x ==  0) player.SetSpeed(Character.MoveType.Walk);
        else if (mood.x == -1) player.SetSpeed(Character.MoveType.Stop);

        if      (mood.y == -1) player.Crouch();
        else if (mood.y ==  0) player.Stand();
        else if (mood.y ==  1 && mood.x == 0) player.Jump();
        else if (mood.y ==  1 && mood.x == 1) player.Jump();

        alignmentIndicator.anchoredPosition = ALIGN_GAP * mood;
    }

    public void EndLevel()
    {
        player.Sleep();
    }

    private void ResetLevel()
    {
        player.transform.position = transform.GetChild(0).position;
        mood = Vector2Int.zero;
        alignmentIndicator.anchoredPosition = ALIGN_GAP * mood;

        AllowEdit(true);
        end = -1;

        player.ResetCharacter();
        FindObjectOfType<LevelExit>().exitToken.SetActive(true);

        foreach (MovingObject item in FindObjectsOfType<MovingObject>())
        {
            item.ResetObject();
        }
    }

    public void AllowEdit(bool value)
    {
        _flagPanel.PlayButtonInteractable(value);
        in_edit_mode = value;
    }

    public bool inEditMode() { return in_edit_mode; }

    public void Death()
    {
        //ResetLevel();
        end = 1.5f;
        player.SetPlaying(false);
    }

    public int getDraggedBannerIndex() { return dragged_banner_index < availableBanners.Count ? dragged_banner_index : -1; }

    public void AddAvailableBanner( Banner.Type type, Banner.Tier tier)
    {
        availableBanners.Add(new BannerData(true, type, tier));
        _flagPanel.Redraw();
    }

    public void RemoveAvailableBanner()
    {
        availableBanners.RemoveAt(dragged_banner_index);
        _flagPanel.Redraw();
    }

    public Vector2Int GetMood() { return mood; }
}
