using UnityEngine;

namespace WhoIsCatchingNaps
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "ScriptableObject/" + nameof(LevelSettings))]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField]
        public int levelTime { get; private set; } = 120;

        [field: SerializeField]
        public float reduceTime { get; private set; }

        [field: SerializeField]
        public Color reduceTimeColor { get; private set; }

        [field: SerializeField]
        public float reduceTimeShakeStrength { get; private set; } = 50f;

        [field: SerializeField]
        public int reduceTimeShakeVibrato { get; private set; } = 100;

        [field: SerializeField]
        public float reduceTimeTextMoveY { get; private set; } = 50f;

        [field: SerializeField]
        public AudioClipSetting[] audioClipSettings { get; private set; }
    }
}