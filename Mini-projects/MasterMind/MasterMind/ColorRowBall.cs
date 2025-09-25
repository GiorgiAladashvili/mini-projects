using System;
using System.Drawing;
using System.Windows.Forms;

namespace MasterMind
{
    public class ColorRowBall
    {
        public ColorBall[] Cell = new ColorBall[5];
        private Panel panel = new Panel();
        private ColorBall selcell;

        public ColorRowBall ()
        {
            for (int i = 0; i < 5; i++)
            {
                Cell[i] = new ColorBall(); Cell[i].Index = i;
                Cell[i].Location = new Point(55 * i + 5, 5);
                Cell[i].BackColor = Color.Silver;
                Cell[i].IsEmpty = true; Cell[i].Enabled = false;
                Cell[i].MouseEnter += new EventHandler(cellball_MouseEnter);
                Cell[i].MouseLeave += new EventHandler(cellball_MouseLeave);
                Cell[i].MouseDown += new MouseEventHandler(cellball_MouseDown);
                Cell[i].DragDrop += new DragEventHandler(cellball_DragDrop);
                Cell[i].DragEnter += new DragEventHandler(cellball_DragEnter);
            }
            panel.Controls.AddRange(Cell);
        }

        public bool IsFull
        {
            get
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Cell[i].IsEmpty) return false;
                }
                return true;
            }
        }

        public void Draw(int x, int y,Color colpanel, Form frm)
        {
            panel.SetBounds(x, y, 280, 60);
            panel.BackColor = colpanel;
            frm.Controls.Add(panel);
        }

        public void Activated()
        {
            for (int i = 0; i < 5; i++)
            {
                Cell[i].BackColor = Color.DarkSeaGreen;
                Cell[i].Enabled = true;
            }
        }

        public void Deactivated()
        {
            for (int i = 0; i < 5; i++)
            {
                Cell[i].BackColor = Color.Silver;
                Cell[i].Enabled = false;
            }
        }

        public void Hide()
        {
            for (int i = 0; i < 5; i++) Cell[i].IsEmpty = true;
        }

        public void Show()
        {
            for (int i = 0; i < 5; i++) Cell[i].IsEmpty = false;
        }

        private void cellball_MouseEnter(object sender, EventArgs e)
        {
            selcell  = (ColorBall)sender;
            selcell.IsSelected = true;
        }

        private void cellball_MouseLeave(object sender, EventArgs e)
        {
            selcell.IsSelected = false;
        }

        private void cellball_MouseDown(object sender, MouseEventArgs e)
        {
            ColorBall cb = (ColorBall)sender;
            if (e.Button == MouseButtons.Left)
            {
                if (!cb.IsEmpty)
                {
                    DataObject data = new DataObject(DataFormats.Serializable, cb);
                    cb.DoDragDrop(data, DragDropEffects.Copy);
                }
            }
            else cb.IsEmpty = true;
        }

        private void cellball_DragDrop(object sender, DragEventArgs e)
        {
            ColorBall cb = (ColorBall)sender;
            ColorBall c1 = (ColorBall)e.Data.GetData(DataFormats.Serializable);
            Color temp = cb.ColorNormal;
            cb.ColorNormal = c1.ColorNormal;
            if (!c1.IsMainBall)
            {
                c1.ColorNormal = temp;
                c1.IsEmpty = cb.IsEmpty;
            }
            cb.IsEmpty = false;
        }

        private void cellball_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Serializable)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

    }
}
