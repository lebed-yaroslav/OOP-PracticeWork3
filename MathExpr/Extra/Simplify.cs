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
		if (left.Equals(right))
			return 2 * left;

        // Unite under common factor:
        // x / y + z / y = (x + z) / y
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
		if (left.Equals(right))
			return 0;

		// Unite under common factor:
		// x / y + z / y = (x + z) / y
		if (
			left is Divide(var lNum, var lDen) &&
			right is Divide(var rNum, var rDen) &&
			lDen.Equals(rDen)
		) return Simplify((lNum - rNum) / lDen);

		return left - right;
	}


	private static Expr Simplify(Multiply mul)
	{
		var left = Simplify(mul.Left);
		var right = Simplify(mul.Right);

		// Constants:
		if (left is Constant(var lc))
		{
			if (lc == 0) return 0;
			if (lc == 1) return right;
		}
		if (right is Constant(var rc))
		{
			if (rc == 0) return 0;
			if (rc == 1) return left;
		}
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

		// FIXME: Datatype LinearCombination and non binary Multiply(...)
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
			return 0;
		if (right is Constant(var rc) && rc == 1)
			return left;
		if (left.Equals(right))
			return 1;

		// FIXME: Datatype LinearCombination and non binary Multiply(...)
		// (x / (y / z) = (x * z) / y
		if (right is Divide(var rNum, var rDen))
			return Simplify((left * rDen) / rNum);

		// (x^a / x^b) = x^(a - b)
		if (left is Power(var lBase, var lPow))
		{
			if (right is Variable rVar && rVar.Equals(lBase))
				return Simplify(lBase ^ (lPow - 1));
			if (right is Power(var rBase, var rPow) && rBase.Equals(lBase))
				return Simplify(lBase ^ (lPow - rPow));
		}
		else if (left is Variable lVar && right is Power(var rBase, var rPow) && rBase.Equals(lVar))
			return Simplify(lVar ^ (1 - rPow));

		return left / right;
	}

	private static Expr Simplify(Power pow)
	{
		var left = Simplify(pow.Left);
		var right = Simplify(pow.Right);

		// Constants:
		if (right is Constant(var rc))
		{
			if (rc == 0) return 1;
			if (rc == 1) return left;
			if (rc == -1) return 1 / left;
		}
		if (left is Constant(var lc))
		{
			if (lc == 0) return 0;
			if (lc == 1) return 1;
		}

		return left ^ right;
	}

	private static Expr Simplify(Sqrt sqrt)
	{
		var arg = Simplify(sqrt.Argument);
		if (arg is Power(var b, var p) && p is Constant(var c))
			return Simplify(b ^ (c / 2));
		return Sqrt(arg);
	}

	private static Expr Simplify(Exp exp)
	{
		var arg = Simplify(exp.Argument);
		return arg switch {
			Log(var a) => a,
			_ => Exp(arg)
		};
	}

	private static Expr Simplify(Log log)
	{
		var arg = Simplify(log.Argument);
		return arg switch {
			Exp(var a) => a,
			_ => Log(arg)
		};
	}

	private static Expr Simplify(Sinh sinh)
		=> Sinh(Simplify(sinh.Argument));

	private static Expr Simplify(Cosh cosh)
		=> Cosh(Simplify(cosh.Argument));

	private static Expr Simplify(Tanh tanh)
		=> Tanh(Simplify(tanh.Argument));
}
