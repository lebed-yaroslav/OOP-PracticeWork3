using MathExpr;
using MathExpr.Functions;
using MathExpr.Extra;
using static MathExpr.Functions.BasicFunctions;

namespace MathExprTests;

[TestClass]
public sealed class TestDerivative
{
	private static readonly Variable x = new("x");
	private static readonly Variable y = new("y");
	private static readonly Variable z = new("z");


	[TestMethod]
	[DynamicData(nameof(FunctionDerivativeCases))]
	public void FunctionDerivativeAreCorrect(Func<Expr, Expr> function, Func<Expr, Expr> expected)
		=> Assert.AreEqual(expected(x), function(x).Derivative(x).Simplify());

	public static readonly IEnumerable<Func<Expr, Expr>[]> FunctionDerivativeCases = [
		[x => -x, x => -1],
		[Sqrt, x => 0.5 / Sqrt(x)],
		[Exp, Exp],
		[Log, x => 1 / x],
		[Sinh, Cosh],
		[Cosh, Sinh],
		[Tanh, x => 1 - (Tanh(x) ^ 2)]
	];


	[TestMethod]
	public void DerivativeNotDependsOnVariable()
		=> Assert.AreEqual(0, Sqrt(x + y).Derivative(z));


	[TestMethod]
	[DynamicData(nameof(OperatorDerivativeCases))]
	public void OperatorDerivativeAreCorrect(Func<Expr, Expr, Expr> op, Func<Expr, Expr, Expr> expected)
		=> Assert.AreEqual(expected(x, y), op(x, y).Derivative(x).Simplify(), message: $"Original expression: {op}");

	public static readonly IEnumerable<Func<Expr, Expr, Expr>[]> OperatorDerivativeCases = [
		[(x, y) => x + y, (x, _) => 1],
		[(x, y) => y - x, (x, _) => -1],
		[(x, _) => x * x, (x, _) => 2 * x],
		[(x, y) => x / y, (x, y) => 1 / y],
		[(x, y) => x ^ y, (x, y) => y * (x ^ y) / x]
	];
}
