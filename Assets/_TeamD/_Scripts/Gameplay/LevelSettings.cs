using UnityEngine;

namespace WhoIsCatchingNaps
{
    [CreateAssetMenu(fileName = nameof(LevelSettings), menuName = "ScriptableObject/" + nameof(LevelSettings))]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField]
        public int levelTime { get; private set; } = 120;

        [field: Header("ReduceTime")]
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

        [field: Header("Combo")]
        [field: SerializeField]
        public float comboDOPunchScale { get; private set; } = 1.1f;

        [field: SerializeField]
        public float comboDOPunchScaleInDuration { get; private set; } = 0.1f;

        [field: SerializeField]
        public float comboDOPunchScaleOutDuration { get; private set; } = 0.2f;

        [field: Header("Audio")]
        [field: SerializeField]
        public AudioClipSetting[] audioClipSettings { get; private set; }
    }
}