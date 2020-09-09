using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blackjack.Users
{
    public class DealerController : UserController
    {
        void Awake()
        {
            _deckController = GetComponent<DeckController>();
        }
    }
}