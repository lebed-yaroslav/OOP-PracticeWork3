using MathExpr.Functions;
using MathExpr.Operator;

namespace MathExpr;

public interface IExprVisitor<out T>
{
	T Visit(Variable var);
	T Visit(Constant c);

	// Operators:
	T Visit(Negate neg);
	T Visit(Add add);
	T Visit(Subtract sub);
	T Visit(Multiply mul);
	T Visit(Divide div);
	T Visit(Power pow);

	// Functions:
	T Visit(Sqrt sqrt);
	T Visit(Exp exp);
	T Visit(Log log);

	T Visit(Sinh sinh);
	T Visit(Cosh cosh);
	T Visit(Tanh tanh);
}
