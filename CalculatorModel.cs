using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp
{
    public class CalculatorModel
    {
        public double PerformOperation(double firstOperand, double secondOperand, string operation)
        {
            return operation switch
            {
                "+" => firstOperand + secondOperand,
                "-" => firstOperand - secondOperand,
                "*" => firstOperand * secondOperand,
                "/" => secondOperand != 0 ? firstOperand / secondOperand : throw new DivideByZeroException("Împărțire la zero!"),
                "%" => firstOperand % secondOperand,
                _ => throw new InvalidOperationException("Operație invalidă")
            };
        }

        public double PerformUnaryOperation(double operand, string operation)
        {
            return operation switch
            {
                "√" => operand >= 0 ? Math.Sqrt(operand) : throw new ArgumentException("Numărul trebuie să fie pozitiv!"),
                "x²" => Math.Pow(operand, 2),
                "1/x" => operand != 0 ? 1 / operand : throw new DivideByZeroException("Împărțire la zero!"),
                "+/-" => -operand,
                _ => throw new InvalidOperationException("Operație invalidă")
            };
        }
    }

}
