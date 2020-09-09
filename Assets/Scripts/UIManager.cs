using UnityEngine;
using UnityEngine.UI;
using Blackjack.Common;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BlackjackGameManager _gameManager = null;
    [SerializeField] private GameObject _gameStartView = null;
    [SerializeField] private GameObject _GamePlayActionButtons = null;
    [SerializeField] private TextMeshProUGUI _dealerHandCardValue = null;
    [SerializeField] private TextMeshProUGUI _playerHandCardValue = null;
    [SerializeField] private GameObject _resultPanel = null;
    [SerializeField] private TextMeshProUGUI _resultText = null;
    [SerializeField] private TextMeshProUGUI _battingAmount = null;
    [SerializeField] private TextMeshProUGUI _currentUserBalance = null;

    public void SetPlayerHandCardValue(int value)
    {
        _playerHandCardValue.text = string.Format("Player : {0}", value);
    }

    public void SetDealerHandCardValue(int value)
    {
        _dealerHandCardValue.text = string.Format("Dealer : {0}", value);
    }

    public void SetTotalBattingChipText(double batAmount)
    {
        _battingAmount.text = string.Format("Batting : {0}", batAmount);
    }

    public void SetBalanceText(double balance)
    {
        _currentUserBalance.text = string.Format("Balance : {0}", balance);
    }
    public void OnClickedSaveButton()
    {
        _gameManager.SaveData();
    }

    public void OnClickedLoadButton()
    {
        _gameManager.LoadData();
    }
    public void OnClickedBattingButton(int batting)
    {
        _gameManager.BattingChip((double)batting);
    }


    public void OnClickedDoubleDownButton()
    {
        _gameManager.DoubleDown();
    }
    public void OnClickedHitButton()
    {
        _gameManager.Hit(true);
    }

    public void OnClickedStandButton()
    {
        _gameManager.UserStand();
    }

    public void OnClickedChangeStandardDecksButton(int value)
    {
        _gameManager.MaxDeck = value;
    }

    public void SetTheResultText(EGameState state)
    {
        string text = "";
        switch (state)
        {
            case EGameState.NONE:
                text = "RESULT";
                break;
            case EGameState.PUSH:
                text = "Push!";
                break;
            case EGameState.BUST:
                text = "Bust!";
                break;
            case EGameState.WON:
                text = "Won!";
                break;
        }
        _resultText.text = text;
        _resultPanel.gameObject.SetActive(true);
        GameOver();
    }

    public void OnClickedSurrenderButton()
    {
        _gameManager.Surrender();
    }
    public void OnClickDealButton()
    {
        _gameManager.StartGame();
    }

    public void StartGame()
    {
        _gameStartView.gameObject.SetActive(false);
        _resultPanel.gameObject.SetActive(false);
        _GamePlayActionButtons.SetActive(true);
    }

    private void GameOver()
    {
        _gameStartView.gameObject.SetActive(true);
        _resultPanel.gameObject.SetActive(true);
        _GamePlayActionButtons.SetActive(false);
    }

}
