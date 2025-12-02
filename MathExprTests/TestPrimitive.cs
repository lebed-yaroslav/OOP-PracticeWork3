using MathExpr;
using System.Diagnostics.CodeAnalysis;

namespace MathExprTests;

[TestClass]
public sealed class TestConstant
{
	[TestMethod]
	public void ShouldComputeWithoutVariables()
	{
		var x = new Constant(5);
		Assert.IsTrue(x.TryComputeConstant(out var c));
		Assert.IsTrue(x.IsConstant);
		Assert.AreEqual(0, x.PolynomialDegree);
		Assert.AreEqual(x.Value, c);
	}

	[TestMethod]
	public void ShouldBeImplicitlyConverted()
	{
		Expr c = 5;
		Assert.IsTrue(c is Constant);
	}

	[TestMethod]
	public void ToStringShouldWrapNegative()
	{
		Expr c = 10.2;
		Assert.AreEqual("10.2", c.ToString());
		c = -10.2;
		Assert.AreEqual("(-10.2)", c.ToString());
	}
}

[TestClass]
public sealed class TestVariable
{
	[TestMethod]
	public void ShouldComputeFromDictionary()
	{
		var x = new Variable("x");
		var result = x.Compute(new Dictionary<string, double> {
			["x"] = 3
		});
        Assert.IsFalse(x.TryComputeConstant(out var c));
        Assert.AreEqual(0, c);
        Assert.IsFalse(x.DependsOn("y"));
        Assert.AreEqual(3, result);
		Assert.ContainsSingle(x.Variables, "x");
		Assert.AreEqual(1, x.PolynomialDegree);
	}

	[TestMethod]
	[ExcludeFromCodeCoverage]
	public void ShouldThrowOnComputeConstant()
	{
		var x = new Variable("x");
		Assert.ThrowsExactly<KeyNotFoundException>(() => x.ComputeConstant());
	}
}
