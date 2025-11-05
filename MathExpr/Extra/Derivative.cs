using MathExpr.Functions;
using MathExpr.Operator;
using static MathExpr.Functions.BasicFunctions;

namespace MathExpr.Extra;


public static class DerivativeExt
{
	extension(Expr expr)
	{
		public Expr Derivative(string v)
			=> expr switch {
				Variable(var x) => new Constant((x == v)  ? 1 : 0),
				Constant => Constant.Zero,
				Negate(var a) => -a.Derivative(v),
				Add(var a, var b) => a.Derivative(v) + b.Derivative(v),
				Subtract(var a, var b) => a.Derivative(v) - b.Derivative(v),
				Multiply(var a, var b) => a.Derivative(v) * b + a * b.Derivative(v),
				Divide(var a, var b) => (a.Derivative(v) * b - a * b.Derivative(v)) / (b ^ 2),
				Power(var a, var b) => (a ^ b) * (b.Derivative(v) * Log(a) + b / a * a.Derivative(v)),
				Sqrt sqrt => 0.5 * sqrt.Argument.Derivative(v) / sqrt,
				Exp exp => exp.Argument.Derivative(v) * exp,
				Log(var a) => a.Derivative(v) / a,
				Sinh(var a) => a.Derivative(v) * Cosh(a),
				Cosh(var a) => a.Derivative(v) * Sinh(a),
				Tanh(var a) => a.Derivative(v) * (1 - (Tanh(a) ^ 2)),
				_ => throw new NotImplementedException(
					$"{nameof(Derivative)} is not implemented for expression of type {expr.GetType()}"
				)
			};

		public Expr Derivative(Variable variable)
			=> expr.Derivative(variable.Name);
	}
}
