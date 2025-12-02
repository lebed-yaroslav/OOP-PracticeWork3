using MathExpr;

namespace MathExprTests;

[TestClass]
public sealed class TestBinaryOperations
{
	private readonly Variable x = new("x");
	private readonly Variable y = new("y");

	[TestMethod]
	public void NegateVariable()
	{
		var neg = -x;
		Assert.IsFalse(neg.IsConstant);
		Assert.ContainsSingle(neg.Variables);
		Assert.IsTrue(neg.IsPolynomial);
		Assert.AreEqual(1, neg.PolynomialDegree);
		Assert.AreEqual("(-x)", neg.ToString());
		Assert.AreEqual(10, neg.Compute(("x", -10)));
    }

	[TestMethod]
	public void SumVariables()
	{
		var sum = x + y;
		Assert.IsFalse(sum.IsConstant);
		Assert.Contains("x", sum.Variables);
		Assert.Contains("y", sum.Variables);
		Assert.IsTrue(sum.IsPolynomial);
		Assert.AreEqual(1, sum.PolynomialDegree);
		Assert.AreEqual("(x + y)", sum.ToString());
		Assert.AreEqual(13, sum.Compute(
			("x", -10), ("y", 23)
		));

		sum = x + (1 / y);
		Assert.IsFalse(sum.IsPolynomial);
	}

	[TestMethod]
	public void SubtractVariables() {
		var sub = x - y;
		Assert.IsFalse(sub.IsConstant);
		Assert.Contains("x", sub.Variables);
		Assert.Contains("y", sub.Variables);
		Assert.IsTrue(sub.IsPolynomial);
		Assert.AreEqual(1, sub.PolynomialDegree);
		Assert.AreEqual("(x - y)", sub.ToString());
		Assert.AreEqual(-33, sub.Compute(
			("x", -10), ("y", 23)
		));

		sub = x - (1 / y);
		Assert.IsFalse(sub.IsPolynomial);
	}

	[TestMethod]
	public void MultiplyVariables()
	{
		var mul = x * y;
		Assert.IsFalse(mul.IsConstant);
		Assert.Contains("x", mul.Variables);
		Assert.Contains("y", mul.Variables);
		Assert.IsTrue(mul.IsPolynomial);
		Assert.AreEqual(2, mul.PolynomialDegree);
		Assert.AreEqual("(x * y)", mul.ToString());
		Assert.AreEqual(1.5, mul.Compute(
			("x", 0.5), ("y", 3)
		));

		mul = x * (1 / y);
		Assert.IsFalse(mul.IsPolynomial);
	}

	[TestMethod]
	public void DivideVariables()
	{
		var div = x / y;
		Assert.IsFalse(div.IsConstant);
		Assert.Contains("x", div.Variables);
		Assert.Contains("y", div.Variables);
		Assert.IsFalse(div.IsPolynomial);
		Assert.AreEqual("(x / y)", div.ToString());
		Assert.AreEqual(3, div.Compute(
			("x", 3.6), ("y", 1.2)
		));

		div = x / 2;
		Assert.AreEqual(1, div.PolynomialDegree);
		div = 1 / x;
		Assert.IsFalse(div.IsPolynomial);
	}

	[TestMethod]
	public void PowerVariables()
	{
		var pow = x ^ 2;
		Assert.IsFalse(pow.IsConstant);
		Assert.Contains("x", pow.Variables);
		Assert.IsTrue(pow.IsPolynomial);
		Assert.AreEqual("(x ^ 2)", pow.ToString());

		pow ^= 1.2;
		Assert.IsFalse(pow.IsConstant);
		Assert.Contains("x", pow.Variables);
		Assert.IsFalse(pow.IsPolynomial);
		Assert.AreEqual("((x ^ 2) ^ 1.2)", pow.ToString());

		pow = x ^ y;
		Assert.IsFalse(pow.IsConstant);
		Assert.Contains("x", pow.Variables);
		Assert.Contains("y", pow.Variables);
		Assert.IsFalse(pow.IsPolynomial);
		Assert.AreEqual("(x ^ y)", pow.ToString());
		Assert.AreEqual(125, pow.Compute(
			("x", 25), ("y", 1.5)
		), double.Epsilon);

		pow = x ^ (-2);
		Assert.IsFalse(pow.IsPolynomial);
		pow = (1 / x) ^ 2;
		Assert.IsFalse(pow.IsPolynomial);
	}
}
