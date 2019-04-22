using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CostsBL.Model;

namespace CostsUI
{
    /// <summary>
    /// Класс формы авторизации
    /// </summary>
    public partial class Form1 : Form
    {
        UserContext db;

        public Form1()
        {
            InitializeComponent();
            db = new UserContext();
        }

        /// <summary>
        /// Закрытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            db.Dispose();
            Application.Exit();
        }

        /// <summary>
        /// Сворачивание формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Перетаскиване формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        /// <summary>
        /// Форма регистрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        /// <summary>
        /// Наведение красоты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            panel1.BackColor = Color.FromArgb(3, 67, 106);
            textBox1.ForeColor = Color.FromArgb(3, 67, 106);

            panel2.BackColor = Color.WhiteSmoke;
            textBox2.ForeColor = Color.Gray;
        }

        /// <summary>
        /// Наведение красоты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox2.PasswordChar = '•'; // alt + 0149

            panel2.BackColor = Color.FromArgb(3, 67, 106);
            textBox2.ForeColor = Color.FromArgb(3, 67, 106);

            panel1.BackColor = Color.WhiteSmoke;
            textBox1.ForeColor = Color.Gray;
        }

        /// <summary>
        /// Подключиться к БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            bool presence = false;
            var users = db.RegistrationsTable;
            if(string.IsNullOrWhiteSpace(textBox1.Text) && string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Поля не могут быть пустыми");
                return;
            }
            foreach (var user in users)
            {
                if(user.Username == textBox1.Text && user.Password == textBox2.Text)
                {
                    Form3 form3 = new Form3();
                    this.Hide();
                    form3.Show();
                    presence = true;
                    return;
                }
            }
            if (!presence)
            {
                MessageBox.Show("Проверьте логин и пароль");
                textBox1.Text = "";
                textBox2.Text = "";
            }
                
        }
    }
}
