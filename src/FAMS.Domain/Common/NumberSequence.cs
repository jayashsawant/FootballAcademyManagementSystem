namespace FAMS.Domain.Common;

public class NumberSequence
{
    public string Prefix { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public int LastNumber { get; private set; }

    private NumberSequence() { }
}