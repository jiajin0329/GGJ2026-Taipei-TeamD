using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class SceneSwitch : MonoBehaviour
    {
        public void LoadGameScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
