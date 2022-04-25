using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparentOnCollide : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private Material targetMaterial;
    [SerializeField]
    private string tagName = "Player";

    void OnTriggerEnter(Collider collider)
    {
        targetMaterial = target.GetComponent<MeshRenderer>().material;
        if (collider.CompareTag(tagName))
        {
            Color targetColor = targetMaterial.GetColor("_BaseColor");
            targetColor.a = 0.5f;
            targetMaterial.SetColor("_BaseColor", targetColor);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(tagName))
        {
            Color targetColor = targetMaterial.GetColor("_BaseColor");
            targetColor.a = 1f;
            targetMaterial.SetColor("_BaseColor", targetColor);
        }
    }
}