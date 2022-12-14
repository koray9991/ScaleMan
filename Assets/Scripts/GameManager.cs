using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject canvas;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
 public   void Buttons(int ButtonNo)
    {
        if (ButtonNo == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
