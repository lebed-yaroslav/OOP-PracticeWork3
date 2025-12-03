using MathExpr;
using MathExpr.Operator;
using MathExpr.Functions;
using static MathExpr.Functions.BasicFunctions;

namespace MathExprTests;

// :)
[TestClass]
public class TestCopyConstructors
{
	private readonly Variable x = new("x");
	private readonly Variable y = new("y");
	private readonly Variable z = new("z");


	[TestMethod]
	public void AllCases() {
		var c = (Constant)4;
		Assert.AreEqual((Constant)5, c with { Value = 5 });
		Assert.AreEqual(y, x with { Name = "y" });
		Assert.AreEqual(-y, new Negate(x) with { Operand = y });
		Assert.AreEqual(x + y, new Add(y, z) with { Left = x, Right = y });
		Assert.AreEqual(x - y, new Subtract(y, z) with { Left = x, Right = y });
		Assert.AreEqual(x * y, new Multiply(y, z) with { Left = x, Right = y });
		Assert.AreEqual(x / y, new Divide(y, z) with { Left = x, Right = y });
		Assert.AreEqual(x ^ y, new Power(y, z) with { Left = x, Right = y });
		Assert.AreEqual(Sqrt(x), new Sqrt(y) with { Argument = x });
		Assert.AreEqual(Exp(x), new Exp(y) with { Argument = x });
		Assert.AreEqual(Log(x), new Log(y) with { Argument = x });
		Assert.AreEqual(Sinh(x), new Sinh(y) with { Argument = x });
		Assert.AreEqual(Cosh(x), new Cosh(y) with { Argument = x });
		Assert.AreEqual(Tanh(x), new Tanh(y) with { Argument = x });
	}
}
