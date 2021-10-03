using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    Character _player;

    private void Start()
    {
        _player = FindObjectOfType<Character>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Utils.PLAYER_TAG || collision.gameObject.tag == "PlayerFeet")
        {
            _player.PlayDeathSound();
           FindObjectOfType<Manager>().Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Utils.PLAYER_TAG || collision.gameObject.tag == "PlayerFeet")
        {
            _player.PlayDeathSound();
            FindObjectOfType<Manager>().Death();
        }

    }
}
