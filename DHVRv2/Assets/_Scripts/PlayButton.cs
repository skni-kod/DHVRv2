using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {
    public Renderer _buttonRenderer;

    public Color _readyColor;
    public Color _resetColor;
    public Color _idleColor;

    void Start() {
        SetReadyColor();
    }

    public void SetReadyColor() {
        _buttonRenderer.material.color = _readyColor;
    }

    public void SetResetColor() {
        _buttonRenderer.material.color = _resetColor;
    }

    public void SetIdleColor() {
        _buttonRenderer.material.color = _idleColor;
    }
}