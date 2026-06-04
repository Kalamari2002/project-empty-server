public struct EventMessage
{
    public string title;
    public object[] arguments;

    public EventMessage(string title, object[] arguments)
    {
        this.title = title;
        this.arguments = arguments;
    }
}
