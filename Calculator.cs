using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCalculator
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();
        }
        struct stHistory
        {
            public double PrevResult;
            public double PrevFirstNumber;
            public double PrevSecondNumber;
            public string PrevsOperation;
            public bool PrevisNewNumber;
            public string PrevTemp1;
            public string PrevTemp2;
            
            
        }
        stHistory PrevCalculation = new stHistory();
        Stack<stHistory> History = new Stack<stHistory>();

        double FirstNumber = 0;
        double SecondNumber = 0;
        string OpType = "";
        bool isNewNumber = false;
        string Temp1 = "";
        string Temp2 = "";
        double Result = 0;
        bool IsResultShown = false;
        private void btnNumber_Click(object sender, EventArgs e)
        {
            
            Button btnNum = (Button)sender;
            if (IsResultShown)
            {
                isNewNumber = false;
                FirstNumber = Result;
                txtMain.Text = FirstNumber.ToString();
                txtResult.Clear();
                IsResultShown = false;
                txtResult.Font = new Font("Microsoft Sans Serif", 25.0f, FontStyle.Bold);
                txtResult.ForeColor = Color.Gray;
            }
            if (isNewNumber)
            {
                //here
                PrevCalculation.PrevTemp2 = Temp2;
                Temp2 += btnNum.Tag.ToString();
                SecondNumber = Convert.ToDouble(Temp2);
                PerformCalculation();
            }
            else
            {
                //here
                PrevCalculation.PrevTemp1 = Temp1;

                Temp1 = txtMain.Text + btnNum.Text.ToString();
                FirstNumber = Convert.ToDouble(Temp1);
                isNewNumber = false;
            }
            
            txtMain.Text += btnNum.Tag.ToString();

        }
        private void btnOparator_Click(object sender, EventArgs e)
        {
            Button btnOp = (Button)sender;
            if(btnOp.Text.ToString() == "-" && txtMain.Text == "")
            {
                txtMain.Text += btnOp.Text;
                return;
            }
            if (txtMain.Text == "")
            {
                return;
            }
           
            
            if (OpType != "")
            {
                FirstNumber = Result;
            }

            if (IsResultShown)
            {

                txtMain.Text = string.Empty;
                FirstNumber = Result;
                txtMain.Text = FirstNumber.ToString();
                txtResult.Text = string.Empty;
                IsResultShown = false;
                txtResult.Font = new Font("Microsoft Sans Serif", 25.0f, FontStyle.Bold);
                txtResult.ForeColor = Color.Gray;
            }
            
            if (txtMain.Text[txtMain.Text.Length - 1] == '+' ||
    txtMain.Text[txtMain.Text.Length - 1] == '-' ||
    txtMain.Text[txtMain.Text.Length - 1] == 'x' ||
    txtMain.Text[txtMain.Text.Length - 1] == '/' ||
    txtMain.Text[txtMain.Text.Length - 1] == '%')
            {
                txtMain.Text = txtMain.Text.Substring(0, txtMain.Text.Length - 1);
            }
            //here
            PrevCalculation.PrevsOperation = OpType;
            txtMain.Text += btnOp.Text;
            OpType = btnOp.Text.ToString();
            Temp1 = "";
            Temp2 = "";
            isNewNumber = true;

        }
        private void btnDecimal_Click(object sender, EventArgs e)
        {

            if (IsResultShown)
            {

                txtMain.Clear();
                FirstNumber = Result;
                txtMain.Text = FirstNumber.ToString();
                txtResult.Clear();
                IsResultShown = false;
                txtResult.Font = new Font("Microsoft Sans Serif", 25.0f, FontStyle.Bold);
                txtResult.ForeColor = Color.Gray;
                isNewNumber = false;
            }
            if ((!Temp1.Contains(".") && txtMain.Text.Contains(".")) && !(FirstNumber.ToString().Contains(".")) || !Temp2.Contains(".") && !(SecondNumber.ToString().Contains(".")))
            {
                txtMain.Text += ".";
            }
            if (isNewNumber)
            {
                Temp2 += ".";
            }
        }
        private void btnAllClear_Click(object sender, EventArgs e)
        {
            FirstNumber = 0;
            SecondNumber = 0;
            OpType = "";
            isNewNumber = false;
            Temp1 = "";
            Temp2 = "";
            Result = 0;
            txtMain.Clear();
            txtResult.Clear();
            History.Clear();
        }
        private void PerformCalculation()
        {
            //PrevCalculation.PrevTemp1 = Temp1;
            //PrevCalculation.PrevTemp2 = Temp2;
            PrevCalculation.PrevResult = Result;
            PrevCalculation.PrevFirstNumber = FirstNumber;
            PrevCalculation.PrevSecondNumber = SecondNumber;
            //PrevCalculation.PrevsOperation = OpType;
            PrevCalculation.PrevisNewNumber = isNewNumber;
            History.Push(PrevCalculation);

            switch (OpType)
            {
                case "":

                    break;

                case "+":
                    {
                        Result = FirstNumber + SecondNumber;
                        break;
                    }
                case "-":
                    {
                        Result = FirstNumber - SecondNumber;
                        break;
                    }
                case "x":
                    {
                        Result = FirstNumber * SecondNumber;
                        break;
                    }
                case "%":
                    {
                        Result = FirstNumber % SecondNumber;
                        break;
                    }
                case "/":
                    {

                        if (SecondNumber != 0)
                        {
                            Result = FirstNumber / SecondNumber;
                        }
                        else
                        {
                            MessageBox.Show("Cannot divide by zero!");
                            return;
                        }
                        break;

                    }
            }
            
            txtResult.Text = Result.ToString();

        }
        void AllClear()
        {
            FirstNumber = 0;
            SecondNumber = 0;
            OpType = "";
            isNewNumber = false;
            Temp1 = "";
            Temp2 = "";
            Result = 0;
            txtMain.Text = string.Empty;
            txtResult.Text = string.Empty;
            History.Clear();
        }
        private void btnEqual_Click(object sender, EventArgs e)
        {
            
            IsResultShown = true;
            PerformCalculation();
            
            txtHistory.Text += FirstNumber.ToString() + OpType + SecondNumber.ToString() + " = " + Result.ToString() + Environment.NewLine;

            txtResult.Font = new Font("Microsoft Sans Serif", 36.0f, FontStyle.Bold);
            txtResult.ForeColor = Color.LimeGreen;
            FirstNumber = Result;
            SecondNumber = 0;
            
        }
        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            
            if (txtMain.Text == "")
            {
                AllClear();
            }

            if (History.Count > 1 && (txtMain.Text.EndsWith("+") ||
            txtMain.Text.EndsWith("-") ||
            txtMain.Text.EndsWith("x") ||
            txtMain.Text.EndsWith("/") ||
            txtMain.Text.EndsWith("%")))
            {
                
            Result = History.First().PrevResult;
            OpType = History.First().PrevsOperation;
            Temp1 = History.First().PrevTemp1;
            Temp2 = History.First().PrevTemp2;
            isNewNumber = History.First().PrevisNewNumber;
            History.Pop();
            }
            
            if (txtMain.Text.Length > 0)
            {
                txtMain.Text = txtMain.Text.Substring(0, txtMain.Text.Length - 1);
            }

            if(Result != 0)
            {
                txtResult.Text = Result.ToString();
            }
            

            

        
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pnlHistory.BringToFront();
        }
        private void picboxHistory_Click(object sender, EventArgs e)
        {
            pnlHistory.Visible = true;
            picboxBack.Visible = true;
        }
        private void btnClearHistory_Click(object sender, EventArgs e)
        {
            txtHistory.Clear();
        }
        private void picboxBack_Click(object sender, EventArgs e)
        {
            pnlHistory.Visible = false;
            picboxBack.Visible = false;
        }
    }
}

