using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Delegate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string textValue = "";

        public delegate void fillTextBox(int x);

        public void FillMathNote(int ExameNote)
        {
            textValue += "Your Math Note is: " + ExameNote + Environment.NewLine;
        }

        public void FillChemistryNote(int ExamNote)
        {
            textValue += "Your Chemistry Note is: " + ExamNote + Environment.NewLine;
        }
        public void ShowInTextBox()
        {
            txtResult.Text = textValue;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            fillTextBox fillText = FillMathNote;
            fillText += FillChemistryNote;
            fillText(80);
            fillText-= FillMathNote;
            fillText(60);
            ShowInTextBox();


            // fillTextBox fillTextBox = new fillTextBox(FillMathNote);
            // fillTextBox.Invoke(80);
        }
    }
}
