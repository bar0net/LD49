using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public GameObject exitToken;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Utils.PLAYER_TAG)
        {
            FindObjectOfType<Manager>().EndLevel();
            if (exitToken != null) exitToken.SetActive(false);
        }
    }
}
