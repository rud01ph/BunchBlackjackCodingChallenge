using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blackjack.Cards
{
    public class CardInfo : MonoBehaviour
    {
        private int _frontCardindex = 0;
        public int FrontCardIndex
        {
            get
            {
                return this._frontCardindex;
            }
            set
            {
                this._frontCardindex = value;
            }
        }

        //back card for setting 
        private int _backCardIndex = 0;
        public int BackCardIndex
        {
            get
            {
                return this._backCardIndex;
            }
            set
            {
                this._backCardIndex = value;
            }
        }

        private bool _isFaceShown = false;
        public bool IsFaceShown
        {
            get
            {
                return this._isFaceShown;
            }
            set
            {
                this._isFaceShown = value;
            }
        }


        [SerializeField] private Sprite[] _frontFace = null;
        [SerializeField] private Sprite[] _backFace = null;


        //return card value 
        //A = 1 , 2 = 2 .... J = 11, Q = 12, K = 13
        public int GetCardValue()
        {
            return (this._frontCardindex % 13) + 1;
        }

        public Sprite GetCardSprite()
        {
            return this._isFaceShown ? this._frontFace[_frontCardindex] : this._backFace[_backCardIndex];
        }
    }

}
