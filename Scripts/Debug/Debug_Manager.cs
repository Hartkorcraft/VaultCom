using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class Debug_Manager : Control
{
    public DebugConsole debugConsole { get; private set; }

    Label debugInfoLabel;
    Dictionary<string, DebugInfo> logs = new Dictionary<string, DebugInfo>(); // Use AddLog, DeleteLog, UpdateLog and ClearLogs()
    string debugLabelText = "";

    public string GetLogDisplay => debugInfoLabel?.Text;

    public override void _EnterTree()
    {
        debugInfoLabel = (Label)GetChild(0);
        debugConsole = (DebugConsole)GetChild(1);
        debugInfoLabel.Text = debugLabelText;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("Toggle_Debug_Menu"))
        { debugConsole.Visible = !debugConsole.Visible; }
    }

    public void AddLog(DebugInfo log)
    {
        logs.Add(log.Name, log);
        UpdateLogsDisplay();
    }

    public void DeleteLog(string name)
    {
        logs.Remove(name);
        UpdateLogsDisplay();
    }

    public void ClearLogs()
    {
        logs.Clear();
        UpdateLogsDisplay();
    }

    public void UpdateLog(string name, string logText, bool display = true)
    {
        if (logs.ContainsKey(name) is false) { GD.PrintErr("no log " + name); }
        logs[name].LabelText = logText;
        logs[name].Display = display;
        UpdateLogsDisplay();
    }


    public void UpdateLogsDisplay()
    {
        string debugLabelText = "";

        foreach (KeyValuePair<string, DebugInfo> log in logs)
        {
            if (log.Value.Display is false) continue;
            debugLabelText += log.Value.Name + ": " + log.Value.LabelText + NewLine;
        }
        debugInfoLabel.Text = debugLabelText;
    }
}

