using MathExpr.Functions;
using MathExpr.Operator;
using static MathExpr.Functions.BasicFunctions;

namespace MathExpr.Extra;


public static class DerivativeExt
{
	extension(Expr expr)
	{
		public Expr Derivative(string v)
		{
			// Always true for Constant
			if (!expr.DependsOn(v))
				return Constant.Zero;
			return expr switch {
				Variable(var x) => Constant.One,
				Negate(var a) => -a.Derivative(v),
				Add(var a, var b) => a.Derivative(v) + b.Derivative(v),
				Subtract(var a, var b) => a.Derivative(v) - b.Derivative(v),
				Multiply(var a, var b) => a.Derivative(v) * b + a * b.Derivative(v),
				Divide(var a, var b) => (a.Derivative(v) * b - a * b.Derivative(v)) / (b ^ 2),
				Power(var a, var b) => (a ^ b) * (b.Derivative(v) * Log(a) + b / a * a.Derivative(v)),
				Function func => func.Argument.Derivative(v) * func.Derivative(),
				_ => throw new NotImplementedException(
					$"{nameof(Derivative)} is not implemented for expression of type {expr.GetType()}"
				)
			};
		}

		public Expr Derivative(Variable variable)
			=> expr.Derivative(variable.Name);
	}
}
