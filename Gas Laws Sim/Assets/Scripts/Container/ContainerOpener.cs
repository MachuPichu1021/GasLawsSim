using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerOpener : MonoBehaviour
{
    [SerializeField] private Container container;
    private const float sensitivity = 0.25f;

    private void OnMouseDrag()
    {
        float startX = transform.position.x;
        transform.Translate(Input.GetAxis("Mouse X") * sensitivity * Vector2.up);
        float clampedXPos = Mathf.Clamp(transform.position.x, container.LeftWall.transform.position.x + 0.5f, container.RightWall.transform.position.x - 0.25f);
        transform.position = new Vector2(clampedXPos, transform.position.y);
        container.TopWall.transform.Translate((startX - clampedXPos) * Vector2.up);
    }
}
