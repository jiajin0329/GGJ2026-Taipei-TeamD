using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour {
    public void SceneSwitch() {
        SceneManager.LoadScene(1);
    }
}
