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
			Constant c => new Constant(-c.Value),
			Negate neg2 => neg2.Operand,
			_ => -operand
		};
	}

	public IExpr Visit(Add add)
	{
		var left = Simplify(add.Left);
		var right = Simplify(add.Right);

		// Constants:
		if (left is Constant lc && lc.Value == 0)
			return right;
		if (right is Constant rc && rc.Value == 0)
			return left;
		if (left is Constant lConst && right is Constant rConst)
			return new Constant(lConst.Value + rConst.Value);
		if (left.Equals(right))
			return 2 * left;

		// Distribution:
		// x / y + z / y = (x + z) / y
		// FIXME: Datatype LinearCombunation and non binary Multiply(...)
		if (left is Divide lDiv && right is Divide rDiv)
			if (lDiv.Right.Equals(rDiv.Right))
				return Simplify((lDiv.Left + rDiv.Left) / lDiv.Right);
		return left + right;
	}
	
	public IExpr Visit(Subtract sub)
	{
		var left = Simplify(sub.Left);
		var right = Simplify(sub.Right);
		
		// Constants:
		if (left is Constant lc && lc.Value == 0)
			return -right;
		if (right is Constant rc && rc.Value == 0)
			return left;
		if (left is Constant lConst && right is Constant rConst)
			return new Constant(lConst.Value - rConst.Value);
		if (left.Equals(right))
			return new Constant(0);
		return left - right;
	}

	public IExpr Visit(Multiply mul)
	{
		var left = Simplify(mul.Left);
		var right = Simplify(mul.Right);

		// Constants:
		if (left is Constant lc)
		{
			if (lc.Value == 0) return new Constant(0);
			if (lc.Value == 1) return right;
		}
		if (right is Constant rc)
		{
			if (rc.Value == 0) return new Constant(0);
			if (rc.Value == 1) return left;
		}
		if (left is Constant lConst && right is Constant rConst)
			return new Constant(lConst.Value * rConst.Value);
		if (left.Equals(right)) return left ^ 2;

		// x * (y + z) = x * y + x * z
		if (left is Add lAdd)
			return Simplify((right * lAdd.Left) + (right * lAdd.Right));
		if (right is Add rAdd)
			return Simplify((left * rAdd.Left) + (left * rAdd.Right));
		// x * (y - z) = x * y - x * z
		if (left is Subtract lSub)
			return Simplify((right * lSub.Left) - (right * lSub.Right));
		if (right is Subtract rSub)
			return Simplify((left * rSub.Left) - (left * rSub.Right));
		// FIXME: Datatype LinearCombunation and non binary Multiply(...)
		// x * (y / z) = (x * y) / z
		if (left is Divide lDiv)
			return Simplify((lDiv.Left * right) / lDiv.Right);
		if (right is Add rDiv)
			return Simplify((rDiv.Left * left) / rDiv.Right);
		return left * right;
	}

	public IExpr Visit(Divide div)
	{
		var left = Simplify(div.Left);
		var right = Simplify(div.Right);

		// Constants:
		if (left is Constant lc && lc.Value == 0)
			return new Constant(0);
		if (right is Constant rc && rc.Value == 1)
			return left;
		if (left is Constant lConst && right is Constant rConst && rConst.Value != 0)
			return new Constant(lConst.Value / rConst.Value);
		if (left.Equals(right))
			return new Constant(1);

		if (right is Divide rDiv)
			return Simplify((left * rDiv.Right) / rDiv.Left);

		return left / right;
	}

	public IExpr Visit(Power pow)
	{
		var left = Simplify(pow.Left);
		var right = Simplify(pow.Right);

		// Constants:
		if (right is Constant rc)
		{
			if (rc.Value == 0) return new Constant(1);
			if (rc.Value == 1) return left;
		}
		if (left is Constant lc && lc.Value == 1) return new Constant(1);
		if (left is Constant lConst && right is Constant rConst)
			return new Constant(Math.Pow(lConst.Value, rConst.Value));

		return left ^ right;
	}

	public IExpr Visit(Sqrt sqrt)
	{
		var arg = Simplify(sqrt.Argument);
		return arg switch {
			Constant c => new Constant(sqrt.ComputeFor(c.Value)),
			_ => new Sqrt(arg)
		};		
	}

	public IExpr Visit(Exp exp)
	{
		var arg = Simplify(exp.Argument);
		return arg switch {
			Constant c => new Constant(exp.ComputeFor(c.Value)),
			Log log => log.Argument,
			Add add => Simplify(Exp(add.Left) * Exp(add.Right)),
			Subtract sub => Simplify(Exp(sub.Left) * Exp(-sub.Right)),
			_ => new Exp(arg)
		};
	}

	public IExpr Visit(Log log)
	{
		var arg = Simplify(log.Argument);
		return arg switch {
			Constant c => new Constant(log.ComputeFor(c.Value)),
			Exp exp => exp.Argument,
			Multiply mul => Simplify(Log(mul.Left) + Log(mul.Right)),
			Divide div => Simplify(Log(div.Left) - Log(div.Right)),
			_ => Log(arg)
		};
	}

	public IExpr Visit(Sinh sinh)
	{
		var arg = Simplify(sinh.Argument);
		return arg switch {
			Constant c => new Constant(sinh.ComputeFor(c.Value)),
			_ => new Sinh(arg)
		};
	}

	public IExpr Visit(Cosh cosh)
	{
		var arg = Simplify(cosh.Argument);
		return arg switch {
			Constant c => new Constant(cosh.ComputeFor(c.Value)),
			_ => new Cosh(arg)
		};
	}

	public IExpr Visit(Tanh tanh)
	{
		var arg = Simplify(tanh.Argument);
		return arg switch {
			Constant c => new Constant(tanh.ComputeFor(c.Value)),
			_ => new Tanh(arg)
		};
	}
}
