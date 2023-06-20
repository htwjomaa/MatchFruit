using System;
using TMPro;
[Serializable]
public struct BombMatchStyleList
{
    public TextMeshProUGUI Button;
    public TextMeshProUGUI ValueTextField;
    public Bomb bomb;

    public BombMatchStyleList(TextMeshProUGUI button, TextMeshProUGUI valueTextField, Bomb bomb)
    {
        Button = button;
        ValueTextField = valueTextField;
        this.bomb = bomb;
    }
}