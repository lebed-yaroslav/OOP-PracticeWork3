using MathExpr;
using MathExpr.Extra;
using static MathExpr.Functions.BasicFunctions;
using MathExpr.Operator;

namespace MathExprTests;

[TestClass]
public sealed class TestSimplify
{
	private static readonly Variable x = new("x");
	private static readonly Variable y = new("y");
	private static readonly Variable z = new("z");


	[TestMethod]
	[DynamicData(nameof(FunctionSimplifyCases))]
	public void FunctionsSimplifyAreCorrect(Func<Expr, Expr> function, Func<Expr, Expr> expected)
		=> Assert.AreEqual(expected(x), function(x).Simplify());

	public static readonly IEnumerable<Func<Expr, Expr>[]> FunctionSimplifyCases = [
		[x => -(-x), x => x],
		[x => Exp(Log(x)), x => x],
		[x => Log(Exp(x)), x => x],
	];


	[TestMethod]
	[DynamicData(nameof(ConstantComputeCases))]
	public void ConstantComputeIsCorrect(Expr expr, Constant expected)
		=> Assert.AreEqual(expected, expr.Simplify());

	public static readonly IEnumerable<Expr[]> ConstantComputeCases = [
		[new Negate(1), -1],
		[Sqrt(1), 1],
		[Exp(1), Math.E],
		[Log(1), 0],
		[Sinh(0), 0],
		[Cosh(0), 1],
		[Tanh(0), 0]
	];


	[TestMethod]
	public void SimplifySqrtOfPowerCorrect()
	{
		var expr = x ^ 5;
		Assert.AreEqual(Sqrt(x ^ y), Sqrt(x ^ y).Simplify());
		Assert.AreEqual(x, Sqrt(x ^ 2).Simplify());
		Assert.AreEqual(x ^ 2, Sqrt(x ^ 4).Simplify());
	}

	[TestMethod]
	[DynamicData(nameof(ConstantFoldingCases))]
	public void ConstantFoldingIsCorrect(Expr expr, Expr expected)
		=> Assert.AreEqual(expected, expr.Simplify(), message: $"Original expression: {expr}");

	public static readonly IEnumerable<Expr[]> ConstantFoldingCases = [
		// Add
		[x + 0, x], [0 + x, x], [x + x, 2 * x], [x + x + 0 + 0, 2 * x],
		// Subtract
		[x - 0, x], [0 - x, -x], [x - x, 0], [x - x + x, x],
		// Multiply
		[x * 0, 0], [0 * x, 0], [x * 1, x], [1 * x, x], [x * x, x ^ 2],
		[3 * x, 3 * x], [x * 3, x * 3],
		// Divide
		[0 / x, 0], [x / 1, x], [x / x, 1],
		// Power
		[x ^ 0, 1], [x ^ 1, x], [x ^ (-1), 1 / x], [1 ^ x, 1],
		[0 ^ x, 0], [x ^ 3, x ^ 3], [3 ^ x, 3 ^ x],
		// Misc
		[x * (1 / x), 1], [(1 / x) * x, 1]
	];


	[TestMethod]
	public void ExprAreUnitedUnderCommonFactor()
	{
		Assert.AreEqual((x + y) / z, ((x / z) + (y / z)).Simplify());
		Assert.AreEqual((x - y) / z, ((x / z) - (y / z)).Simplify());
	}


	[TestMethod]
	public void NestedDividesAreFlip()
	{
		Assert.AreEqual((x * y) / z, (x / (z / y)).Simplify());
		Assert.AreEqual((x ^ 2) / (z ^ 2), (x / (z / (x / z))).Simplify());
	}


	[TestMethod]
	[DynamicData(nameof(DivisionOfPowersCases))]
	public void DivisionOfPowersAreCorrect(Expr expr, Expr expected)
		=> Assert.AreEqual(expected, expr.Simplify(), message: $"Original expression: {expr}");

	public static readonly IEnumerable<Expr[]> DivisionOfPowersCases = [
		[(x ^ y) / x, x ^ (y - 1)],
		[x / (x ^ y), x ^ (1 - y)],
		[(x ^ 1.5) / (x ^ 0.5), x]
	];


	[TestMethod]
	[DynamicData(nameof(MultiplyDistributionCases))]
	public void MultiplyDistributionIsCorrect(Expr expr, Expr expected)
		=> Assert.AreEqual(expected, expr.Simplify(), message: $"Original expression: {expr}");

	public static readonly IEnumerable<Expr[]> MultiplyDistributionCases = [
		[x * (y + z), x * y + x * z],
		[(y + z) * x, x * y + x * z],
		[x * (y - z), x * y - x * z],
		[(y - z) * x, x * y - x * z]
	];
}
