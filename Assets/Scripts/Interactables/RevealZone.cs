using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealZone : PlayerDetector
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _fadeDuration = 2f;
    private Coroutine currentFade;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void Effect(Collider2D collision)
    {
        // Fade out (desaparece)
        if (currentFade != null) 
            StopCoroutine(currentFade);

        currentFade = StartCoroutine(FadeToAlpha(0f, _fadeDuration));
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DogPlayer"))
        {
            // Fade in (aparece)
            if (currentFade != null)
                StopCoroutine(currentFade);
            currentFade = StartCoroutine(FadeToAlpha(1f, _fadeDuration));
        }
    }
    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        float startAlpha = _spriteRenderer.color.a;
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            _spriteRenderer.color = new Color(0, 0, 0, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        // Asegura el valor final
        _spriteRenderer.color = new Color(0, 0, 0, targetAlpha);
    }
}
