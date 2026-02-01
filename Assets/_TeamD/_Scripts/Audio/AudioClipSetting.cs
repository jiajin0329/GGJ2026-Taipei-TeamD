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
        handDown, //打擊音效
        skill_rollCall, //技能觸發 - 點名
        skill_math, //技能觸發 - 數學
        skill_catGrass, //技能觸發 - 貓草
        wrong, //懲罰
        nap, //打嗑睡
        chuckle, //小聲笑聲
        classBell //上課鐘音效
    }
}