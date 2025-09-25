using System;
using System.Drawing;
using System.Windows.Forms;

namespace MasterMind
{
    public  class ControlRow
    {
        public ColorBall[] Cell = new ColorBall[5];
        private Panel panel;

        public ControlRow()
        {
            for (int i = 0; i < 5; i++)
            {
                Cell[i] = new ColorBall();
                Cell[i].IsEmpty = true;
                Cell[i].SetBounds(30 * i + 5, 5, 25, 25);
            }
            panel = new Panel();
            panel.Controls.AddRange(Cell);
        }

        public void Draw(int x, int y, Color colpanel, Form frm)
        {
            panel.SetBounds(x, y, 155, 35);
            panel.BackColor = colpanel;
            frm.Controls.Add(panel);
        }

        public void Show(int white, int black)
        {
            for (int i = 0; i < white + black; i++)
            {
                if (i < white) Cell[i].ColorNormal = Color.White;
                else Cell[i].ColorNormal = Color.FromArgb(80, 80, 80);
                Cell[i].IsEmpty = false;
            }
        }

        public void Hide()
        {
            for (int i = 0; i < 5; i++) Cell[i].IsEmpty = true;
        }

    }
}
