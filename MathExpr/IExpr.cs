using MathExpr.Util;

namespace MathExpr;


public partial interface IExpr
{
	IEnumerable<string> Variables { get; }
	int? PolynomialDegree { get; }
	double Compute(IReadOnlyDictionary<string, double> variables);
	T Accept<T>(IExprVisitor<T> visitor);
}


public static class IExprExt
{
	extension(IExpr expr)
	{
		public bool IsConstant => !expr.Variables.Any();
		public bool IsPolynomial => expr.PolynomialDegree.HasValue;

		/// <summary>
		/// Computes value if underlying expression is constant
		/// </summary>
		/// <exception cref="KeyNotFoundException"/>
		/// <returns>Computed value</returns>
		public double ComputeConstant() => expr.Compute(new Dictionary<string, double>());
	}
}
