using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
        // Pas de setter, donc ne peut setter de l'exterieur
    }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            print("[Singleton] trying to instantiate a second instance of singleton class");
            return;
        }

        instance = (T) this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
