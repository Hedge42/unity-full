using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoEvent : MonoBehaviour
{
    // Can overload all of these!
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

    public UnityEvent onValidate;
    public UnityEvent onAwake;
    public UnityEvent onStart;
    public UnityEvent onUpdate;
    public UnityEvent onLateUpdate;
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    public UnityEvent onDestroy;

    public UnityEvent<Collision> onCollisionEnter;
    public UnityEvent<Collision> onCollisionExit;
    public UnityEvent<Collision> onCollisionStay;
    public UnityEvent<Collider> onTriggerEnter = new UnityEvent<Collider>();
    public UnityEvent<Collider> onTriggerExit = new UnityEvent<Collider>();
    public UnityEvent<Collider> onTriggerStay = new UnityEvent<Collider>();

    private void OnValidate()
    {
        onValidate?.Invoke();
    }
    private void Awake()
    {
        onAwake?.Invoke();
    }
    private void OnEnable()
    {
        onEnable?.Invoke();
    }
    private void OnDisable()
    {
        onDisable?.Invoke();
    }
    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
    private void Start()
    {
        onStart?.Invoke();
    }
    private void Update()
    {
        onUpdate?.Invoke();
    }
    private void LateUpdate()
    {
        onLateUpdate?.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        onCollisionExit?.Invoke(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay?.Invoke(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }
}
