
class UtilityFunctions
{
    public static int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }

    public static float Wrap(float value, float min, float max)
    {
        float diff = max - min;
        if (value < min) {
            value += diff;
        }
        else if(value > max)
        {
            value -= diff;
        }
        return value;
    }

}