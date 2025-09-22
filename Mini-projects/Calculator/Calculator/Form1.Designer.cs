namespace Calculator
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox textBoxDisplay;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBoxDisplay = new System.Windows.Forms.TextBox();
            SuspendLayout();

            textBoxDisplay.BackColor = System.Drawing.Color.Black;
            textBoxDisplay.Dock = System.Windows.Forms.DockStyle.Top;
            textBoxDisplay.Font = new System.Drawing.Font("Segoe UI", 28F);
            textBoxDisplay.ForeColor = System.Drawing.Color.White;
            textBoxDisplay.ReadOnly = true;
            textBoxDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxDisplay.Size = new System.Drawing.Size(320, 70);
            textBoxDisplay.TabStop = false;
            textBoxDisplay.Text = "0";
            textBoxDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            ClientSize = new System.Drawing.Size(320, 480);
            Controls.Add(textBoxDisplay);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            Text = "iPhone Calculator";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
