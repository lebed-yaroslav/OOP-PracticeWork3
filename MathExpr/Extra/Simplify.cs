using MathExpr.Function;
using MathExpr.Operator;
using static MathExpr.Function.BasicFunctions;

namespace MathExpr.Extra;


public static class SimplifyExt
{
	extension(IExpr expr)
	{
		public IExpr Simplify() => expr.Accept(BasicSimplifyVisitor.Instance);
	}
}


public sealed class BasicSimplifyVisitor : IExprVisitor<IExpr>
{
	public static BasicSimplifyVisitor Instance = new();
	private BasicSimplifyVisitor() {}

	private IExpr Simplify(IExpr expr)
		=> expr.Accept(this);

	public IExpr Visit(Variable var)
		=> var;

	public IExpr Visit(Constant c)
		=> c;

	public IExpr Visit(Negate neg)
	{
		var operand = Simplify(neg.Operand);
		return operand switch {
			Constant(var val) => new Constant(-val),
			Negate(var op) => op,
			_ => -operand
		};
	}

	public IExpr Visit(Add add)
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
	
	public IExpr Visit(Subtract sub)
	{
		var left = Simplify(sub.Left);
		var right = Simplify(sub.Right);

		// Constants:
		if (left is Constant(var lc) && lc == 0)
			return -right;
		if (right is Constant(var rc) && rc == 0)
			return left;
		if(
			left is Constant(var lConst) &&
			right is Constant(var rConst)
		)
			return new Constant(lConst - rConst);
		if (left.Equals(right))
			return Constant.Zero;
		return left - right;
	}

	public IExpr Visit(Multiply mul)
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

	public IExpr Visit(Divide div)
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

	public IExpr Visit(Power pow)
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

	public IExpr Visit(Sqrt sqrt)
	{
		var arg = Simplify(sqrt.Argument);
		return arg switch {
			Constant(var c) => new Constant(sqrt.ComputeFor(c)),
			_ => Sqrt(arg)
		};		
	}

	public IExpr Visit(Exp exp)
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

	public IExpr Visit(Log log)
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

	public IExpr Visit(Sinh sinh)
	{
		var arg = Simplify(sinh.Argument);
		return arg switch {
			Constant(var c) => new Constant(sinh.ComputeFor(c)),
			_ => Sinh(arg)
		};
	}

	public IExpr Visit(Cosh cosh)
	{
		var arg = Simplify(cosh.Argument);
		return arg switch {
			Constant(var c) => new Constant(cosh.ComputeFor(c)),
			_ => Cosh(arg)
		};
	}

	public IExpr Visit(Tanh tanh)
	{
		var arg = Simplify(tanh.Argument);
		return arg switch {
			Constant(var c) => new Constant(tanh.ComputeFor(c)),
			_ => Tanh(arg)
		};
	}
}
