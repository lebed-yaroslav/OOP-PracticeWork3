using MathExpr.Functions;
using MathExpr.Operator;

using static MathExpr.Functions.BasicFunctions;

namespace MathExpr.Extra;

public static class SimplifyExt
{
	extension(Expr expr)
	{
		public Expr Simplify()
		{
			if (expr.TryComputeConstant(out var c) && double.IsFinite(c))
				return c;
			return expr switch {
				Negate neg => Simplify(neg),
				Add add => Simplify(add),
				Subtract sub => Simplify(sub),
				Multiply mul => Simplify(mul),
				Divide div => Simplify(div),
				Power pow => Simplify(pow),
				Sqrt sqrt => Simplify(sqrt),
				Exp exp => Simplify(exp),
				Log log => Simplify(log),
				Sinh sinh => Simplify(sinh),
				Cosh cosh => Simplify(cosh),
				Tanh tanh => Simplify(tanh),
				_ => expr
			};
		}
	}

	private static Expr Simplify(Negate neg)
	{
		var operand = Simplify(neg.Operand);
		return operand switch {
			Constant(var val) => new Constant(-val),
			Negate(var op) => op,
			_ => -operand
		};
	}

	private static Expr Simplify(Add add)
	{
		var left = Simplify(add.Left);
		var right = Simplify(add.Right);

		// Constants:
		if (left is Constant(var lc) && lc == 0)
			return right;
		if (right is Constant(var rc) && rc == 0)
			return left;
		if (left is Constant(var lConst) && right is Constant(var rConst))
			return new Constant(lConst + rConst);
		if (left.Equals(right))
			return 2 * left;

		// Distribution:
		// x / y + z / y = (x + z) / y
		// FIXME: Datatype LinearCombunation and non binary Multiply(...)
		if (
			left is Divide(var lNum, var lDen) &&
			right is Divide(var rNum, var rDen) &&
			lDen.Equals(rDen)
		) return Simplify((lNum + rNum) / lDen);

		return left + right;
	}

	private static Expr Simplify(Subtract sub)
	{
		var left = Simplify(sub.Left);
		var right = Simplify(sub.Right);

		// Constants:
		if (left is Constant(var lc) && lc == 0)
			return -right;
		if (right is Constant(var rc) && rc == 0)
			return left;
		if (
			left is Constant(var lConst) &&
			right is Constant(var rConst)
		)
			return new Constant(lConst - rConst);
		if (left.Equals(right))
			return Constant.Zero;
		return left - right;
	}


	private static Expr Simplify(Multiply mul)
	{
		var left = Simplify(mul.Left);
		var right = Simplify(mul.Right);

		// Constants:
		if (left is Constant(var lc))
		{
			if (lc == 0) return Constant.Zero;
			if (lc == 1) return right;
		}
		if (right is Constant(var rc))
		{
			if (rc == 0) return Constant.Zero;
			if (rc == 1) return left;
		}
		if (
			left is Constant(var lConst) &&
			right is Constant(var rConst)
		)
			return new Constant(lConst * rConst);

		if (left.Equals(right))
			return left ^ 2;

		// x * (y + z) = x * y + x * z
		if (left is Add(var a1, var b1))
			return Simplify((right * a1) + (right * b1));
		if (right is Add(var a2, var b2))
			return Simplify((left * a2) + (left * b2));
		// x * (y - z) = x * y - x * z
		if (left is Subtract(var a3, var b3))
			return Simplify((right * a3) - (right * b3));
		if (right is Subtract(var a4, var b4))
			return Simplify((left * a4) - (left * b4));
		// FIXME: Datatype LinearCombunation and non binary Multiply(...)
		// x * (y / z) = (x * y) / z
		if (left is Divide(var lNum, var lDen))
			return Simplify((lNum * right) / lDen);
		if (right is Divide(var rNum, var rDen))
			return Simplify((rNum * left) / rDen);
		return left * right;
	}

	private static Expr Simplify(Divide div)
	{
		var left = Simplify(div.Left);
		var right = Simplify(div.Right);

		// Constants:
		if (left is Constant(var lc) && lc == 0)
			return Constant.Zero;
		if (right is Constant(var rc) && rc == 1)
			return left;
		if (
			left is Constant(var lConst) &&
			right is Constant(var rConst) &&
			rConst != 0
		)
			return new Constant(lConst / rConst);
		if (left.Equals(right))
			return Constant.One;

		// FIXME: Datatype LinearCombunation and non binary Multiply(...)
		// (x / (y / z) = (x * z) / y
		if (right is Divide(var rNum, var rDen))
			return Simplify((left * rDen) / rNum);

		return left / right;
	}

	private static Expr Simplify(Power pow)
	{
		var left = Simplify(pow.Left);
		var right = Simplify(pow.Right);

		// Constants:
		if (right is Constant(var rc))
		{
			if (rc == 0) return Constant.One;
			if (rc == 1) return left;
		}
		if (left is Constant(var lc) && lc == 1)
			return Constant.One;
		if (
			left is Constant(var lConst) &&
			right is Constant(var rConst)
		)
			return new Constant(Math.Pow(lConst, rConst));

		return left ^ right;
	}

	private static Expr Simplify(Sqrt sqrt)
	{
		var arg = Simplify(sqrt.Argument);
		return arg switch {
			Constant(var c) => new Constant(sqrt.ComputeFor(c)),
			_ => Sqrt(arg)
		};
	}

	private static Expr Simplify(Exp exp)
	{
		var arg = Simplify(exp.Argument);
		return arg switch {
			Constant(var c) => new Constant(exp.ComputeFor(c)),
			Log(var a) => a,
			Add(var a, var b) => Simplify(Exp(a) * Exp(b)),
			Subtract(var a, var b) => Simplify(Exp(a) * Exp(-b)),
			_ => Exp(arg)
		};
	}

	private static Expr Simplify(Log log)
	{
		var arg = Simplify(log.Argument);
		return arg switch {
			Constant(var c) => new Constant(log.ComputeFor(c)),
			Exp(var a) => a,
			Multiply(var a, var b) => Simplify(Log(a) + Log(b)),
			Divide(var a, var b) => Simplify(Log(a) - Log(b)),
			_ => Log(arg)
		};
	}

	private static Expr Simplify(Sinh sinh)
	{
		var arg = Simplify(sinh.Argument);
		return arg switch {
			Constant(var c) => new Constant(sinh.ComputeFor(c)),
			_ => Sinh(arg)
		};
	}

	private static Expr Simplify(Cosh cosh)
	{
		var arg = Simplify(cosh.Argument);
		return arg switch {
			Constant(var c) => new Constant(cosh.ComputeFor(c)),
			_ => Cosh(arg)
		};
	}

	private static Expr Simplify(Tanh tanh)
	{
		var arg = Simplify(tanh.Argument);
		return arg switch {
			Constant(var c) => new Constant(tanh.ComputeFor(c)),
			_ => Tanh(arg)
		};
	}
}
