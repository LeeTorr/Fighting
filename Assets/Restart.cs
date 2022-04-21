using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public Button RestartButton;
    public GameObject guimanager;


    void Start()
    {    
        Button btn = RestartButton.GetComponent<Button>();
        btn.onClick.AddListener(RestartGame);
    }
    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

}