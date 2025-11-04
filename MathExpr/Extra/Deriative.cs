using MathExpr.Function;
using MathExpr.Operator;
using static MathExpr.Function.BasicFunctions;

namespace MathExpr.Extra;


public static class DeriativeExt
{
	extension(IExpr expr)
	{
		public IExpr Deriative(string variable)
			=> expr.Accept(new DeriativeVisitor(variable));

		public IExpr Deriative(Variable variable)
			=> expr.Deriative(variable.Name);
	}
}


public sealed record DeriativeVisitor(string Var) : IExprVisitor<IExpr>
{
	private IExpr DDVar(IExpr expr)
		=> expr.Accept(this);

	public IExpr Visit(Variable variable)
		=> new Constant((variable.Name == Var) ? 1 : 0);

	public IExpr Visit(Constant constant)
		=> new Constant(0);


	public IExpr Visit(Negate negate)
		=> -DDVar(negate.Operand);

	public IExpr Visit(Add add)
		=> DDVar(add.Left) + DDVar(add.Right);

	public IExpr Visit(Subtract sub)
		=> DDVar(sub.Left) - DDVar(sub.Right);

	public IExpr Visit(Multiply mul)
		=> DDVar(mul.Left) * mul.Right + mul.Left * DDVar(mul.Right);

	public IExpr Visit(Divide div)
		=> (DDVar(div.Left) * div.Right - div.Left * DDVar(div.Right)) / (div.Right ^ 2);

	public IExpr Visit(Power pow)
		=> pow * (DDVar(pow.Right) * Log(pow.Left) + pow.Right / pow.Left * DDVar(pow.Left));


	public IExpr Visit(Sqrt sqrt)
		=> DDVar(sqrt.Argument) / (2 * (IExpr)sqrt);
	public IExpr Visit(Exp exp)
		=> DDVar(exp.Argument) * exp;
	public IExpr Visit(Log log)
		=> DDVar(log.Argument) / log.Argument;


	public IExpr Visit(Sinh sinh)
		=> DDVar(sinh.Argument) * Cosh(sinh.Argument);

	public IExpr Visit(Cosh cosh)
		=> DDVar(cosh.Argument) * Sinh(cosh.Argument);

	public IExpr Visit(Tanh tanh)
		=> DDVar(tanh.Argument) * (1 - ((IExpr)tanh ^ 2));
}
