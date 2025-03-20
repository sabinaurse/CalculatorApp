using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalculatorApp
{
    public class CalculatorModel
    {
        public double PerformOperation(double firstOperand, double secondOperand, string operation)
        {
            try
            {
                return operation switch
                {
                    "+" => firstOperand + secondOperand,
                    "-" => firstOperand - secondOperand,
                    "*" => firstOperand * secondOperand,
                    "/" => secondOperand != 0 ? firstOperand / secondOperand : throw new DivideByZeroException("Nu se poate împărți la zero!"),
                    "%" => secondOperand != 0 ? firstOperand % secondOperand : throw new DivideByZeroException("Modulo cu zero nu este permis!"),
                    _ => throw new InvalidOperationException("Operație invalidă")
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return double.NaN;
            }
        }


        public double PerformUnaryOperation(double operand, string operation)
        {
            try
            {
                return operation switch
                {
                    "√" => operand >= 0 ? Math.Sqrt(operand) : throw new ArgumentException("Nu se poate calcula rădăcina pătrată a unui număr negativ!"),
                    "x²" => operand is > -1E154 and < 1E154 ? Math.Pow(operand, 2) : throw new OverflowException("Rezultatul este prea mare!"),
                    "1/x" => operand != 0 ? 1 / operand : throw new DivideByZeroException("Împărțire la zero!"),
                    "+/-" => -operand,  // ✅ Schimbă corect semnul numărului curent
                    _ => throw new InvalidOperationException("Operație invalidă")
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return double.NaN;
            }
        }


    }

}
