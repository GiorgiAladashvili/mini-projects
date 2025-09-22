using NUnit.Framework;
using Calculator;

namespace Calculator.Tests
{
    [TestFixture]
    public class CalculatorEngineTests
    {
        private CalculatorEngine _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new CalculatorEngine();
        }

        [Test]
        public void Clear_ShouldResetExpressionToZero()
        {
            _calculator.AddNumber("5");
            _calculator.Clear();
            Assert.That(_calculator.Expression, Is.EqualTo("0"));
        }

        [Test]
        public void AddNumber_ShouldAppendNumberToExpression()
        {
            _calculator.AddNumber("5");
            _calculator.AddNumber("3");
            Assert.That(_calculator.Expression, Is.EqualTo("53"));
        }

        [Test]
        public void AddOperator_ShouldAddOperatorBetweenNumbers()
        {
            _calculator.AddNumber("5");
            _calculator.AddOperator("+");
            _calculator.AddNumber("3");
            Assert.That(_calculator.Expression, Is.EqualTo("5+3"));
        }

        [Test]
        public void ToggleSign_ShouldChangeSignOfLastNumber()
        {
            _calculator.AddNumber("5");
            _calculator.ToggleSign();
            Assert.That(_calculator.Expression, Is.EqualTo("-5"));
        }

        [Test]
        public void Percent_ShouldConvertLastNumberToPercentage()
        {
            _calculator.AddNumber("50");
            _calculator.Percent();
            Assert.That(_calculator.Expression, Is.EqualTo("0.5"));
        }

        [Test]
        public void Calculate_ShouldReturnCorrectResult_Addition()
        {
            _calculator.AddNumber("5");
            _calculator.AddOperator("+");
            _calculator.AddNumber("3");
            string result = _calculator.Calculate();
            Assert.That(result, Is.EqualTo("8"));
        }

        [Test]
        public void Calculate_ShouldReturnCorrectResult_Multiplication()
        {
            _calculator.AddNumber("5");
            _calculator.AddOperator("×");
            _calculator.AddNumber("3");
            string result = _calculator.Calculate();
            Assert.That(result, Is.EqualTo("15"));
        }

        [Test]
        public void Calculate_ShouldReturnDivisionByZeroMessage()
        {
            _calculator.AddNumber("5");
            _calculator.AddOperator("÷");
            _calculator.AddNumber("0");
            string result = _calculator.Calculate();
            Assert.That(result, Is.EqualTo("Division by zero"));
        }

        [Test]
        public void Calculate_ShouldHandleInvalidExpression()
        {
            _calculator.AddOperator("+");
            _calculator.AddOperator("×");
            string result = _calculator.Calculate();
            Assert.That(result, Is.EqualTo("Invalid expression"));
        }

        [Test]
        public void Calculate_ShouldResetExpressionAfterCalculation()
        {
            _calculator.AddNumber("2");
            _calculator.AddOperator("+");
            _calculator.AddNumber("3");
            _calculator.Calculate();
            _calculator.AddNumber("4");
            Assert.That(_calculator.Expression, Is.EqualTo("4"));
        }

        [Test]
        public void ToggleSign_WithOperatorBeforeNumber_ShouldOnlyAffectNumber()
        {
            _calculator.AddNumber("5");
            _calculator.AddOperator("+");
            _calculator.AddNumber("3");
            _calculator.ToggleSign();
            Assert.That(_calculator.Expression, Is.EqualTo("5+-3"));
        }

        [Test]
        public void Percent_WithOperatorBeforeNumber_ShouldOnlyAffectLastNumber()
        {
            _calculator.AddNumber("200");
            _calculator.AddOperator("×");
            _calculator.AddNumber("50");
            _calculator.Percent();
            Assert.That(_calculator.Expression, Is.EqualTo("200×0.5"));
        }

        [Test]
        public void ToggleSign_AfterCalculation_ShouldStartNewExpression()
        {
            _calculator.AddNumber("2");
            _calculator.AddOperator("+");
            _calculator.AddNumber("3");
            _calculator.Calculate();
            _calculator.ToggleSign();
            Assert.That(_calculator.Expression, Is.EqualTo("-5"));
        }

        [Test]
        public void Percent_AfterCalculation_ShouldStartNewExpression()
        {
            _calculator.AddNumber("100");
            _calculator.Calculate();
            _calculator.Percent();
            Assert.That(_calculator.Expression, Is.EqualTo("1"));
        }

        [Test]
        public void MultipleOperations_WithToggleSignAndPercent()
        {
            _calculator.AddNumber("200");
            _calculator.AddOperator("-");
            _calculator.AddNumber("50");
            _calculator.Percent();
            _calculator.ToggleSign();
            _calculator.AddOperator("+");
            _calculator.AddNumber("10");
            string result = _calculator.Calculate();
            Assert.That(result, Is.EqualTo("210.5"));
        }
    }
}
