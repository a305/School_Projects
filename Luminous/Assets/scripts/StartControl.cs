using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartControl : MonoBehaviour {

    [SerializeField]
    string nextScene;
    public void OnClick()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            SceneManager.LoadScene(nextScene);
    }
}
