using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;

    [SerializeField] private GameObject containerHandle;

    public void Resize()
    {
        leftWall.transform.position = new Vector2(containerHandle.transform.position.x + 0.625f, leftWall.transform.position.y);
        float midpoint = (leftWall.transform.position.x + rightWall.transform.position.x) / 2;
        topWall.transform.position = new Vector2(midpoint, topWall.transform.position.y);
        bottomWall.transform.position = new Vector2(midpoint, bottomWall.transform.position.y);
        topWall.transform.localScale = new Vector2(topWall.transform.localScale.x, Mathf.Abs(midpoint) * 2 + rightWall.transform.localScale.x);
        bottomWall.transform.localScale = new Vector2(bottomWall.transform.localScale.x, Mathf.Abs(midpoint) * 2 + rightWall.transform.localScale.x);
    }
}
