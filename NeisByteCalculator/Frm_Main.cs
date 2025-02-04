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

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int strlen(string str);

        private Thread thread;

        public Frm_Main()
        {
            InitializeComponent();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            // 글자 수 계산을 Thread로 실행하기
            thread = new Thread(CalculateThread);
            thread.IsBackground = true;
            thread.Start();

            // 테스트를 위한 글자 입력 => 나이스 결과 : 공백 제외 90자, 공백 포함 99자, 157바이트
            richTextBox1.Text = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()-_=+`~;:[]{}'/?\n동해물과 백두산이 마르고 닳도록\n하느님이 보우하사 우리 나라 만세!";

        }

        private void CalculateThread()
        {
            try
            {
                while (true)
                {
                    Invoke(new Action(() =>
                    {
                        string input = richTextBox1.Text;
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

            return bytes + CountCharacter(input, '\n');
        }


    }
}
