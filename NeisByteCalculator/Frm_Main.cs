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
            thread = new Thread(CalculateThread);
            thread.IsBackground = true;
            thread.Start();
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
                        lbl_BytesInfo.Text = CalculateByteSize(input).ToString("#,##0") + " bytes";


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

            return bytes;// + CountCharacter(input, '\n');
        }





    }
}
