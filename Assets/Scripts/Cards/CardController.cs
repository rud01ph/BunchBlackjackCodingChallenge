using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blackjack.Cards
{
    [RequireComponent(typeof(CardInfo))]
    public class CardController : MonoBehaviour
    {
        [SerializeField] private CardInfo _cardInfo = null;
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        public AnimationCurve _scaleCurve = null;
        private float _duration = 0.5f;

        public void ShowFace(bool value)
        {
            _cardInfo.IsFaceShown = value;
            _spriteRenderer.sprite = _cardInfo.GetCardSprite();
        }

        //flip Animation 
        public void FlipCard()
        {
            StopCoroutine(FlipCoroutine());
            StartCoroutine(FlipCoroutine());
        }

        private IEnumerator FlipCoroutine()
        {
            _spriteRenderer.sprite = _cardInfo.GetCardSprite();
            _cardInfo.IsFaceShown = !_cardInfo.IsFaceShown;
            float time = 0f;

            while (time <= 1f)
            {
                float scale = _scaleCurve.Evaluate(time);
                time = time + Time.fixedDeltaTime / _duration;

                Vector3 localScale = transform.localScale;
                localScale.x = scale;
                transform.localScale = localScale;

                if (time >= 0.5f)
                {
                    _spriteRenderer.sprite = _cardInfo.GetCardSprite();
                }

                yield return new WaitForFixedUpdate();
            }
        }
    }

}
