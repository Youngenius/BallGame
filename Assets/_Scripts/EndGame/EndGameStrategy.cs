using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public abstract class EndGameStrategy : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] protected Image _image;
    [SerializeField] protected Vector2 _imageStartScale;
    [SerializeField] protected Vector2 _imageNormalScale;
    [SerializeField] protected float _animationDuration;

    [Header("Other")]
    [SerializeField] protected TextMeshProUGUI _TMPro;
    [SerializeField] protected Button _button;

    public UnityEvent OnGameEnd;

    public virtual void EndGame()
    {
        OnGameEnd?.Invoke();

        _image.transform.DOScale(_imageStartScale, 0);
        _image.transform.DOScale(_imageNormalScale, _animationDuration).OnComplete(() =>
        {
            _TMPro.gameObject.SetActive(true);
            _button.interactable = true;
        });
    }
}


public class EndGameController : MonoBehaviour
{
    private EndGameStrategy _endGameStrategy;


    public void HandleEnd(EndGameBy end)
    {
        switch (end)
        {
            case EndGameBy.Win:
                _endGameStrategy = FindObjectOfType<WinGame>().GetComponent<WinGame>();
                break;

            case EndGameBy.Lose:
                _endGameStrategy = FindObjectOfType<LoseGame>().GetComponent<LoseGame>();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(end), end, null);
        }

        _endGameStrategy.EndGame();
    }
}

public enum EndGameBy
{
    Win,
    Lose
}