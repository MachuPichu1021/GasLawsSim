using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerHandle : MonoBehaviour
{
    [SerializeField] private Container container;
    private const float sensitivity = 0.25f;

    [SerializeField] private Transform topLeftCorner;
    [SerializeField] private Transform bottomRightCorner;
    [SerializeField] private GameObject ghostTopWall;
    [SerializeField] private GameObject ghostBottomWall;
    [SerializeField] private GameObject ghostLeftWall;

    private void OnEnable()
    {
        transform.position = new Vector2(container.LeftWall.transform.position.x - 0.625f, transform.position.y);
    }

    private void OnMouseDown()
    {
        ghostLeftWall.SetActive(true);
        ghostTopWall.SetActive(true);
        ghostBottomWall.SetActive(true);
        container.LeftWall.GetComponent<Collider2D>().enabled = false;
        GasParticle[] gasParticles = FindObjectsByType<GasParticle>(FindObjectsSortMode.None);
        foreach (GasParticle particle in gasParticles)
            particle.Freeze();
    }

    private void OnMouseDrag()
    {
        transform.Translate(Input.GetAxis("Mouse X") * sensitivity * Vector2.right);
        float clampedXPos = Mathf.Clamp(transform.localPosition.x, -8.5f, -3);
        transform.localPosition = new Vector2(clampedXPos, transform.localPosition.y);
        container.Resize();
    }

    private void OnMouseUp()
    {
        ghostLeftWall.SetActive(false);
        ghostTopWall.SetActive(false);
        ghostBottomWall.SetActive(false);
        ghostLeftWall.transform.position = container.LeftWall.transform.position;
        ghostTopWall.transform.position = container.TopWall.transform.position;
        ghostTopWall.transform.localScale = container.TopWall.transform.localScale;
        ghostBottomWall.transform.position = container.BottomWall.transform.position;
        ghostBottomWall.transform.localScale = container.BottomWall.transform.localScale;
        container.LeftWall.GetComponent<Collider2D>().enabled = true;
        GasParticle[] gasParticles = FindObjectsByType<GasParticle>(FindObjectsSortMode.None);
        foreach (GasParticle particle in gasParticles)
        {
            float xPos = Random.Range(topLeftCorner.position.x, bottomRightCorner.position.x);
            float yPos = Random.Range(bottomRightCorner.position.y, topLeftCorner.position.y);
            particle.transform.position = new Vector2(xPos, yPos);
            particle.Unfreeze();
        }
    }
}