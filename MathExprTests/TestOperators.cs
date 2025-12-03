using MathExpr;

namespace MathExprTests;

[TestClass]
public sealed class TestBinaryOperations
{
	private static readonly Variable x = new("x");
	private static readonly Variable y = new("y");

	[TestMethod]
	public void NegateVariable()
	{
		var neg = -x;
		Assert.IsFalse(neg.IsConstant);
		Assert.ContainsSingle(neg.Variables);
		Assert.IsTrue(neg.IsPolynomial);
		Assert.AreEqual(1, neg.PolynomialDegree);
		Assert.AreEqual("(-x)", neg.ToString());
	}


	[TestMethod]
	[DynamicData(nameof(OperatorPropertiesCases))]
	public void OperatorPropertiesAreCorrect(Func<Expr, Expr, Expr> op, bool isConstant, int? polynomialDegree, string asString)
	{
		var oper = op(x, y);
		Assert.AreEqual(isConstant, oper.IsConstant);
		Assert.Contains("x", oper.Variables);
		Assert.Contains("y", oper.Variables);
		Assert.AreEqual(polynomialDegree.HasValue, oper.IsPolynomial);
		Assert.AreEqual(polynomialDegree, oper.PolynomialDegree);
		Assert.AreEqual(asString, oper.ToString());
	}

	private static readonly IEnumerable<object[]> OperatorPropertiesCases = [
		[(Expr x, Expr y) => x + y, /*isConstant=*/false, /*polynomialDegree*/1, "(x + y)"],
		[(Expr x, Expr y) => x - y, /*isConstant=*/false, /*polynomialDegree*/1, "(x - y)"],
		[(Expr x, Expr y) => x * y, /*isConstant=*/false, /*polynomialDegree*/2, "(x * y)"],
		[(Expr x, Expr y) => x / y, /*isConstant=*/false, /*polynomialDegree*/new int?(), "(x / y)"],
		[(Expr x, Expr y) => x ^ y, /*isConstant=*/false, /*polynomialDegree*/new int?(), "(x ^ y)"]
	];


	[TestMethod]
	[DynamicData(nameof(OperatorComputeCases))]
	public void OperatorsComputesCorrectly(
		Func<Expr, Expr, Expr> op,
		double inputX,
		double inputY,
		double expected
	)
	{
		var expr = op(x, y);
		Assert.AreEqual(expected, expr.Compute(("x", inputX), ("y", inputY)), message: $"Operator: {op}");
	}

	private static readonly IEnumerable<object[]> OperatorComputeCases = [
		[(Expr x, Expr _) => -x, 2.5, 0, -2.5],
		[(Expr x, Expr y) => x + y, 2, 3, 5],
		[(Expr x, Expr y) => x - y, 2, 3, -1],
		[(Expr x, Expr y) => x * y, -3, 3.5, -10.5],
		[(Expr x, Expr y) => x / y, 3.6, 1.2, 3],
		[(Expr x, Expr y) => x ^ y, 25, 1.5, 125]
	];


	[TestMethod]
	public void SumNonPolynomial()
	{
		var sum = x + (1 / y);
		Assert.IsFalse(sum.IsPolynomial);
	}


	[TestMethod]
	public void SubtractNonPolynomial() {
		var sub = x - (1 / y);
		Assert.IsFalse(sub.IsPolynomial);
	}


	[TestMethod]
	public void MultiplyNonPolynomial()
	{
		var mul = x * (1 / y);
		Assert.IsFalse(mul.IsPolynomial);
	}


	[TestMethod]
	public void DividePolynomialCases()
	{
		var div = x / 2;
		Assert.AreEqual(1, div.PolynomialDegree);
		div = 1 / x;
		Assert.IsFalse(div.IsPolynomial);
	}


	[TestMethod]
	public void PowerPolynomialCases()
	{
		var pow = x ^ 2;
		Assert.IsTrue(pow.IsPolynomial);
		pow ^= 1.2;
		Assert.IsFalse(pow.IsPolynomial);
		pow = x ^ y;
		Assert.IsFalse(pow.IsPolynomial);
		Assert.AreEqual("(x ^ y)", pow.ToString());
		pow = x ^ (-2);
		Assert.IsFalse(pow.IsPolynomial);
		pow = (1 / x) ^ 2;
		Assert.IsFalse(pow.IsPolynomial);
	}
}
