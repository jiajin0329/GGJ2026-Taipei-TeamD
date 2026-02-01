using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public static class VFX
    {
        public static async UniTaskVoid TextEffect(TextMeshProUGUI _text, string _string, float _moveY)
        {
            var _clone = UnityEngine.Object.Instantiate(_text, _text.transform.parent);
            var _cloneTransform = _clone.transform;
            _clone.text = _string;
            _cloneTransform.DOLocalMoveY(_cloneTransform.localPosition.y + _moveY, 1f).OnComplete(() => UnityEngine.Object.Destroy(_clone));
            _clone.gameObject.SetActive(true);

            await UniTask.Delay(500);

            _clone.DOFade(0f, 0.48f);
        }
    }
}