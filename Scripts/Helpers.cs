public static class Helpers
{

    public static string NameAndType(INameable inameable)
    {
        return new string(inameable?.ObjectName + " " + inameable?.GetType());
    }

}