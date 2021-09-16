using Godot;
using System;

public class DebugConsole : Control
{

    TextEdit outputBox;
    LineEdit inputBox;

    public override void _EnterTree()
    {
        base._EnterTree();
        outputBox = (TextEdit)GetNode("Output");
        inputBox = (LineEdit)GetNode("Input");
    }

    public override void _Ready()
    {
        base._Ready();
        inputBox.GrabFocus();
    }

    public void OutputText(string text)
    {
        outputBox.Text += "\n" + text;
    }

    public void _on_input_text_entered(string new_text)
    {
        inputBox.Clear();
        if (new_text.Length == 0) { return; }
        OutputText(new_text);
        outputBox.CursorSetLine(outputBox.GetLineCount());
    }
}
