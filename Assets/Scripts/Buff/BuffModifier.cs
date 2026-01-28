using System;

[Serializable]
public class BuffModifier
{
    public float value;
    public BuffAddType type;
    public object source;

    public BuffModifier(float value, BuffAddType type, object source = null)
    {
        this.value = value;
        this.type = type;
        this.source = source;
    }
}
