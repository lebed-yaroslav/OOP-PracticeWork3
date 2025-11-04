namespace MathExpr.Util;

public static class EnumerableExt
{
	public static IEnumerable<T> Yield<T>(this T item)
	{
		yield return item;
	}
}
