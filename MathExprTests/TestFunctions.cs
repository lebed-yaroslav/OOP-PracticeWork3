using MathExpr;
using MathExpr.Extra;
using MathExpr.Functions;
using static MathExpr.Functions.BasicFunctions;

namespace MathExprTests;

[TestClass]
public sealed class TestFunctions
{
	private static readonly Variable x = new("x");


	[TestMethod]
	[DynamicData(nameof(FunctionPropertiesCases))]
	public void FunctionsPropertiesAreCorrect(Func<Expr, Expr> function, bool isConstant, bool isPolynomial, string asString)
	{
		var func = function(x);
		Assert.AreEqual(isConstant, func.IsConstant);
		Assert.ContainsSingle(func.Variables);
		Assert.AreEqual(isPolynomial, func.IsPolynomial);
		Assert.AreEqual(asString, func.ToString());
	}

#pragma warning disable CS8974 // Converting method group to non-delegate type
	private static readonly IEnumerable<object[]> FunctionPropertiesCases = [
		[Sqrt, /*isConstant=*/false, /*isPolynomial*/false, "Sqrt(x)"],
		[Exp, /*isConstant=*/false, /*isPolynomial*/false, "Exp(x)"],
		[Log, /*isConstant=*/false, /*isPolynomial*/false, "Log(x)"],
		[Sinh, /*isConstant=*/false, /*isPolynomial*/false, "Sinh(x)"],
		[Cosh, /*isConstant=*/false, /*isPolynomial*/false, "Cosh(x)"],
		[Tanh, /*isConstant=*/false, /*isPolynomial*/false, "Tanh(x)"]
	];
#pragma warning restore CS8974 // Converting method group to non-delegate type


	[TestMethod]
	public void FunctionWithConstantHasDegreeZero()
	{
		var func = Log(1);
		Assert.IsTrue(func.IsConstant);
		Assert.AreEqual(0, func.PolynomialDegree);
		Assert.AreEqual(0, func.ComputeConstant());
	}


	[TestMethod]
	[DynamicData(nameof(FunctionComputeTestCases))]
	public void FunctionComputesCorrectly(
	Func<Expr, Expr> function,
	double input,
	double expected)
	{
		var expr = function(x);
		Assert.AreEqual(expected, expr.Compute(("x", input)), 1e-14);
	}

#pragma warning disable CS8974 // Converting method group to non-delegate type
	private static readonly IEnumerable<object[]> FunctionComputeTestCases = [
		[Sqrt, 1, 1], [Sqrt, 4, 2], [Sqrt, 25, 5],
		[Exp, 0, 1], [Exp, 1, Math.E], [Exp, 2, Math.E * Math.E],
		[Log, 1, 0], [Log, Math.E, 1], [Log, Math.E * Math.E, 2],
		[Sinh, 0, 0], [Cosh, 0, 1], [Tanh, 0, 0]
	];
#pragma warning restore CS8974 // Converting method group to non-delegate type
}
