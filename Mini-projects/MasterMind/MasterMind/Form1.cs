using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MasterMind
{
    public partial class Form1 : Form
    {
        private Random rd = new Random();
        private Color[] mainColor = { Color.FromArgb(80, 80, 80), Color.Red, Color.LightGreen, Color.Orange, Color.White,
            Color.DodgerBlue, Color.Yellow, Color.FromArgb(90, 162, 214) };
        private ColorBall SelectedBall;
        private ColorBall[] mainball = new ColorBall[8];
        private ColorBall[] keyball = new ColorBall[5];
        private ColorRowBall[] testrow = new ColorRowBall[10];
        private Panel panel1, panel2;
        private Label status, author;
        private ControlRow[] cr = new ControlRow[10];
        private Button bNew, bTest, bEmpty;
        public byte WhiteScore, BlackScore;
        private int CurrentRow;


        public Form1()
        {
            InitializeComponent();

            // Create mainball
            for (int i = 0; i < 8; i++)
            {
                mainball[i] = new ColorBall(); mainball[i].Index = i;
                mainball[i].IsMainBall = true;
                mainball[i].ColorNormal = mainColor[i];
                if (i < 4) mainball[i].Location = new Point(55 * i + 5, 5);
                else mainball[i].Location = new Point(55 * (i - 4) + 5, 60);
                mainball[i].MouseEnter += new EventHandler(colball_MouseEnter);
                mainball[i].MouseLeave += new EventHandler(colball_MouseLeave);
                mainball[i].MouseDown += new MouseEventHandler(mainball_MouseDown);
            }

                // Create panel 1
                panel1 = new Panel();
                panel1.BackColor = Color.PeachPuff;
                panel1.SetBounds(10, 680, 225, 115);
                panel1.Controls.AddRange(mainball);

            // create keyball
            for (int i = 0; i < 5; i++)
            {
                keyball[i] = new ColorBall();
                keyball[i].Location = new Point(55 * i + 5, 5);
                keyball[i].IsEmpty = false;
                keyball[i].Index = i;
            }

            // create label status
            status = new Label();
            status.Font = new Font("Microsoft Sans Serif", 12f);
            status.Text = "Generate a new key";
            status.AutoSize = false;
            status.SetBounds(15, 20, 270, 50);

            // create label author
            author = new Label();
            author.Font = new Font("Microsoft Sans Serif", 10.0f);
            author.ForeColor = Color.Navy;
            author.Text = "by Selariu Serban 2023";
            author.AutoSize = false;
            author.SetBounds(280, 770, 180, 25);

            // create panel 2
            panel2 = new Panel();
            panel2.BackColor = Color.Salmon;
            panel2.SetBounds(10, 10, 280, 60);
            panel2.Controls.AddRange(keyball);
            panel2.Visible = false;

            // create testrow
            for (int k = 0; k < 10; k++)
            {
                testrow[k] = new ColorRowBall();
                testrow[k].Draw(10, 610 - 60 * k, Color.Peru , this);
            }

            // create button bNew
            bNew = new Button();
            bNew.Text = "New Key";
            bNew.Font = new Font("Microsoft Sans Serif", 14.25f);
            bNew.SetBounds(305, 20, 120, 35);
            bNew.Click += new EventHandler(bNew_Click);

            // create button bTest
            bTest = new Button();
            bTest.Text = "Next Row";
            bTest.Enabled = false;
            bTest.Font = new Font("Microsoft Sans Serif", 14.25f);
            bTest.SetBounds(305, 680, 120, 35);
            bTest.Click += new EventHandler(bTest_Click);

            // create button bEmpty
            bEmpty = new Button();
            bEmpty.Text = "Empty Row";
            bEmpty.Enabled = false;
            bEmpty.Font = new Font("Microsoft Sans Serif", 14.25f);
            bEmpty.SetBounds(305, 720, 120, 35);
            bEmpty.Click += new EventHandler(bEmpty_Click);

            // create ControlRow
            for (int i = 0; i < 10; i++)
            {
                cr[i] = new ControlRow();
                cr[i].Draw(300, 625 - 60 * i, Color.LightGreen , this);
            }

            this.Controls.Add(panel1);
            this.Controls.Add(panel2);
            this.Controls.Add(bNew);
            this.Controls.Add(bTest);
            this.Controls.Add(bEmpty);
            this.Controls.Add(status);
            this.Controls.Add(author);
            SetBounds(300, 50, 480, 840);
        }

        private void colball_MouseEnter(object sender, EventArgs e)
        {
            SelectedBall = (ColorBall)sender;
            SelectedBall.IsSelected = true;
        }

        private void colball_MouseLeave(object sender, EventArgs e)
        {
            SelectedBall.IsSelected = false;
        }

        private void mainball_MouseDown(object sender, MouseEventArgs e)
        {
            ColorBall cb = (ColorBall)sender;
            DataObject data = new DataObject(DataFormats.Serializable, cb);
            DoDragDrop(data, DragDropEffects.Copy);
        }

        private void bNew_Click(object sender, EventArgs e)
        {
            List<int> Number = new List<int>();
            for (int i = 0; i < 8; i++) Number.Add(i);
            int index;
            HideKey();
            for (int i = 0; i < 5; i++)
            {
                index = rd.Next(0, Number.Count);
                keyball[i].ColorNormal = mainColor[Number[index]];
                Number.RemoveAt(index);
            }
            for (int k = 0; k < 10; k++)
            {
                testrow[k].Hide();
                testrow[k].Deactivated();
                cr[k].Hide();
            }
            testrow[0].Activated(); CurrentRow = 0;
            status.Text = "Guess the combination key";
            bTest.Enabled = true;
            bEmpty.Enabled = true;
        }

        private void bTest_Click(object sender, EventArgs e)
        {
            WhiteScore = 0; BlackScore = 0;
            if (testrow[CurrentRow].IsFull)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (testrow[CurrentRow].Cell[i].ColorNormal == keyball[j].ColorNormal)
                        {
                            if (i == j) BlackScore++;
                            else WhiteScore++;
                        }
                    }
                }
                cr[CurrentRow].Show(WhiteScore, BlackScore);
                status.Text = $"Guess {WhiteScore + BlackScore} colors, only {BlackScore} are in \nthe right position";
                testrow[CurrentRow].Deactivated();
                if ((BlackScore == 5) || (CurrentRow == 9))
                {
                    ShowKey();
                    bTest.Enabled = false;
                    bEmpty.Enabled = false;
                }
                else
                {
                    CurrentRow++;
                    testrow[CurrentRow].Activated();
                }
            }
            else status.Text = "The row is not complete";
        }

        private void bEmpty_Click(object sender, EventArgs e)
        {
            testrow[CurrentRow].Hide();
        }

        private void ShowKey() => panel2.Visible = true;

        private void HideKey() => panel2.Visible = false;
       

    }
}
