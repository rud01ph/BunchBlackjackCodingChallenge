using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blackjack.Users
{
    [RequireComponent(typeof(DeckController))]
    public class UserController : MonoBehaviour
    {
        protected DeckController _deckController = null;

        //get face up cards total value 
        public int GetFaceUpCardValue()
        {
            return _deckController.GetHandValue(false);
        }

        //get hand's total value (check with face down cards) 
        public int GetTotalHandCardValue()
        {
            return _deckController.GetHandValue(true);
        }

        public void ClearHand()
        {
            _deckController.ClearDeck();
        }

        public void ShowCardFace(int index, bool isShown)
        {
            _deckController.ShowCard(index, isShown);
        }

        //check how many cards in hands
        public int GetTotalCards()
        {
            return _deckController.GetTotalCards();
        }

        public void Flip(int index)
        {
            _deckController.FlipCard(index);
        }

        public void PushCard(int index)
        {
            _deckController.Push(index);
        }

        public void PopCard()
        {
            _deckController.Pop();
        }
    }
}