using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualApply : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer[] _legs;

    [SerializeField]
    private SkinnedMeshRenderer[] _shirts;

    [SerializeField]
    private SkinnedMeshRenderer[] _mouths;

    [SerializeField]
    private SkinnedMeshRenderer[] _brows;
    
    [SerializeField]
    private SkinnedMeshRenderer[] _eyes;

    [Header("Materials")]
    [SerializeField]
    private Material _skinMat;

    [SerializeField]
    private Material _shirtMat;

    [SerializeField]
    private Material _legsMat;

    [Space(8), SerializeField]
    private SavedCharacter _applyOnStart;

    void Start()
    {
        if (_applyOnStart != null)
            SetVisuals(_applyOnStart);
    }

    public void SetVisuals(SavedCharacter data)
    {
        foreach (var mesh in _legs)
        {
            mesh.gameObject.SetActive(mesh.gameObject.name == data.legsName);
            mesh.material = _legsMat;
        }

        foreach (var mesh in _shirts)
        {
            mesh.gameObject.SetActive(mesh.gameObject.name == data.shirtName);
            mesh.material = _shirtMat;
        }

        foreach (var mesh in _mouths)
        {
            mesh.gameObject.SetActive(mesh.gameObject.name == data.mouthName);
        }

        foreach (var mesh in _brows)
        {
            mesh.gameObject.SetActive(mesh.gameObject.name == data.browName);
        }
        
        foreach (var mesh in _eyes)
        {
            mesh.gameObject.SetActive(mesh.gameObject.name == data.eyeName);
        }
    }
}
