using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    [SerializeField]
    MeshRenderer _meshRenderer;
    [SerializeField]
    Texture _openEye;
    [SerializeField]
    Texture _closeEye;
    [SerializeField]
    float _openTimer = 2;
    [SerializeField]
    float _closeTimer = 0.1f;

    bool _eyeState = true;

    private void OnEnable()
    {
        StartCoroutine(IE_StartOpen());
    }

    IEnumerator IE_StartOpen()
    {
        _meshRenderer.material.SetTexture("TextEye", _openEye);
        yield return new WaitForSeconds(_openTimer);
        StartCoroutine(IE_StartClose());
    }

    IEnumerator IE_StartClose()
    {
        _meshRenderer.material.SetTexture("TextEye", _closeEye);
        yield return new WaitForSeconds(_closeTimer);
        StartCoroutine(IE_StartOpen());
    }

}
