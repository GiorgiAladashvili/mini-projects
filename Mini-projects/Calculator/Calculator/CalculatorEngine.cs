using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Calculator;

public class CalculatorEngine
{
	private string expression = "0";
	private bool justCalculated = false;

	public string Expression => expression;

	public void Clear()
	{
		expression = "0";
		justCalculated = false;
	}

	public void AddNumber(string number)
	{
		if (justCalculated) Clear();

		if (expression == "0" && number != ".")
		{
			expression = number;
			return;
		}

		if (number == ".")
		{
			int i = expression.Length - 1;
			while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.'))
			{
				if (expression[i] == '.')
				{
					return; 
				}
				i--;
			}
			expression += number;
		}
		else
		{
			expression += number;
		}
	}

	public void AddOperator(string op)
	{
		if (justCalculated) justCalculated = false;

		if ("+-×÷".Contains(expression[^1]))
			expression = expression[..^1] + op;
		else
			expression += op;
	}

	public void ToggleSign()
	{
		// Find the last number in the expression
		int i = expression.Length - 1;
		while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.'))
		{
			i--;
		}

		string lastNumberString = expression.Substring(i + 1);

		if (double.TryParse(lastNumberString, out double lastNumberValue))
		{
			// Toggle the sign
			double toggledValue = -lastNumberValue;

			// Rebuild the expression
			string newExpression = expression.Substring(0, i + 1) + toggledValue.ToString(CultureInfo.InvariantCulture);

			expression = newExpression;
		}
	}

	public void Percent()
	{
		// Find the last number and the operator before it
		int i = expression.Length - 1;
		while (i >= 0 && (char.IsDigit(expression[i]) || expression[i] == '.'))
		{
			i--;
		}

		string lastNumberString = expression.Substring(i + 1);

		if (double.TryParse(lastNumberString, out double lastNumberValue))
		{
			double percentageValue = lastNumberValue / 100.0;
			string percentageString = percentageValue.ToString(CultureInfo.InvariantCulture);

			// Replace the last number with its percentage value
			expression = expression.Substring(0, i + 1) + percentageString;
		}
	}

	public string Calculate()
	{
		try
		{
			string expr = expression.Replace("×", "*").Replace("÷", "/");

			// Check for division by zero
			if (Regex.IsMatch(expr, @"/[0]+(\.0+)?"))
			{
				Clear();
				return "Division by zero";
			}

			var table = new DataTable();
			double result = Convert.ToDouble(table.Compute(expr, string.Empty));

			expression = result.ToString(CultureInfo.InvariantCulture);
			justCalculated = true;
			return expression;
		}
		catch
		{
			Clear();
			return "Invalid expression";
		}
	}
}
