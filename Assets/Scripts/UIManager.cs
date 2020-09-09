using UnityEngine;
using UnityEngine.UI;
using Blackjack.Common;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BlackjackGameManager _gameManager = null;
    [SerializeField] private GameObject _dealButton = null;
    [SerializeField] private GameObject _GamePlayActionButtons = null;
    [SerializeField] private Button _settingButton = null;
    [SerializeField] private TextMeshProUGUI _dealerHandCardValue = null;
    [SerializeField] private TextMeshProUGUI _playerHandCardValue = null;
    [SerializeField] private TextMeshProUGUI _resultText = null;


    public void SetPlayerHandCardValue(int value)
    {
        _playerHandCardValue.text = string.Format("Player : {0}", value);
    }

    public void SetDealerHandCardValue(int value)
    {
        _dealerHandCardValue.text = string.Format("Dealer : {0}", value);
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
        _resultText.gameObject.SetActive(true);
        GameOver();
    }

    public void OnClickDealButton()
    {
        _gameManager.StartGame();
    }

    public void StartGame()
    {
        _dealButton.gameObject.SetActive(false);
        _resultText.gameObject.SetActive(false);
        _GamePlayActionButtons.SetActive(true);
    }

    private void GameOver()
    {
        _dealButton.gameObject.SetActive(true);
        _resultText.gameObject.SetActive(true);
        _GamePlayActionButtons.SetActive(false);
    }

}
