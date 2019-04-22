using CostsBL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostsUI
{
    /// <summary>
    /// Класс регистрации нового пользователя бд
    /// </summary>
    public partial class Form2 : Form
    {

        UserContext db;

        public Form2()
        {
            InitializeComponent();
            db = new UserContext();
        }

        /// <summary>
        /// Закрытие формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Сворачивание формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label20_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Регистрация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var users = db.RegistrationsTable;
            bool presence = false;  //флаг наличия такого имени
            Registration newUser = new Registration();
            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox2.Text))
            {
                newUser.Username = textBox1.Text;
                newUser.Password = textBox2.Text;               
            }
            else
            {
                MessageBox.Show("Пустых символов и полей не должно быть");
            }
            if (db != null)
            {
                //проверяем наличие логина в бд
                foreach (var u in users)
                {
                    if (u.Username.ToLower() == textBox1.Text.ToLower())
                    {
                        MessageBox.Show("Имя пользователя уже существует");
                        presence = true;
                        break;
                    }
                }
                if (!presence)
                {
                    db.RegistrationsTable.Add(newUser);
                    db.SaveChanges();
                    MessageBox.Show("Новый пользователь зарегистрирован");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Нет подключения к БД или ошибка создания пользователя");
            }
            textBox1.Text = "";
            textBox2.Text = "";
        }

        /// <summary>
        /// Перетаскивание формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }
    }
}
