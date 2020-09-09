using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blackjack.Users
{
    public class PlayerController : UserController
    {
        void Awake()
        {
            _deckController = GetComponent<DeckController>();
            userInfo = new UserInfo();
            userInfo.id = "Eden";
            userInfo.balance = 10000;
        }
    }
}