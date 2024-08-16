using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParameters : MonoBehaviour
{
    public static MenuParameters Instance { get; private set; }

    public int height { get; set; }

    public int width { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}