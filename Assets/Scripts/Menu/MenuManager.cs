using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _mapWidth;

    [SerializeField]
    private TMP_InputField _mapHeight;
    private void SetAllParameters()
    {
        MenuParameters.Instance.height = int.Parse(_mapHeight.text);
        MenuParameters.Instance.width = int.Parse(_mapWidth.text);
    }
    public void LoadMainScene()
    {
        SetAllParameters();
        SceneManager.LoadScene(1);
    }
}