using System;
using System.Collections.Generic;

public class ConsoleCommandsManager
{
    Debug_Manager debug_Manager;
    DebugConsole console;
    public List<object> commandList;

    public static DebugCommand HELLO;
    public static DebugCommand<int, int> ADD;
    public static DebugCommand HELP;
    public static DebugCommand GET_PLAYER_CHARACTER_INFO;

    private void AddCommands()
    {
        HELLO = new DebugCommand("hello", "Prints Hello!", "hello", () =>
        {
            console.OutputText("* Hello!");
        });
        ADD = new DebugCommand<int, int>("add", "Adds numbers", "add <int> <int>", (num1, num2) =>
        {
            console.OutputText($"* {(num1 + num2).ToString()}");
        });
        HELP = new DebugCommand("help", "Prints commands and other help", "help", () =>
        {
            console.OutputText("* Available Commands:");
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
                console.OutputText($"- {commandBase.commandId} {commandBase.commandDescription} {commandBase.commandFormat}");
            }
        });
        GET_PLAYER_CHARACTER_INFO = new DebugCommand("get_player_info", "prints IGetInfoAble Info", "get_player_info", () =>
        {
            var index = 0;

            console.OutputText($"*Current selection: {Game_Manager.CurrentSelection?.ToString()}");
            foreach (var player_character in Game_Manager.GetPlayerInfoObjects())
            {
                console.OutputText("---------------------------------------------------");
                console.OutputText($"{index}. ");
                console.OutputText($"{player_character.GetInfo()}");
                index++;
            }
        });

        commandList = new List<object>
        {
            HELLO,
            ADD,
            HELP,
            GET_PLAYER_CHARACTER_INFO
        };
    }
    public bool HandleInput(string input)
    {
        string[] words = input.Trim().Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (string.Equals(words[0], commandBase.commandId, StringComparison.OrdinalIgnoreCase))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                    return true;
                }
                else if (commandList[i] as DebugCommand<int> != null && int.TryParse(words[1], out int num1))
                {
                    (commandList[i] as DebugCommand<int>).Invoke(num1);
                    return true;
                }
                else if (commandList[i] as DebugCommand<int, int> != null && int.TryParse(words[1], out int _num1) && int.TryParse(words[2], out int _num2))
                {
                    (commandList[i] as DebugCommand<int, int>).Invoke(_num1, _num2);
                    return true;
                }
            }
        }
        return false;
    }

    public ConsoleCommandsManager(Debug_Manager _debug_Manager, DebugConsole _console)
    {
        debug_Manager = _debug_Manager;
        console = _console;
        AddCommands();
    }
}