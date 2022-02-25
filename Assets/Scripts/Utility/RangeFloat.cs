using System;


// By Unity
// https://github.com/richard-fine/scriptable-object-demo

[Serializable]
public struct RangeFloat
{
	public float minValue;
	public float maxValue;
	public RangeFloat(float min, float max)
	{
		minValue = min;
		maxValue = max;
	}
}

public class MinMaxRangeAttribute : Attribute
{
	public MinMaxRangeAttribute(float min, float max)
	{
		Min = min;
		Max = max;
	}
	public float Min { get; private set; }
	public float Max { get; private set; }
}