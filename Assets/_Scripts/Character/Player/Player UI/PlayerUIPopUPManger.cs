using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LS
{
    public class PlayerUIPopUPManger : MonoBehaviour
    {
        [Header("Death Pop Up")]
        [SerializeField] GameObject deathPopUpGameObject;
        [SerializeField] TextMeshProUGUI deathPopUpBackgroundText;
        [SerializeField] TextMeshProUGUI deathPopUpText;
        [SerializeField] CanvasGroup deathPopUpCanvasGroup;

        public void SendDeathPopUp()
        {
            // ACTIVATE POST PROCESSING EFFECTS

            deathPopUpGameObject.SetActive(true);
            deathPopUpBackgroundText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(deathPopUpBackgroundText, 8f, 20f));
            StartCoroutine(FadeInPopUpOverTime(deathPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(deathPopUpCanvasGroup, 5, 5));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if(duration > 0)
            {
                text.characterSpacing = 0;
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
        {
            if (duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 1;
            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
        {
            if (duration > 0)
            {

                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;
                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvas.alpha = 0;
            yield return null;
        }
    }

}