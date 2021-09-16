using System;
using System.Collections.Generic;

public class ConsoleCommandsManager
{

    DebugConsole console;
    public List<object> commandList;

    public static DebugCommand HELLO;
    public static DebugCommand<int, int> ADD;

    private void AddCommands()
    {
        HELLO = new DebugCommand("hello", "Prints Hello!", "hello", () =>
        {
            console.OutputText("Hello!");
        });
        ADD = new DebugCommand<int, int>("add", "Adds numbers", "add<int,int>", (num1, num2) =>
         {
             console.OutputText((num1 + num2).ToString());
         });

        commandList = new List<object> {
            HELLO,
            ADD
             };
    }
    public void HandleInput(string input)
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
                }
                else if (commandList[i] as DebugCommand<int> != null && int.TryParse(words[1], out int num1))
                {
                    //int result;
                    (commandList[i] as DebugCommand<int>).Invoke(num1);
                }
                else if (commandList[i] as DebugCommand<int, int> != null && int.TryParse(words[1], out int _num1) && int.TryParse(words[2], out int _num2))
                {
                    (commandList[i] as DebugCommand<int, int>).Invoke(_num1, _num2);
                }
            }
        }
    }

    public ConsoleCommandsManager(DebugConsole _console)
    {
        console = _console;
        AddCommands();
    }
}