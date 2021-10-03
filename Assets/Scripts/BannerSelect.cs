using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerSelect : MonoBehaviour
{
    public GameObject crossUI;

    Manager _m;
    SpriteRenderer _sr;
    Banner _b;

    private void Start()
    {
        _m = FindObjectOfType<Manager>();
        _sr = GetComponent<SpriteRenderer>();
        _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 0.5f);

        _b = GetComponentInParent<Banner>();

        crossUI.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (_m.inEditMode() && _b.interactable)
        {
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 1.0f);

            if (_b.type != Banner.Type.None) crossUI.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (_m.inEditMode() && _b.interactable)
        {
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 0.5f);
            crossUI.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        if (!_m.inEditMode() || !_b.interactable) return;

        if (_b.type == Banner.Type.None)
        {
            int idx = _m.getDraggedBannerIndex();
            if (idx == -1) return;

            _b.Set(_m.availableBanners[idx].type, _m.availableBanners[idx].tier);
            _m.RemoveAvailableBanner();
            crossUI.SetActive(true);
        }
        else
        {
            _m.AddAvailableBanner(_b.type, _b.tier);
            _b.Set(Banner.Type.None, Banner.Tier.None);
            crossUI.SetActive(false);
        }
    }
}
