using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] UIs;

    private void Start()
    {
        SwitchUI(0);
    }

    public void SwitchUI(int index)
    {
        foreach (GameObject UI in UIs)
            UI.SetActive(false);
        UIs[index].SetActive(true);
    }
}
