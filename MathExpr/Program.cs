using MathExpr;
using MathExpr.Extra;
using static MathExpr.Functions.BasicFunctions;

internal static class TestProgram
{
	private static void Main(string[] args)
	{
		// FIXME: Use unit tests
		Console.WriteLine("[BasicTest]");
		RunBasicTest();
		Console.WriteLine();
		Console.WriteLine("[DeriativeTest1]");
		RunDeriativeTest1();
	}

	private static void RunBasicTest()
	{
		IExpr x = new Variable("x");
		IExpr y = new Variable("y");
		IExpr c = new Constant(3);
		var expr1 = (x - 4) * (3 * x + y * y) / 5;
		var expr2 = (5 - 3 * c) * Sqrt(16 + c * c);
		var inputValues = new Dictionary<string, double> { ["x"] = 1, ["y"] = 2 };
		WriteExprInfo(expr1);
		Console.WriteLine($"-Value (x = 1, y = 2): {expr1.Compute(inputValues)}");
		WriteExprInfo(expr2);
		Console.WriteLine($"-Value (x = 1, y = 2): {expr2.Compute(inputValues)}");
	}

	private static void RunDeriativeTest1()
	{
		IExpr x = new Variable("x");
		IExpr y = new Variable("y");
		IExpr c = new Constant(10);
		var inputValues = new Dictionary<string, double> { ["x"] = 1, ["y"] = 2 };
		var expr1 = Exp(Log(x + y + c));
		WriteExprInfo(expr1);
		Console.WriteLine($"-Value (x = 1, y = 2): {expr1.Compute(inputValues)}");
		Console.WriteLine($"(Simplified): {expr1.Simplify()}");
		Console.WriteLine($"(Simplified) d/dx: {expr1.Simplify().Deriative("x")}");
		Console.WriteLine($"d/dx (Non simplified): {expr1.Deriative("x")}");
		Console.WriteLine($"d/dx (Simplified): {expr1.Deriative("x").Simplify()}");
	}

	private static void WriteExprInfo(IExpr expr)
		=> Console.WriteLine($"""
		Expression: {expr}
		- IsConstant: {expr.IsConstant}
		- Variables: [{string.Join(", ", expr.Variables)}]
		- IsPolynomial: {expr.IsPolynomial}
		- Degree: {expr.PolynomialDegree}
		""");
}
