using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blackjack.Cards;

public class DeckController : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefeb = null;
    [SerializeField] private Vector3 _cardOffset = Vector3.zero;

    private Dictionary<int, GameObject> _objectPool = new Dictionary<int, GameObject>();

    private List<int> _cardList = new List<int>();
    private const int MAX_CARDS_COUNT = 52;
    private int _count = 0;

    //for the start game 
    public void CreateDeck(int totalDeck)
    {
        _count = 0;
        _cardList.Clear();
        foreach (var obj in _objectPool)
        {
            obj.Value.SetActive(false);
        }

        for (int j = 0; j < totalDeck; ++j)
        {
            for (int i = 0; i < MAX_CARDS_COUNT; ++i)
            {
                _cardList.Add(i);
            }

        }
    }

    //shuffle the card 
    public void Shuffle()
    {
        int n = _cardList.Count;

        //Shuffle 
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int temp = _cardList[k];
            _cardList[k] = _cardList[n];
            _cardList[n] = temp;
        }
    }

    //show cards 
    public void ShowCards(bool isShown)
    {
        for (int i = 0; i < _cardList.Count; ++i)
        {
            ShowCard(i, isShown);
        }
    }
    public void ClearDeck()
    {
        _count = 0;
        _cardList.Clear();
        foreach (var obj in _objectPool)
        {
            obj.Value.SetActive(false);
        }
    }
    public void ShowCard(int index, bool isShown)
    {
        GameObject cardObj = null;

        if (_objectPool.ContainsKey(index))
        {
            cardObj = _objectPool[index];
            _objectPool[index].SetActive(true);
        }
        else
        {
            cardObj = (GameObject)Instantiate(_cardPrefeb);
            _objectPool.Add(index, cardObj);
        }

        float offsetX = _cardOffset.x * index;
        float offsetY = _cardOffset.y * index;
        float offsetZ = _cardOffset.z * index;


        cardObj.transform.parent = this.transform;
        cardObj.transform.position = this.transform.position + new Vector3(offsetX, offsetY, offsetZ);

        CardController cardController = cardObj.GetComponent<CardController>();
        CardInfo cardInfo = cardObj.GetComponent<CardInfo>();
        cardInfo.FrontCardIndex = _cardList[index];

        cardController.ShowFace(isShown);
    }

    //flip card animation
    public void FlipCard(int index)
    {
        if (_objectPool.ContainsKey(index))
        {
            _objectPool[index].SetActive(true);
            _objectPool[index].GetComponent<CardController>().FlipCard();
        }
    }

    //adding the card in deck 
    public int Pop()
    {
        int temp = -1;
        if (_cardList.Count > 0)
        {
            temp = _cardList[0];
            _cardList.RemoveAt(0);
            _objectPool[_count++].SetActive(false);
            return temp;
        }

        return temp;
    }

    //remove the card in deck
    public void Push(int cardIndex)
    {
        _cardList.Add(cardIndex);
        ShowCard(_cardList.Count - 1, false);
    }

    //total cards value 
    public int GetHandValue(bool checkTotalCardValueInHand)
    {
        int sum = 0;
        int aceCount = 0;
        for (int i = 0; i < _cardList.Count; ++i)
        {
            CardInfo cardInfo = _objectPool[i].GetComponent<CardInfo>();
            int cardNum = cardInfo.GetCardValue();
            if (cardInfo.IsFaceShown || checkTotalCardValueInHand)
            {
                //J Q K = 10 
                if (cardNum > 10)
                {
                    cardNum = 10;
                }
                //Ace 
                else if (cardNum == 0)
                {
                    aceCount++;
                }

                sum += cardNum;
            }
        }

        //Decide to make Ace's value 1 or 11 
        for (int i = 0; i < aceCount; ++i)
        {
            if (sum > 10 && aceCount > 0)
            {
                sum += 1;
            }
            else
            {
                sum += 11;
            }
        }

        return sum;
    }
    //check how many cards is aveable.
    public int GetTotalCards()
    {
        return _cardList.Count;
    }

}
