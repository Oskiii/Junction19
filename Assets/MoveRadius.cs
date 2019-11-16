using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRadius : MonoBehaviour
{
    public void SetRadius(float radius)
    {
        var bounds = GetComponent<SpriteRenderer>().sprite.bounds;
        var xSize = bounds.size.x;
        var ySize = bounds.size.y;
        transform.localScale = new Vector3(radius / (xSize * 0.5f), radius / (ySize * 0.5f), transform.localScale.z);
    }
}