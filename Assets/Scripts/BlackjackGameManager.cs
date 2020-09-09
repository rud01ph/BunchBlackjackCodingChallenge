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


    ESideGameState _sideGameState = ESideGameState.NONE;
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

    //Do shuffle when it's has 20 cards left/
    public void ShuffleDeck()
    {
        _deck.CreateDeck(maxDeck);
        _deck.Shuffle();
        _deck.ShowCards(false);

    }

    // Deal 
    public void Deal()
    {
        _sideGameState = ESideGameState.NONE;
        _gameUIManager.StartGame();
        _gameUIManager.SetTotalBattingChipText(_player.GetBattingFee());
        _gameUIManager.SetBalanceText(_player.GetBalance());

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

        //check player is black jack or not
        if (_player.GetTotalHandCardValue() == 21)
        {
            _player.IsBlackJack = true;
        }

        _gameUIManager.SetPlayerHandCardValue(_player.GetFaceUpCardValue());
        _gameUIManager.SetDealerHandCardValue(_dealer.GetFaceUpCardValue());
    }

    public void StartGame()
    {
        //when the cards remain less than 20, it start shuffle again.
        if (_deck.GetTotalCards() <= 20)
        {
            ShuffleDeck();
        }

        //if the player batting is less than 0, you cannot start the game 
        if (_player.GetBattingFee() > 0)
        {
            Deal();
        }
        else
        {
            Debug.Log("Please, batting money");
        }

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
                ShowResult();
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

        yield return new WaitForSeconds(1f);
        ShowResult();
    }


    public void BattingChip(double value)
    {
        CanBatting(value);
    }

    //if the two cards have the same value, separate them to make two hands
    public void DoubleDown()
    {
        _sideGameState = ESideGameState.DOUBLE;

        if (CanBatting(_player.GetBattingFee()))
        {
            Hit(true);
            UserStand();
        }
    }

    private bool CanBatting(double value)
    {
        if (!_player.SetBatting(value))
        {
            Debug.Log("The money is no enough");
            return false;
        }
        _gameUIManager.SetTotalBattingChipText(_player.GetBattingFee());
        _gameUIManager.SetBalanceText(_player.GetBalance());
        return true;
    }

    //give up a half-bet and retire from the game
    public void Surrender()
    {
        _sideGameState = ESideGameState.SURRENDER;
        UserStand();
    }

    private EGameState EvaluateCards()
    {
        int playerCardValue = _player.GetFaceUpCardValue();
        int dealerCardValue = _dealer.GetFaceUpCardValue();

        if (_sideGameState == ESideGameState.SURRENDER)
            return EGameState.BUST;

        if (playerCardValue <= 21)
        {
            if (dealerCardValue > 21)
            {
                return EGameState.WON;
            }
            else if (dealerCardValue < playerCardValue)
            {
                return EGameState.WON;
            }
            else if (dealerCardValue == playerCardValue)
            {
                return EGameState.PUSH;
            }
            else if (dealerCardValue > playerCardValue)
            {
                return EGameState.BUST;
            }
        }
        return EGameState.BUST;

    }

    void ShowResult()
    {
        EGameState result = EvaluateCards();

        if (result == EGameState.WON)
        {
            if (_player.IsBlackJack)
            {
                _player.AddBalance(_player.GetBattingFee() * 2.5);
            }
            else
            {
                _player.AddBalance(_player.GetBattingFee() * 2);
            }
        }
        else if (result == EGameState.BUST)
        {
            if (_sideGameState == ESideGameState.SURRENDER)
            {
                _player.AddBalance(_player.GetBattingFee() * 0.5);
            }
        }
        else
        {
            _player.AddBalance(_player.GetBattingFee());
        }

        _player.ClearHand();
        _dealer.ClearHand();
        _gameUIManager.SetTheResultText(result);
    }

    public void SaveData()
    {
        SaveAndLoad.SavePlayerInfo(_dealer.GetUserID(), _dealer.GetBalance());
        SaveAndLoad.SavePlayerInfo(_player.GetUserID(), _player.GetBalance() + _player.GetBattingFee());
    }
    public void LoadData()
    {
        _dealer.SetUserInfo(SaveAndLoad.LoadPlayerInfo(_dealer.GetUserID()));
        _player.SetUserInfo(SaveAndLoad.LoadPlayerInfo(_player.GetUserID()));
        _gameUIManager.SetBalanceText(_player.GetBalance());
    }

}
