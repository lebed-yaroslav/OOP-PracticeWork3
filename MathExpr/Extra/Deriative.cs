using MathExpr.Function;
using MathExpr.Operator;
using static MathExpr.Function.BasicFunctions;

namespace MathExpr.Extra;


public static class DeriativeExt
{
	extension(IExpr expr)
	{
		public IExpr Deriative(string v)
			=> expr switch {
				Variable(var x) => new Constant((x == v)  ? 1 : 0),
				Constant => Constant.Zero,
				Negate(var a) => -a.Deriative(v),
				Add(var a, var b) => a.Deriative(v) + b.Deriative(v),
				Subtract(var a, var b) => a.Deriative(v) - b.Deriative(v),
				Multiply(var a, var b) => a.Deriative(v) * b + a * b.Deriative(v),
				Divide(var a, var b) => (a.Deriative(v) * b - a * b.Deriative(v)) / (b ^ 2),
				Power(var a, var b) => (a ^ b) * (b.Deriative(v) * Log(a) + b / a * a.Deriative(v)),
				Sqrt sqrt => 0.5 * sqrt.Argument.Deriative(v) / sqrt,
				Exp exp => exp.Argument.Deriative(v) * exp,
				Log(var a) => a.Deriative(v) / a,
				Sinh(var a) => a.Deriative(v) * Cosh(a),
				Cosh(var a) => a.Deriative(v) * Sinh(a),
				Tanh(var a) => a.Deriative(v) * (1 - (Tanh(a) ^ 2)),
				_ => throw new NotImplementedException(
					$"{nameof(Deriative)} is not implemented for expression of type {expr.GetType()}"
				)
			};

		public IExpr Deriative(Variable variable)
			=> expr.Deriative(variable.Name);
	}
}
