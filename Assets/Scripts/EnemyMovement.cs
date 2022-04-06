using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Two positions the enemy should fly between
    public Vector3 pos1;
    public Vector3 pos2;

    // Speed and lerp stats
    private float interpolate = 0;
    public float rawSpeed;
    private float speed;

    void Update()
    {
        // Lerp between two positions
        transform.position = Vector3.Lerp(pos1, pos2, interpolate);
        interpolate += Time.deltaTime * speed;

        // Change direction when enemy reaches one position
        if (interpolate >= 1)
        {
            speed = -rawSpeed;
        }
        else if (interpolate <= 0)
        {
            speed = rawSpeed;
        }
    }
}
