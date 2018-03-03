using System;

public class ThresholdAction
{
    bool wasReached;
    Action onPress;
    Action onRelease;
    float threshold;

    public ThresholdAction(float threshold, Action onPress, Action onRelease)
    {
        this.onPress = onPress;
        this.onRelease = onRelease;
        this.threshold = threshold;
    }

    public void SetMagnitude(float pressure)
    {
        bool isReached = pressure >= threshold;
        if (onPress != null && !wasReached && isReached)
        {
            onPress();
        }
        else if (onRelease != null && wasReached && !isReached)
        {
            onRelease();
        }
        wasReached = isReached;
    }
}
