using System;
using System.Collections;
using System.Collections.Generic;

public class DebugCommandBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string commandId { get { return _commandId; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }

    public DebugCommandBase(string id, string description, string format)
    {
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;
    public DebugCommand(string id, string description, string format, Action _command) : base(id, description, format)
    {
        command = _command;
    }
    public void Invoke()
    {
        command.Invoke();
    }
}
public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;
    public DebugCommand(string id, string description, string format, Action<T1> _command) : base(id, description, format)
    {
        command = _command;
    }
    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}
public class DebugCommand<T1, T2> : DebugCommandBase
{
    private Action<T1, T2> command;
    public DebugCommand(string id, string description, string format, Action<T1, T2> _command) : base(id, description, format)
    {
        command = _command;
    }
    public void Invoke(T1 value1, T2 value2)
    {
        command.Invoke(value1, value2);
    }
}
