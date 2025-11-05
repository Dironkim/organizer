using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Organizer
{
    public partial class MainForm : Form
    {
        string FileName;
        public MainForm()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            if (RecordTextBox.Text.Replace(" ", "") != "")
            {
                CurrentListBox.Items.Add(RecordTextBox.Text);
                CurrentListBox.SelectedIndex = CurrentListBox.Items.Count-1;
                RecordTextBox.Text = (string)CurrentListBox.SelectedItem;
            }
            else MessageBox.Show("Пустое поле ввода");
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            RecordTextBox.Text = (string)CurrentListBox.SelectedItem;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FileName = dateTimePicker1.Text + "org";
            LoadFromFile(FileName);

        }
        private void SaveToFile(string FileName)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FileName))
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        //задаем текущий компонент ListBox
                        ListBox CurListBox = (ListBox)Controls.Find("listBox" + i, true)[0];
                        //записываем в файл кол-во строк списка
                        sw.WriteLine(CurListBox.Items.Count.ToString());
                        //записываем в файл все записи из списка
                        for (int j = 0; j < CurListBox.Items.Count; j++)
                            sw.WriteLine(CurListBox.Items[j]);
                        //очищаем список записей текущего ListBox
                        CurListBox.Items.Clear();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении!");
            }
        }

        private void LoadFromFile(string FileName)
        {
            try
            {
                string tempString;
                int tempCount;
                bool succ;
                using (StreamReader sw = new StreamReader(FileName))
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        //задаем текущий компонент ListBox
                        ListBox CurListBox = (ListBox)Controls.Find("listBox" + i, true)[0];
                        //кол-во строк списка
                        tempString=sw.ReadLine();
                        succ = int.TryParse(tempString, out tempCount);
                        if (succ)
                        {
                            CurListBox.Items.Clear();
                            for (int j = 0; j < tempCount; j++)
                            {
                                tempString = sw.ReadLine();
                                CurListBox.Items.Add(tempString);
                            }
                            
                        }
                    }
                }
            }
            catch
            {
                for (int i = 1; i <= 4; i++)
                {
                    ListBox CurListBox = (ListBox)Controls.Find("listBox" + i, true)[0];
                    CurListBox.Items.Clear();
                }
                //MessageBox.Show("Ошибка при загрузке из файла!");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SaveToFile(FileName);
            FileName = dateTimePicker1.Text + "org";
            LoadFromFile(FileName);
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            try
            {
                string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
                ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
                int ind = CurrentListBox.SelectedIndex;
                CurrentListBox.Items.RemoveAt(ind);
                CurrentListBox.Items.Insert(ind, RecordTextBox.Text);
                CurrentListBox.SelectedIndex = ind;
            }
            catch
            {
                MessageBox.Show("Не выбран элемент для изменения!");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
                ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
                int ind = CurrentListBox.SelectedIndex;
                CurrentListBox.Items.RemoveAt(ind);
                if (CurrentListBox.Items.Count != 0)
                {
                    if (ind == CurrentListBox.Items.Count)//ind с нуля, но Count только что уменьшился, -1 не нужно 
                        CurrentListBox.SelectedIndex = ind - 1;
                    else CurrentListBox.SelectedIndex = ind;
                    RecordTextBox.Text = (string)CurrentListBox.SelectedItem;
                }
            }
            catch
            {
                MessageBox.Show("Не выбран элемент для удаления!");
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            while (CurrentListBox.Items.Count > 0)
            {
                CurrentListBox.Items.RemoveAt(0);
            }
            RecordTextBox.Text = "";
        }

        private void RecordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue==13)
            {
                string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
                ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
                CurrentListBox.Items.Add(RecordTextBox.Text);
                RecordTextBox.Text = "";
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            SaveToFile(FileName);
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString() + " " +DateTime.Today.DayOfWeek.ToString();
        }
    }
}
