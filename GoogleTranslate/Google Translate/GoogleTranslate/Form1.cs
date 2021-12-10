using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GoogleTranslate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string TranslateText(string input)
        {            
            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             lang_first, lang_second, Uri.EscapeUriString(input));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;            
            var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);            
            var translationItems = jsonData[0];            
            string translation = "";           
            foreach (object item in translationItems)
            {                
                IEnumerable translationLineObject = item as IEnumerable;                
                IEnumerator translationLineString = translationLineObject.GetEnumerator();              
                translationLineString.MoveNext();                
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }
            if (translation.Length > 1) { translation = translation.Substring(1); };           
            return translation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           textBox2.Text = TranslateText(textBox1.Text); 
            string file = @"history.txt";
            if(System.IO.File.Exists(file)==false)
            {
                MessageBox.Show("Vui lòng tạo file history.txt cạnh Application!");
            }
            else
            {
                string str = " " + textBox1.Text + "=" + textBox2.Text + Environment.NewLine;
                System.IO.File.AppendAllText(file, str);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            linkLabel1.Links.Add(0, 15, "www.facebook.com/berlin.03");
        }
        
        string lang_first = "auto";
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Tiếng Anh") lang_first = "en";
            if (comboBox1.Text == "Tiếng Việt") lang_first = "vi";
            if (comboBox1.Text == "Tiếng Trung") lang_first = "zh-tw";
            if (comboBox1.Text == "Phát hiện ngôn ngữ") lang_first = "auto";
        }
        string lang_second = "en";
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Tiếng Anh") lang_second = "en";
            if (comboBox2.Text == "Tiếng Việt") lang_second = "vi";
            if (comboBox2.Text == "Tiếng Trung") lang_second = "zh-tw";
            if (comboBox2.Text == "Tiếng Pháp") lang_second = "fr";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filehistory = @"history.txt";
            if (System.IO.File.Exists(filehistory) == false)
            {
                MessageBox.Show("Vui lòng tạo file history.txt cạnh Application!");
            }
            else
            {
                string str = System.IO.File.ReadAllText(filehistory);
                MessageBox.Show(str, "Lịch sử");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filehistory = @"history.txt";
            if (System.IO.File.Exists(filehistory) == false)
            {
                MessageBox.Show("Vui lòng tạo file history.txt cạnh Application!");
            }
            else
            {
                string str = "";
                System.IO.File.WriteAllText(filehistory,str);
                MessageBox.Show("Thành công!", "Xóa lịch sử");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
        
        private void textBox1_TextChanged(object sender,EventArgs e)
        {
         
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox2.Text = TranslateText(textBox1.Text);
            }
        }
    }
}
