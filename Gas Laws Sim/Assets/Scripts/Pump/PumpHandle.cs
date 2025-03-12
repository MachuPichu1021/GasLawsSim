using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpHandle : MonoBehaviour
{
    private const float sensitivity = 0.25f; 
    private void OnMouseDrag()
    { 
        transform.Translate(Vector2.up * Input.GetAxis("Mouse Y") * sensitivity);
        float clampedYPos = Mathf.Clamp(transform.localPosition.y, 0, 2.25f);
        transform.localPosition = new Vector2(transform.localPosition.x, clampedYPos);
    }
}
