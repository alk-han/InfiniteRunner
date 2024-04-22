using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [SerializeField] Transform defaultSubUI;
    private Transform currentActivatedUI;


    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.parent == transform) // direct child
            {
                child.gameObject.SetActive(false);
            }
        }
        SetActiveUI(defaultSubUI);
    }


    public void SetActiveUI(Transform newActiveUI)
    {
        if (newActiveUI == currentActivatedUI) return;

        if (currentActivatedUI != null)
        {
            currentActivatedUI.gameObject.SetActive(false);
        }

        newActiveUI.gameObject.SetActive(true);
        currentActivatedUI = newActiveUI;
    }
}
