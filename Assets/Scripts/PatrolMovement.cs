using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MovingObject
{
    public Transform target;
    public Transform[] points;
    public float speed = 3.0f;

    [SerializeField] bool forward = true;
    [SerializeField] int prev_index = 0;
    [SerializeField] int next_index = 1;

    Vector3 direction = Vector3.zero;
    Character _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Character>();
        ResetObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_player.isPlaying()) return;

        Vector3 diff = (points[next_index].position - target.position);

        if (diff.magnitude < speed * Time.deltaTime)
        {
            target.position = points[next_index].position;

            prev_index = next_index;
            next_index = forward ? next_index + 1 : next_index - 1;

            if (next_index < 0)
            {
                next_index = 1;
                forward = true;
            }
            else if (next_index >= points.Length)
            {
                next_index = points.Length - 2;
                forward = false;
            }
            direction = (points[next_index].position - points[prev_index].position).normalized;
        }
        else
        {
            target.position += speed * Time.deltaTime * direction;
        }

    }

    public override void ResetObject()
    {
        target.position = points[0].position;
        direction = (points[1].position - points[0].position).normalized;
    }
}
