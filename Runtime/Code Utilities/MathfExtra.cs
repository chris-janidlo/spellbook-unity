using System;

namespace crass
{
public static class MathfExtra
{
    public static int TernarySign (float value)
    {
        return value == 0 ? 0 : Math.Sign(value);
    }
}
}
