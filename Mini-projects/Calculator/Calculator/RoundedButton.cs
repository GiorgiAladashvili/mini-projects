using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedButton : Button
{
    private Color _buttonColor = Color.DarkGray;

    public Color ButtonColor
    {
        get => _buttonColor;
        set
        {
            _buttonColor = value;
            FlatAppearance.MouseOverBackColor = _buttonColor;
            FlatAppearance.MouseDownBackColor = _buttonColor;
            Invalidate();
        }
    }

    public RoundedButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        FlatAppearance.MouseOverBackColor = _buttonColor;
        FlatAppearance.MouseDownBackColor = _buttonColor;
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 18F);
        Width = 70;
        Height = 70;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        using GraphicsPath path = new GraphicsPath();

        if (Text == "+")
        {
            int radius = 20;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
            path.AddArc(0, Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
        }
        else
        {
            int diameter = Math.Min(Width, Height);
            path.AddEllipse(0, 0, diameter, diameter);
        }

        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using SolidBrush brush = new SolidBrush(ButtonColor);
        pevent.Graphics.FillPath(brush, path);

        Rectangle textRect = Text == "+"
            ? new Rectangle(0, (Height - TextRenderer.MeasureText(Text, Font).Height) / 2, Width, TextRenderer.MeasureText(Text, Font).Height)
            : new Rectangle(0, 0, Width, Height);

        TextRenderer.DrawText(
            pevent.Graphics,
            Text,
            Font,
            textRect,
            ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
        );
    }
}
