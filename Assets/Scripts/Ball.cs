using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    [SerializeField]
    private LineRenderer _line;
    public LineRenderer Line => _line;

    private void Awake()
    {
        if (!_rigidbody) _rigidbody = GetComponent<Rigidbody>();
        if (_line) _line = GetComponentInChildren<LineRenderer>(true);
    }
}
