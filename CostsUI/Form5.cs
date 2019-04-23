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
    /// Редактирование данных
    /// </summary>
    public partial class Form5 : Form
    {

        UserContext db;
        int idprod;
        public Form5(int idCurrentProduct)
        {
            InitializeComponent();
            db = new UserContext();
            LoadDataFromDB(idCurrentProduct);
        }

        /// <summary>
        /// Выгрузка данных для конкретной выделенной строки. На вход функции подаётся id продукта
        /// </summary>
        /// <param name="idProduct"></param> 
        private void LoadDataFromDB(int idProduct)
        {
            idprod = idProduct;
            var employeeCost = db.EmoloyeeCostsTable;
            var costPrice = db.CostPricesTable;

            Product product = db.ProductsTable.Find(idProduct);
            textBox1.Text = product.ProductName;
            dateTimePicker1.Value = product.Date;

            foreach (var empl in employeeCost)
            {
                if (empl.idLink == idProduct)
                {
                    textBox3.Text = empl.Salary.ToString();
                    textBox4.Text = empl.Training.ToString();
                }
            }
            foreach (var costPr in costPrice)
            {
                if (costPr.idLink == idProduct)
                {
                    textBox30.Text = costPr.MaterilaCost.ToString();
                    textBox31.Text = costPr.SemiFinishedProducts.ToString();
                    textBox32.Text = costPr.FuelAndEnergyCosts.ToString();
                    textBox33.Text = costPr.Depreciation.ToString();
                    textBox34.Text = costPr.Insurance.ToString();
                    textBox35.Text = costPr.Transport.ToString();
                    textBox36.Text = costPr.Sales.ToString();
                    textBox37.Text = costPr.Other.ToString();
                }
            }
        }

        /// <summary>
        /// Применить измененные данные, загрузив их в бд
        /// </summary>
        private void LoadDataToDB()
        {
            var employeeCost = db.EmoloyeeCostsTable;
            var costPrice = db.CostPricesTable;

            if (idprod >= 0)
            {
                Product product = db.ProductsTable.Find(idprod);
                product.ProductName = textBox1.Text;
                product.Date = dateTimePicker1.Value;
                foreach (var empl in employeeCost)
                {
                    if (empl.idLink == idprod)
                    {
                        empl.Salary = Convert.ToDouble(textBox3.Text);
                        empl.Training = Convert.ToDouble(textBox4.Text);
                        textBox3.Text = "0";
                        textBox4.Text = "0";
                    }
                }
                foreach (var costPr in costPrice)
                {
                    if (costPr.idLink == idprod)
                    {
                        costPr.MaterilaCost = Convert.ToDouble(textBox30.Text);
                        costPr.SemiFinishedProducts = Convert.ToDouble(textBox31.Text);
                        costPr.FuelAndEnergyCosts = Convert.ToDouble(textBox32.Text);
                        costPr.Depreciation = Convert.ToDouble(textBox33.Text);
                        costPr.Insurance = Convert.ToDouble(textBox34.Text);
                        costPr.Transport = Convert.ToDouble(textBox35.Text);
                        costPr.Sales = Convert.ToDouble(textBox36.Text);
                        costPr.Other = Convert.ToDouble(textBox37.Text);
                        textBox30.Text = "0";
                        textBox31.Text = "0";
                        textBox32.Text = "0";
                        textBox33.Text = "0";
                        textBox34.Text = "0";
                        textBox35.Text = "0";
                        textBox36.Text = "0";
                        textBox37.Text = "0";
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Изменения приняты");
            }
            else
            {
                MessageBox.Show("Ошибка");
            }
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
        /// Свернуть форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label20_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Применить изменения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            LoadDataToDB();
        }

        /// <summary>
        /// Перетаскивание формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form5_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }
    }
}
