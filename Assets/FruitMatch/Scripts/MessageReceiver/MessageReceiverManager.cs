using System;
using TMPro;
using UnityEngine;

public class MessageReceiverManager : MonoBehaviour
{
    public TextMeshProUGUI MessageOneRef;
    public TextMeshProUGUI MessageTwoRef;
    public TextMeshProUGUI MessageThreeRef;
    public TextMeshProUGUI MessageFourRef;

    public MessageReceiver MessageReceiver;
    
    public static TextMeshProUGUI MessageOne;
    public static TextMeshProUGUI MessageTwo;
    public static TextMeshProUGUI MessageThree;
    public static TextMeshProUGUI MessageFour;
    public Color LevelAttributeColor = Color.green;

    private void Awake()
    {
        MessageOne = MessageOneRef;
        MessageTwo = MessageTwoRef;
        MessageThree = MessageThreeRef;
        MessageFour = MessageFourRef;
    }

    private void Start()
    {
        LoadLastestMessages();
    }

    public void LoadLastestMessages() => MessageReceiver.LoadLatestMessages();
}
