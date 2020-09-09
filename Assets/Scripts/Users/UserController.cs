using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class UserInfo
{
    public string id = "";
    public double balance = 0;

    public UserInfo()
    {

    }

    public UserInfo(string id, double balance)
    {
        this.id = id;
        this.balance = balance;
    }

}
namespace Blackjack.Users
{
    [RequireComponent(typeof(DeckController))]
    public class UserController : MonoBehaviour
    {
        protected UserInfo userInfo;
        protected DeckController _deckController = null;

        protected bool isBlackJack;
        public bool IsBlackJack
        {
            get
            {
                return isBlackJack;
            }
            set
            {
                isBlackJack = value;
            }
        }

        protected double battingFee = 0;

        public bool SetBatting(double value)
        {
            if (userInfo.balance >= value)
            {
                battingFee += value;
                userInfo.balance -= value;
                return true;
            }
            return false;
        }


        public double GetBattingFee()
        {
            return battingFee;
        }

        public double GetBalance()
        {
            return userInfo.balance;
        }

        public void SetBalance(double value)
        {
            userInfo.balance = value;
        }
        public void AddBalance(double value)
        {
            userInfo.balance += value;
        }

        public string GetUserID()
        {
            return userInfo.id;
        }


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
            battingFee = 0;
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