using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blackjack.Users;
using Blackjack.Common;

public class BlackjackGameManager : MonoBehaviour
{
    [SerializeField] PlayerController _player = null;
    [SerializeField] DealerController _dealer = null;
    [SerializeField] DeckController _deck = null;
    [SerializeField] UIManager _gameUIManager = null;

    [SerializeField] private int maxDeck = 8;
    public int MaxDeck
    {
        get
        {
            return maxDeck;
        }
        set
        {
            maxDeck = value;
        }
    }

    // Deal 
    public void StartGame()
    {
        _gameUIManager.StartGame();

        _deck.CreateDeck(maxDeck);
        _deck.Shuffle();
        _deck.ShowCards(true);

        _player.ClearHand();
        _dealer.ClearHand();


        for (int i = 0; i < 2; ++i)
        {
            _player.PushCard(_deck.Pop());
            _player.ShowCardFace(i, true);
            _dealer.PushCard(_deck.Pop());
        }
        _dealer.ShowCardFace(0, true);

        //if the dealer's hand is not blackjack, it does not reveal. 
        _dealer.ShowCardFace(1, (_dealer.GetTotalHandCardValue() == 21));

        _gameUIManager.SetPlayerHandCardValue(_player.GetFaceUpCardValue());
        _gameUIManager.SetDealerHandCardValue(_dealer.GetFaceUpCardValue());
    }

    public void Hit(bool isPlayer)
    {
        //player hit the card 
        if (isPlayer)
        {
            _player.PushCard(_deck.Pop());
            _player.ShowCardFace(_player.GetTotalCards() - 1, false);
            _player.Flip(_player.GetTotalCards() - 1);
            _gameUIManager.SetPlayerHandCardValue(_player.GetFaceUpCardValue());

            //when player get over 21, player always lost the game.
            if (_player.GetFaceUpCardValue() > 21)
            {
                _gameUIManager.SetTheResultText(EvaluateCards());
            }

        }
        else
        {
            _dealer.PushCard(_deck.Pop());
            _dealer.ShowCardFace(_dealer.GetTotalCards() - 1, false);
            _dealer.Flip(_dealer.GetTotalCards() - 1);
            _gameUIManager.SetDealerHandCardValue(_dealer.GetFaceUpCardValue());
        }

    }



    public void UserStand()
    {
        //dealer start acting
        StartCoroutine(DealersTurn());
    }
    IEnumerator DealersTurn()
    {
        _dealer.Flip(1);
        _gameUIManager.SetDealerHandCardValue(_dealer.GetFaceUpCardValue());

        yield return new WaitForSeconds(1f);

        while (_dealer.GetFaceUpCardValue() < 17)
        {
            Hit(false);
            yield return new WaitForSeconds(1f);
        }

        _gameUIManager.SetTheResultText(EvaluateCards());

        yield return new WaitForSeconds(1f);
    }
    //if the two cards have the same value, separate them to make two hands
    public void DoubleDown()
    {
        Hit(true);
        UserStand();
    }

    //give up a half-bet and retire from the game
    public void Surrender()
    {

    }

    //if the two cards have the same value, separate them to make two hands
    public void Split()
    {

    }

    private EGameState EvaluateCards()
    {
        if (_player.GetFaceUpCardValue() <= 21)
        {
            if (_dealer.GetFaceUpCardValue() > 21)
            {
                return EGameState.WON;
            }
            else if (_dealer.GetFaceUpCardValue() < _player.GetFaceUpCardValue())
            {
                return EGameState.WON;
            }
            else if (_dealer.GetFaceUpCardValue() == _player.GetFaceUpCardValue())
            {
                return EGameState.PUSH;
            }
            else if (_dealer.GetFaceUpCardValue() > _player.GetFaceUpCardValue())
            {
                return EGameState.BUST;
            }
        }
        return EGameState.BUST;
    }

}
