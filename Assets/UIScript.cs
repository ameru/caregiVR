using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public void StartGame() 
    {
        SceneManager.LoadScene("Demo");
    }
}
