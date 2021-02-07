using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Combine
{
    public class Overlay : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private Coroutine _coroutine;


        public void FadeOut(Action callback = null)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _image.gameObject.SetActive(true);
            _coroutine = StartCoroutine(AnimationCoroutine(Screen.height / 2, Screen.height * 1.5f, 1, 0.9f, 600, callback));
        }

        public void FadeIn(Action callback = null)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _image.gameObject.SetActive(true);
            _coroutine = StartCoroutine(AnimationCoroutine(Screen.height * 1.5f, Screen.height / 2, 0.9f, 1, 600, callback));
        }

        private IEnumerator AnimationCoroutine(float fromY, float toY, float fromAlpha, float toAlpha, float duration, Action callback)
        {
            RectTransform imgTranform = _image.rectTransform;

            float y;
            float alpha;
            float t = 0;
            float speedMult = Mathf.Abs((toY - fromY) / duration);

            while (t < 1)
            {
                t += Time.deltaTime * speedMult;
                y = Mathf.Lerp(fromY, toY, t);
                alpha = Mathf.Lerp(fromAlpha, toAlpha, t);

                imgTranform.position = new Vector3(imgTranform.position.x, y, imgTranform.position.z);
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);

                yield return null;
            }

            if (callback != null)
                callback.Invoke();
        }
    }
}