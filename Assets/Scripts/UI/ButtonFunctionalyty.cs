using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctionalyty : MonoBehaviour
{
    GameObject text;

    private void OnEnable()
    {
        text = transform.GetChild(0).gameObject;
        text.GetComponent<Text>().enabled = true;
        text.SetActive(false);
    }
    public void OnClick()
    {
        text.SetActive(!text.activeInHierarchy);
    }
}
