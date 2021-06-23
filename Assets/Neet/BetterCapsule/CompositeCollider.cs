﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CompositeCollider : MonoBehaviour
{
    public bool isTrigger;
    public ColliderHandler[] handlers;

    // multiple colliders may cause these to fire at unexpected times 
    // or multiple times per frame
    public UnityEvent<Collider> onEnter;
    public UnityEvent<Collider> onExit;
    public UnityEvent<Collider> onStay;

    private void Awake()
    {
        for (int i = 0; i < handlers.Length; i++)
        {
            handlers[i].onTriggerStay += OnStay;
            handlers[i].onTriggerEnter += OnEnter;
            handlers[i].onTriggerExit += OnExit;

            for (int j = i; j < i; j++)
                Physics.IgnoreCollision(
                    handlers[i].GetComponent<Collider>(),
                    handlers[j].GetComponent<Collider>());
        }
    }

    private void OnValidate()
    {
        foreach (var c in handlers)
        {
            Collider col = c.GetComponent<Collider>();
            col.isTrigger = isTrigger;
        }
    }

    void OnStay(Collider h)
    {
        onStay?.Invoke(h);
    }
    void OnExit(Collider h)
    {
        onExit?.Invoke(h);
    }
    void OnEnter(Collider c)
    {
        onEnter?.Invoke(c);
    }
}
