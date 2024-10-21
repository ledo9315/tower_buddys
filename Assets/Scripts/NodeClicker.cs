using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeClicker : MonoBehaviour
{
    private Boolean _alreadyFull = false;
    private Renderer _rend;
    private Color _startColor;
    private Color _builtColor = Color.black;
    private Color _emptyMarkedColor = new(1f, 0.8f, 0.1f);
    private Color _fullMarkedColor = new(0.5f, 0.2f, 0f);


    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _startColor = _rend.material.color;
    }

    private void OnMouseDown()
    {
        if (!_alreadyFull) {
            _rend.material.color = _builtColor;
            _alreadyFull = true;
        } else {
            _rend.material.color = _startColor;
            _alreadyFull = false;
        }
    }

    private void OnMouseEnter()
    {
        if (!_alreadyFull)
        {
            _rend.material.color = _emptyMarkedColor;
        }else
        {
            _rend.material.color = _fullMarkedColor;
        }
    }

    private void OnMouseExit()
    {
        if (_alreadyFull)
        {
            _rend.material.color = _builtColor;
        }
        else
        {
            _rend.material.color = _startColor;
        }
    }
}
