using System;

[Serializable]
public class BuffModifier
{
    public AddValue[] addValues;
    public object source;

    public BuffModifier(AddValue[] addValues, object source = null)
    {
        this.addValues = addValues;
        this.source = source;
    }
}
