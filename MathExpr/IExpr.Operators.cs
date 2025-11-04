using MathExpr.Operator;

namespace MathExpr;

public partial interface IExpr
{
	public static IExpr operator -(IExpr operand)
		=> new Negate(operand);

	public static IExpr operator +(IExpr left, IExpr right)
		=> new Add(left, right);
	public static IExpr operator -(IExpr left, IExpr right)
		=> new Subtract(left, right);
	public static IExpr operator *(IExpr left, IExpr right)
		=> new Multiply(left, right);
	public static IExpr operator /(IExpr left, IExpr right)
		=> new Divide(left, right);

	public static IExpr operator ^(IExpr left, IExpr right)
		=> new Power(left, right);


	public static IExpr operator +(IExpr left, double right)
		=> left + new Constant(right);
	public static IExpr operator +(double left, IExpr right)
		=> new Constant(left) + right;
	public static IExpr operator -(IExpr left, double right)
		=> left - new Constant(right);
	public static IExpr operator -(double left, IExpr right)
		=> new Constant(left) - right;
	public static IExpr operator *(IExpr left, double right)
		=> left * new Constant(right);
	public static IExpr operator *(double left, IExpr right)
		=> new Constant(left) * right;
	public static IExpr operator /(IExpr left, double right)
		=> left / new Constant(right);
	public static IExpr operator /(double left, IExpr right)
		=> new Constant(left) / right;
	public static IExpr operator ^(IExpr left, double right)
		=> left ^ new Constant(right);
	public static IExpr operator ^(double left, IExpr right)
		=> new Constant(left) ^ right;
}
