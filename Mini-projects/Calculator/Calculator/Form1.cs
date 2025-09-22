using System;
using System.Drawing;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private readonly CalculatorEngine engine = new CalculatorEngine();

        private readonly string[,] buttonLayout =
        {
            { "C", "±", "%", "÷" },
            { "7", "8", "9", "×" },
            { "4", "5", "6", "-" },
            { "1", "2", "3", "+" },
            { "0", ".", "=", " " }
        };

        public Form1()
        {
            InitializeComponent();
            InitializeButtons();
            KeyPreview = true;
            KeyDown += Form1_KeyDown;
        }

        private void InitializeButtons()
        {
            int buttonWidth = 70;
            int buttonHeight = 70;
            int spacing = 5;
            int startX = 10;
            int startY = 70;

            for (int row = 0; row < buttonLayout.GetLength(0); row++)
            {
                for (int col = 0; col < buttonLayout.GetLength(1); col++)
                {
                    string text = buttonLayout[row, col];
                    if (string.IsNullOrWhiteSpace(text)) continue;

                    var btn = new RoundedButton
                    {
                        Text = text,
                        Width = buttonWidth,
                        Height = buttonHeight,
                        Left = startX + col * (buttonWidth + spacing),
                        Top = startY + row * (buttonHeight + spacing)
                    };

                    if ("0123456789.".Contains(text))
                        btn.ButtonColor = Color.FromArgb(50, 50, 50);
                    else if ("+-×÷=".Contains(text))
                        btn.ButtonColor = Color.FromArgb(255, 149, 0);
                    else
                        btn.ButtonColor = Color.FromArgb(142, 142, 147);

                    if (text == "+")
                        btn.Height = buttonHeight * 2 + spacing;

                    btn.Click += Button_Click;
                    Controls.Add(btn);
                }
            }

            BackColor = Color.Black;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (sender is not Button btn) return;
            string value = btn.Text;

            switch (value)
            {
                case "C": engine.Clear(); break;
                case "±": engine.ToggleSign(); break;
                case "%": engine.Percent(); break;
                case "=": textBoxDisplay.Text = engine.Calculate(); return;
                case "+": case "-": case "×": case "÷": engine.AddOperator(value); break;
                default: engine.AddNumber(value); break;
            }

            textBoxDisplay.Text = engine.Expression;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxDisplay.Text = engine.Calculate();
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.Back)
            {
                engine.Clear();
                textBoxDisplay.Text = "0";
                return;
            }

            string key = e.KeyCode switch
            {
                Keys.D0 or Keys.NumPad0 => "0",
                Keys.D1 or Keys.NumPad1 => "1",
                Keys.D2 or Keys.NumPad2 => "2",
                Keys.D3 or Keys.NumPad3 => "3",
                Keys.D4 or Keys.NumPad4 => "4",
                Keys.D5 or Keys.NumPad5 => "5",
                Keys.D6 or Keys.NumPad6 => "6",
                Keys.D7 or Keys.NumPad7 => "7",
                Keys.D8 or Keys.NumPad8 => "8",
                Keys.D9 or Keys.NumPad9 => "9",
                Keys.OemPeriod or Keys.Decimal => ".",
                Keys.Add => "+",
                Keys.Subtract => "-",
                Keys.Multiply => "×",
                Keys.Divide => "÷",
                _ => null
            };

            if (key != null)
            {
                if ("0123456789.".Contains(key))
                    engine.AddNumber(key);
                else
                    engine.AddOperator(key);

                textBoxDisplay.Text = engine.Expression;
            }
        }
    }
}
