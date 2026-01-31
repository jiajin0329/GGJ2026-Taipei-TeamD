using UnityEngine;

namespace Menu
{
    public class CloseGame : MonoBehaviour
    {
        public void OnClickButton()
        {
            Application.Quit();
        }
    }
}
