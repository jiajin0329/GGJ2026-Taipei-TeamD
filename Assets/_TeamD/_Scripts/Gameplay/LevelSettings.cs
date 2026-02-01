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

        [field: Header("doPunch")]
        [field: SerializeField]
        public float doPunchScaleInDuration { get; private set; } = 0.05f;

        [field: SerializeField]
        public float doPunchScaleOutDuration { get; private set; } = 0.4f;

        [field: Header("Combo")]
        [field: SerializeField]
        public float comboDOPunchScale { get; private set; } = 1.4f;

        [field: Header("AddScore")]
        [field: SerializeField]
        public float addScoreDOPunchScale { get; private set; } = 1.1f;

        [field: SerializeField]
        public Color addScoreColor { get; private set; }

        [field: Header("Audio")]
        [field: SerializeField]
        public AudioClipSetting[] audioClipSettings { get; private set; }
    }
}