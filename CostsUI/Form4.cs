﻿using CostsBL.Model;
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
    /// Класс обработки добавления
    /// </summary>
    public partial class Form4 : Form
    {

        UserContext db;

        public Form4()
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
        /// Свернуть
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label20_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Добавить данные в бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите название продукта!");
                return;
            }
            else
            {
                //создание нового продукта
                Product newProduct = new Product();
                newProduct.ProductName = textBox1.Text;
                newProduct.Date = dateTimePicker1.Value;
                db.ProductsTable.Add(newProduct);
                db.SaveChanges();

                //создание объекта затрат на продукт (по умолчанию значения будут = 0)
                CostPrice newCostPrice = new CostPrice();
                newCostPrice.MaterilaCost = Convert.ToDouble(textBox30.Text);
                newCostPrice.SemiFinishedProducts = Convert.ToDouble(textBox31.Text);
                newCostPrice.FuelAndEnergyCosts = Convert.ToDouble(textBox32.Text);
                newCostPrice.Depreciation = Convert.ToDouble(textBox33.Text);
                newCostPrice.Insurance = Convert.ToDouble(textBox34.Text);
                newCostPrice.Transport = Convert.ToDouble(textBox35.Text);
                newCostPrice.Sales = Convert.ToDouble(textBox36.Text);
                newCostPrice.Other = Convert.ToDouble(textBox37.Text);
                newCostPrice.idLink = newProduct.id;

                //создание объекта затрат на сотрудников обслуживающих продукт (по умолчанию значения будут = 0)
                EmployeeCosts newEmployeeCosts = new EmployeeCosts();
                newEmployeeCosts.Salary = Convert.ToDouble(textBox3.Text);
                newEmployeeCosts.Training = Convert.ToDouble(textBox4.Text);
                newEmployeeCosts.idLink = newProduct.id;


                db.CostPricesTable.Add(newCostPrice);
                db.EmoloyeeCostsTable.Add(newEmployeeCosts);
                db.SaveChanges();

                MessageBox.Show("Продукт добавлен");
            }
            //выделить в отдельный метод!!!!!!!!
            textBox1.Text = "";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox30.Text = "0";
            textBox31.Text = "0";
            textBox32.Text = "0";
            textBox33.Text = "0";
            textBox34.Text = "0";
            textBox35.Text = "0";
            textBox36.Text = "0";
            textBox37.Text = "0";
        }

        /// <summary>
        /// Очистка полей заполнения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox30.Text = "0";
            textBox31.Text = "0";
            textBox32.Text = "0";
            textBox33.Text = "0";
            textBox34.Text = "0";
            textBox35.Text = "0";
            textBox36.Text = "0";
            textBox37.Text = "0";
        }

        /// <summary>
        /// Перетаскивание формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form4_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        /// <summary>
        /// Проверка на корректность ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            //tb = (TextBox)sender;
            //tb.Text = tb.Text.Replace(".", ",");
            //if (Char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != 46)
            //{
            //    e.Handled = true;
            //}
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44)
            {
                e.Handled = true;
            }
        }
    }
}
