using Godot;
using System;
using System.Collections.Generic;
public class UI : CanvasLayer
{

    public Control Opened_UI { get; private set; }
    //public HashSet<Popup> MouseOverPopup { get; } = new HashSet<Popup>();
    public override void _EnterTree()
    {
        Opened_UI = (Control)GetNode("Opened_UI");
    }

}
