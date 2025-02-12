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

        private bool bEdited = false;

        public Frm_Main()
        {
            InitializeComponent();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            // font list
            foreach (FontFamily ff in FontFamily.Families)
            {
                cbo_FontName.Items.Add(ff.Name);
                if (ff.Name.Equals(txt_Input.Font.Name))
                    cbo_FontName.SelectedIndex = cbo_FontName.Items.Count - 1;
            }

            cbo_FontSize.Items.AddRange(new string[] {"8", "9", "10", "11", "12","14", "16", "18", "20","22","24","26","28","36","48","72" });
            cbo_FontSize.Text = txt_Input.Font.Size.ToString();

            // split container에 전체 크기로 자동 변경
            txt_Input.Dock = DockStyle.Fill;

            // 테스트를 위한 글자 입력 => 나이스 결과 : 공백 제외 90자, 공백 포함 99자, 157바이트
            txt_Input.Text = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()-_=+`~;:[]{}'/?\n동해물과 백두산이 마르고 닳도록\n하느님이 보우하사 우리 나라 만세!";
        }


        public static int CountCharacter(string input, char targetChar)
        {
            // 문자 갯수를 세는 방법에 여러가지가 있지만, 이 방법이 가장 빠르다고 한다.
            // https://stackoverflow.com/questions/541954/how-to-count-occurrences-of-a-char-string-within-a-string?rq=1
            // 다른 방법으로는 Split을 사용하는 방법, Regular Expression을 사용하는 방법, IndexOf를 사용하는 방법, Count를 사용하는 방법 등 다양한 방법이 존재한다.
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
            int bytes = Encoding.UTF8.GetBytes(input).Length;   // UTF8 Encoder를 사용하면 한글 1글자를 3Byte로 처리 함.
            return bytes + CountCharacter(input, '\r');     // 엔터키가 한바이트로 처리(\r) 되므로 엔터키 갯수만큼 증가
                                                            // RichTextBox에서는 Enter를 \n으로 인식했는데, CustomRichTextBox는 \r로 인식 함.
        }


        private void UpdateCharacterCounters()
        {
            string input = txt_Input.Text;
            int bytes = CalculateByteSize(input);
            int countChar = input.Length;
            int countCharWithoutSpace = input.Replace(" ", "").Replace("\r", "").Length;    // Space와 Enter 제거

            lbl_BytesInfo.Text = $"공백 제외 {countCharWithoutSpace}자, 공백 포함 {countChar}자, {bytes.ToString("#,##0")}바이트";
            Console.WriteLine($"공백 제외 {countCharWithoutSpace}자, 공백 포함 {countChar}자, {bytes.ToString("#,##0")}바이트");
        }


        private void txt_Input_TextChanged(object sender, EventArgs e)
        {
            UpdateCharacterCounters();
            bEdited = true;
        }

        private void cmd_Open_Click(object sender, EventArgs e)
        {
            if (bEdited)
            {
                if (MessageBox.Show("내용이 수정되었으나 저장하지 않았습니다. 저장 후 파일열기를 하시겠습니까?", "열기", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd_Save.PerformClick();
                }
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!OpenFile(dialog.FileName, RichTextBoxStreamType.RichText))
                {
                    if (!OpenFile(dialog.FileName, RichTextBoxStreamType.PlainText))
                    {
                        MessageBox.Show(null, "파일 열기에 실패했습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                bEdited = false;
            }
        }


        private bool OpenFile(string fileName, RichTextBoxStreamType type)
        {
            try
            {
                txt_Input.LoadFile(fileName, type);
                cbo_FontName.Text = txt_Input.Font.Name;
                cbo_FontSize.Text = txt_Input.Font.Size.ToString();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        private void cmd_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;
                Console.WriteLine(System.IO.Path.GetExtension(fileName));
                if (!System.IO.Path.GetExtension(fileName).Equals(".rtf", StringComparison.OrdinalIgnoreCase))
                {
                    if (fileName.Substring(fileName.Length - 1, 1).Equals("."))
                        fileName += "rtf";
                    else
                        fileName += ".rtf";
                }

                txt_Input.SaveFile(fileName, RichTextBoxStreamType.RichText);
                bEdited = false;
            }
        }

        private void cbo_FontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectIndex = txt_Input.SelectionStart;
            int selectLength = txt_Input.SelectionLength;
            try
            {
                Console.WriteLine("font size update");
                txt_Input.SelectAll();
                txt_Input.SelectionFont = new Font(cbo_FontName.Text, txt_Input.Font.Size);
            }
            catch (Exception)
            {
            }
            finally
            {
                txt_Input.SelectionStart = selectIndex;
                txt_Input.SelectionLength = selectLength;
            }
        }

        private void cbo_FontSize_TextUpdate(object sender, EventArgs e)
        {
        }

        private void cbo_FontSize_TextChanged(object sender, EventArgs e)
        {
            int selectIndex = txt_Input.SelectionStart;
            int selectLength = txt_Input.SelectionLength;
            try
            {
                Console.WriteLine("font size update");
                txt_Input.SelectAll();
                txt_Input.SelectionFont = new Font(cbo_FontName.Text, float.Parse(cbo_FontSize.Text));
            }
            catch (Exception)
            {
            }
            finally
            {
                txt_Input.SelectionStart = selectIndex;
                txt_Input.SelectionLength = selectLength;
            }
        }

        private void cmd_ForeColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (txt_Input.SelectedText.Length > 0)
                {
                    txt_Input.SelectionColor = dialog.Color;
                }
            }
        }

        private void cmd_BackColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (txt_Input.SelectedText.Length > 0)
                {
                    txt_Input.SelectionBackColor = dialog.Color;
                }
            }
        }

        private void cmd_Exit_Click(object sender, EventArgs e)
        {
            if (bEdited)
            {
                if (MessageBox.Show("내용이 수정되었으나 저장하지 않았습니다. 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            Application.Exit();
        }

    }
}
