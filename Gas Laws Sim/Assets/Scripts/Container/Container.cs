using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField] private GameObject leftWall;
    public GameObject LeftWall { get => leftWall; }
    [SerializeField] private GameObject rightWall;
    public GameObject RightWall { get => rightWall; }
    [SerializeField] private GameObject topWall;
    public GameObject TopWall { get => topWall; }
    [SerializeField] private GameObject bottomWall;
    public GameObject BottomWall { get => bottomWall; }

    [SerializeField] private GameObject containerHandle;

    public void Resize()
    {
        leftWall.transform.position = new Vector2(containerHandle.transform.position.x + 0.625f, leftWall.transform.position.y);
        float midpoint = (leftWall.transform.localPosition.x + rightWall.transform.localPosition.x) / 2;
        topWall.transform.localPosition = new Vector2(midpoint, topWall.transform.localPosition.y);
        bottomWall.transform.localPosition = new Vector2(midpoint, bottomWall.transform.localPosition.y);
        topWall.transform.localScale = new Vector2(topWall.transform.localScale.x, Mathf.Abs(midpoint) * 2 + rightWall.transform.localScale.x);
        bottomWall.transform.localScale = new Vector2(bottomWall.transform.localScale.x, Mathf.Abs(midpoint) * 2 + rightWall.transform.localScale.x);
    }

    public void Resize(float volume)
    {
        volume /= 1000;
        float width = volume / Mathf.Abs(topWall.transform.position.y - bottomWall.transform.position.y);
        leftWall.transform.position = new Vector2(rightWall.transform.position.x - width, leftWall.transform.position.y);
        float midpoint = (leftWall.transform.position.x + rightWall.transform.position.x) / 2;
        topWall.transform.position = new Vector2(midpoint, topWall.transform.position.y);
        bottomWall.transform.position = new Vector2(midpoint, bottomWall.transform.position.y);
        topWall.transform.localScale = new Vector2(topWall.transform.localScale.x, Mathf.Abs(midpoint) * 2 + rightWall.transform.localScale.x);
        bottomWall.transform.localScale = new Vector2(bottomWall.transform.localScale.x, Mathf.Abs(midpoint) * 2 + rightWall.transform.localScale.x);
    }
}