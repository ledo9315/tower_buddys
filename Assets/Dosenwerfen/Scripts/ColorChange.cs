using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChange : MonoBehaviour
{
    [SerializeField] private Color changeColor;
    private Color originalColor;
    public void SetChangeColor(BaseInteractionEventArgs arg)
    {
        Material mat = GetObjectMaterial(arg);
        if (mat != null)
        {
            originalColor = mat.color;
            mat.color = changeColor;
        }
    }

    public void SetOriginalColor(BaseInteractionEventArgs arg)
    {
        Material mat = GetObjectMaterial(arg);
        if (mat != null)
        {
            mat.color = originalColor;
        }
    }

    private Material GetObjectMaterial(BaseInteractionEventArgs arg)
    {
        return arg.interactorObject.transform.parent.GetChild(1).GetComponent<Renderer>().material;
    }
}
