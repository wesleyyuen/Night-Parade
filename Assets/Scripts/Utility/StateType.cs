public class StateType
{
    public override string ToString()
    {
        return Value;
    }

    protected StateType(string value)
    {
        this.Value = value;
    }

    public string Value { get; protected set; }
}