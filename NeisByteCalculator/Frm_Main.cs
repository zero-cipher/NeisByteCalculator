using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeisByteCalculator
{


    public partial class Frm_Main : Form
    {

        //private Thread thread;

        //private CustomRichTextBox txt_Input;

        public Frm_Main()
        {
            InitializeComponent();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            /*
                        txt_Input = new CustomRichTextBox();
                        txt_Input.ImeMode = System.Windows.Forms.ImeMode.NoControl;
                        txt_Input.Location = new System.Drawing.Point(3, 24);
                        txt_Input.Size = new System.Drawing.Size(785, 171);
                        txt_Input.Text = "";
                        txt_Input.TextChanged += new System.EventHandler(txt_Input_TextChanged);

                        this.splitContainer1.Panel1.Controls.Add(txt_Input);
            */
            /*

            // 글자 수 계산을 Thread로 실행하기
            thread = new Thread(CalculateThread);
            thread.IsBackground = true;
            thread.Start();

            */

            // 테스트를 위한 글자 입력 => 나이스 결과 : 공백 제외 90자, 공백 포함 99자, 157바이트
            customRichTextBox1.Text = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()-_=+`~;:[]{}'/?\n동해물과 백두산이 마르고 닳도록\n하느님이 보우하사 우리 나라 만세!";

        }

        private void CalculateThread()
        {
            try
            {
                while (true)
                {
                    Invoke(new Action(() =>
                    {
                        string input = customRichTextBox1.Text;
                        int bytes = CalculateByteSize(input);
                        int countChar = input.Length;
                        int countCharWithoutSpace = input.Replace(" ", "").Replace("\n", "").Length;

                        lbl_BytesInfo.Text = $"공백 제외 {countCharWithoutSpace}자, 공백 포함 {countChar}자, {bytes.ToString("#,##0")}바이트";
                    }));

                    Thread.Sleep(1);
                }
            }
            catch (Exception)
            {

            }
        }



        public static int CountCharacter(string input, char targetChar)
        {
            int count = 0;
            foreach (char c in input)
            {
                if (c == targetChar)
                    count++;
            }
            return count;
        }
        public static int CalculateByteSize(string input)
        {
            int bytes = Encoding.UTF8.GetBytes(input).Length;

            return bytes + CountCharacter(input, '\r');
        }


        private void UpdateCharacterCounters()
        {
            string input = customRichTextBox1.Text;
            int bytes = CalculateByteSize(input);
            int countChar = input.Length;
            int countCharWithoutSpace = input.Replace(" ", "").Replace("\r", "").Length;

            lbl_BytesInfo.Text = $"공백 제외 {countCharWithoutSpace}자, 공백 포함 {countChar}자, {bytes.ToString("#,##0")}바이트";
            Console.WriteLine($"공백 제외 {countCharWithoutSpace}자, 공백 포함 {countChar}자, {bytes.ToString("#,##0")}바이트");
        }

        private void txt_Input_TextChanged(object sender, EventArgs e)
        {
            UpdateCharacterCounters();
        }

        private void customRichTextBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateCharacterCounters();
        }
    }
}
