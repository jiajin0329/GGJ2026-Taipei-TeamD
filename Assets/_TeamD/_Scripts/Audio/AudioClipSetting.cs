using System;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public class AudioClipSetting
    {
        [field: SerializeField]
        public AudioName name { get; private set; }

        [field: SerializeField]
        public AudioClip audioClip { get; private set; }

        [field: SerializeField]
        public float volume { get; private set; }
    }

    public enum AudioName : byte
    {
        click,
        correct,
        wrong,
        timeIsAlmostUp,
        win,
        lose
    }
}