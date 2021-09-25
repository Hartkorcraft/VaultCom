using Godot;
using System;
using System.Collections.Generic;
public class UI : CanvasLayer
{
    public TextureButton PrimaryButton { get; private set; }
    public TextureButton UseButton { get; private set; }
    public Control Opened_UI { get; private set; }

    public static event Action primary_button_pressed;
    public static event Action use_button_pressed;

    public override void _EnterTree()
    {
        Opened_UI = (Control)GetNode("Opened_UI");
        PrimaryButton = (TextureButton)GetNode("ButtonsContainer/PrimaryButton");
        UseButton = (TextureButton)GetNode("ButtonsContainer/UseButton");
    }

    public void _on_PrimaryButton_button_down()
    {
        primary_button_pressed?.Invoke();
    }

    public void _on_UseButton_button_down()
    {
        use_button_pressed?.Invoke();
    }
}
