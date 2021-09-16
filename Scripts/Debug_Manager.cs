using Godot;
using System;
using System.Collections.Generic;
using HartLib;
using static HartLib.Utils;

public class Debug_Manager : Control
{

    Label debugInfoLabel;
    DebugConsole debugConsole;
    /// Use AddLog, DeleteLog, UpdateLog and ClearLogs()
    Dictionary<string, DebugInfo> logs = new Dictionary<string, DebugInfo>();
    string debugText = "";


    public override void _EnterTree()
    {
        debugInfoLabel = (Label)GetChild(0);
        debugConsole = (DebugConsole)GetChild(1);
        debugInfoLabel.Text = debugText;
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
        if (logs.ContainsKey(name) is false) { throw new Exception("no log " + name); }
        logs[name].LabelText = logText;
        logs[name].Display = display;
        UpdateLogsDisplay();
    }


    public void UpdateLogsDisplay()
    {
        string debugText = "";

        foreach (KeyValuePair<string, DebugInfo> log in logs)
        {
            if (log.Value.Display is false) continue;
            debugText += log.Value.Name + ": " + log.Value.LabelText + NewLine;
        }
        debugInfoLabel.Text = debugText;
    }
}
public class DebugInfo
{
    public string Name { get; }
    public string LabelText { get; set; }
    public bool Display { get; set; }

    public DebugInfo(string name, string labelTest = "", bool display = true)
    {
        Name = name;
        LabelText = labelTest;
        Display = display;
    }
}
