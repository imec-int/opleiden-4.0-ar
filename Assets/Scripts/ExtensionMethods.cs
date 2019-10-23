using UnityEngine;
using System;

public static class ExtensionMethods
{
	public static float RemapValue(this float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public static Rect ToScreenSpace(this RectTransform transform)
	{
		Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
		Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
		rect.x -= (transform.pivot.x * size.x);
		rect.y -= ((1.0f - transform.pivot.y) * size.y);
		return rect;
	}

	public static string ToUnicodeForTMPro(this string iconUnicode)
	{
		iconUnicode = iconUnicode.Replace(@"\u", "");
		try
		{
			int unicode = int.Parse(iconUnicode, System.Globalization.NumberStyles.HexNumber);
			return char.ConvertFromUtf32(unicode);
		}
		catch (FormatException e)
		{
			Debug.LogWarning($"Failed to parse {iconUnicode} for TMPro: {e}");
		}
		return iconUnicode;
	}

	public static string ToHexCodeString(this Color color)
	{
        var colorBase255 = new Vector3(color.r, color.g, color.b) * 255;
        return $"#{(int)colorBase255.x:X2}{(int)colorBase255.y:X2}{(int)colorBase255.z:X2}";
	}
}
