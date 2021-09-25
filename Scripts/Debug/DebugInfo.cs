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