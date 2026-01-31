using UnityEngine;

namespace WhoIsCatchingNaps
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "ScriptableObject/" + nameof(LevelSettings))]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField]
        public int levelTime;
    }
}