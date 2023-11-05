using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishMenu : Menu<FinishMenu>
{
    [SerializeField] private Text finishText;
    [SerializeField] private Button buttonFinish;
    [SerializeField] private Text buttonfinishText;

    private bool? IsWinning;
    protected override void Awake()
    {
        base.Awake(); 
    }
    public void OnButtonFinishClick()
    {
        if (IsWinning.HasValue)
        {
            if (IsWinning.Value)
            {
                
            }
            else
            {
                Debug.Log("Thua roi");

            }
        }
    }
    public void SetWinningState(bool isWinning)
    {
        IsWinning = isWinning;
        UpdateFinishText();
    }
    private void UpdateFinishText()
    {
        if (IsWinning.HasValue)
        {
            finishText.text = IsWinning.Value ? "You Win!" : "Bot Wins!";
            buttonfinishText.text = IsWinning.Value ? "Next Level!" : "Retry!";
        }
    }
    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        Close();
    }
}
