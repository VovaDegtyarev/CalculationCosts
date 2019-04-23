using CostsBL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostsUI
{
    /// <summary>
    /// Основная форма для работы с данными
    /// </summary>
    public partial class Form3 : Form
    {

        UserContext db = new UserContext();
        static double sum = 0;

        public Form3()
        {
            InitializeComponent();
            //Task.Run(() => LoadDataFromDB());
            LoadDataFromDB();
        }

        /// <summary>
        /// Закрытие формы. Выход.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label3_Click(object sender, EventArgs e)
        {
            db.Dispose();
            Application.Exit();
        }

        /// <summary>
        /// Сворачивание формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        /// <summary>
        /// Обновление datagridview
        /// </summary>
        private void LoadDataFromDB()
        {
            UserContext db = new UserContext();
            db.ProductsTable.Load();
            //dataGridView1.Invoke((Action)delegate
            //{
            dataGridView1.DataSource = db.ProductsTable.Local.ToBindingList();
            //});
            ColorRowsTable();
        }

        /// <summary>
        /// обобщенный метод получения данных (не используется пока)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IQueryable<TEntity> Select <TEntity>() where TEntity : class
        {
            UserContext db = new UserContext();
            return db.Set<TEntity>();
        }
        

        /// <summary>
        /// Форма добавления данных в бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        /// <summary>
        /// Принудельное обновление формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            LoadDataFromDB();
        }
        
        /// <summary>
        /// Перетаскивание формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        /// <summary>
        /// Цвет строк в таблице
        /// </summary>
        private void ColorRowsTable()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (i % 2 == 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form3_Load(object sender, EventArgs e)
        {
            LoadDataFromDB();
            ColorRowsTable();
        }

        /// <summary>
        /// Фильтр данных в заданном промежутке дат
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;

            if (startDate < endDate)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Index == dataGridView1.RowCount)
                    {
                        break;
                    }
                    string deadline = row.Cells[2].Value.ToString();
                    DateTime deadlineRow = Convert.ToDateTime(deadline);
                    if (startDate <= deadlineRow && deadlineRow <= endDate)
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        dataGridView1.CurrentCell = null;
                        row.Visible = false;
                    }
                }               
            }
            else
            {
                MessageBox.Show("Проверьте корректность выбранной даты");
                return;
            }
            ColorRowsTable();
        }

        /// <summary>
        /// Сброс значений поиска после фильтра
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Visible = true;
            }
        }

        /// <summary>
        /// Поиск записи по названию и выделение её курсором
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox2.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            break;
                        }
            }
        }

        /// <summary>
        /// Сброс результатов поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            ColorRowsTable();
        }

        /// <summary>
        /// Отображение дополнительных данных по продукту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            db = new UserContext();
            int idProduct = -1;
            var employeeCost = db.EmoloyeeCostsTable;
            var costPrice = db.CostPricesTable;

            //поиск id в строке продуктов
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                idProduct = Convert.ToInt32(selectedRow.Cells[3].Value);
            }

            foreach (var empl in employeeCost)
            {
                if(empl.idLink == idProduct)
                {
                    label8.Text = empl.Salary.ToString();
                    label9.Text = empl.Training.ToString();
                }
            }
            foreach (var costPr in costPrice)
            {
                if (costPr.idLink == idProduct)
                {
                    label18.Text = costPr.MaterilaCost.ToString();
                    label19.Text = costPr.SemiFinishedProducts.ToString();
                    label20.Text = costPr.FuelAndEnergyCosts.ToString();
                    label21.Text = costPr.Depreciation.ToString();
                    label22.Text = costPr.Insurance.ToString();
                    label23.Text = costPr.Transport.ToString();
                    label24.Text = costPr.Sales.ToString();
                    label25.Text = costPr.Other.ToString();
                }
            }
        }

        /// <summary>
        /// Удаление данных (выделенная строка) //может быть добавить удаление по check box ?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            int idProduct = -1;
            DialogResult dialogResult = MessageBox.Show("Вы действительно хотите удалить строку?", "Удаление данных", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                    idProduct = Convert.ToInt32(selectedRow.Cells[3].Value);
                }
                Product product = db.ProductsTable.Find(idProduct);
                if(product != null)
                {
                    db.ProductsTable.Remove(product);
                    db.SaveChanges();
                }
                MessageBox.Show("Продукт удален");
            }
            LoadDataFromDB();
        }

        /// <summary>
        /// Вызов формы редактирования данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            int idProduct = -1;
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                idProduct = Convert.ToInt32(selectedRow.Cells[3].Value);
            }

            Form5 form5 = new Form5(idProduct);
            form5.ShowDialog();

        }
        
        //добавить единичный расчёт без checkbox'a
        /// <summary>
        /// Выбор расчёта типа себестоимости.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int idProduct = -1;
            var employeeCost = db.EmoloyeeCostsTable;
            var costPrice = db.CostPricesTable;
            int countCheck = 0;
            
            string select;
            select = comboBox1.SelectedItem.ToString();

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (Convert.ToBoolean(dataGridView1[0, i].Value) == true)
                {
                    countCheck++;
                }
            }
      
            switch (select)
            {
                case "Расходы на реализацию":
                    if (countCheck > 0)
                    {
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (Convert.ToBoolean(dataGridView1[0, i].Value) == true)
                            {
                                idProduct = Convert.ToInt32(dataGridView1[3, i].Value);
                                foreach (var empl in employeeCost)
                                {
                                    if (empl.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(empl.Salary); //не уверен прибавляется ли зп в данном случае 
                                    }
                                }
                                foreach (var costPr in costPrice)
                                {
                                    if (costPr.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(costPr.FuelAndEnergyCosts + costPr.Transport + costPr.Other);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                        idProduct = Convert.ToInt32(selectedRow.Cells[3].Value);
                        foreach (var empl in employeeCost)
                        {
                            if (empl.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(empl.Salary); //не уверен прибавляется ли зп в данном случае 
                            }
                        }
                        foreach (var costPr in costPrice)
                        {
                            if (costPr.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(costPr.FuelAndEnergyCosts + costPr.Transport + costPr.Other);
                            }
                        }
                    }
                    MessageBox.Show(sum.ToString(), "Расходы на реализацию", MessageBoxButtons.OK);
                    sum = 0;
                    break;
                case "Расходы на рабочую силу":
                    if (countCheck > 0)
                    {
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (Convert.ToBoolean(dataGridView1[0, i].Value) == true)
                            {
                                idProduct = Convert.ToInt32(dataGridView1[3, i].Value);
                                foreach (var empl in employeeCost)
                                {
                                    if (empl.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(empl.Salary + empl.Training);
                                    }
                                }
                                foreach (var costPr in costPrice)
                                {
                                    if (costPr.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(costPr.Other);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                        idProduct = Convert.ToInt32(selectedRow.Cells[3].Value);
                        foreach (var empl in employeeCost)
                        {
                            if (empl.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(empl.Salary + empl.Training);
                            }
                        }
                        foreach (var costPr in costPrice)
                        {
                            if (costPr.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(costPr.Other);
                            }
                        }
                    }
                    MessageBox.Show(sum.ToString(), "Расходы на рабочую силу", MessageBoxButtons.OK);
                    sum = 0;
                    break;
                case "Производственная себестоимость":
                    if (countCheck > 0)
                    {
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (Convert.ToBoolean(dataGridView1[0, i].Value) == true)
                            {
                                idProduct = Convert.ToInt32(dataGridView1[3, i].Value);
                                foreach (var empl in employeeCost)
                                {
                                    if (empl.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(empl.Salary + empl.Training);
                                    }
                                }
                                foreach (var costPr in costPrice)
                                {
                                    if (costPr.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(costPr.MaterilaCost + costPr.SemiFinishedProducts + costPr.FuelAndEnergyCosts +
                                        costPr.Insurance + costPr.Sales);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                        idProduct = Convert.ToInt32(selectedRow.Cells[3].Value);
                        foreach (var empl in employeeCost)
                        {
                            if (empl.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(empl.Salary + empl.Training);
                            }
                        }
                        foreach (var costPr in costPrice)
                        {
                            if (costPr.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(costPr.MaterilaCost + costPr.SemiFinishedProducts + costPr.FuelAndEnergyCosts +
                                costPr.Insurance + costPr.Sales);
                            }
                        }
                    }
                    MessageBox.Show(sum.ToString(), "Полная себестоимость", MessageBoxButtons.OK);
                    sum = 0;
                    break;
                case "Полная себестоимость":
                    //если кратко, то как только найдём true в checkbox запоминаем id продукта и по его связям делаем sum+= нужных значений
                    if (countCheck > 0)
                    {
                        for (int i = 0; i < dataGridView1.RowCount; i++)
                        {
                            if (Convert.ToBoolean(dataGridView1[0, i].Value) == true)
                            {
                                idProduct = Convert.ToInt32(dataGridView1[3, i].Value);
                                foreach (var empl in employeeCost)
                                {
                                    if (empl.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(empl.Salary + empl.Training);
                                    }
                                }
                                foreach (var costPr in costPrice)
                                {
                                    if (costPr.idLink == idProduct)
                                    {
                                        sum += Convert.ToDouble(costPr.MaterilaCost + costPr.SemiFinishedProducts + costPr.FuelAndEnergyCosts +
                                        costPr.Depreciation + costPr.Insurance + costPr.Transport + costPr.Sales + costPr.Other);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                        idProduct = Convert.ToInt32(selectedRow.Cells[3].Value);
                        foreach (var empl in employeeCost)
                        {
                            if (empl.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(empl.Salary + empl.Training);
                            }
                        }
                        foreach (var costPr in costPrice)
                        {
                            if (costPr.idLink == idProduct)
                            {
                                sum += Convert.ToDouble(costPr.MaterilaCost + costPr.SemiFinishedProducts + costPr.FuelAndEnergyCosts +
                                costPr.Depreciation + costPr.Insurance + costPr.Transport + costPr.Sales + costPr.Other);
                            }
                        }
                    }
                    MessageBox.Show(sum.ToString(), "Полная себестоимость", MessageBoxButtons.OK);
                    sum = 0;
                    break;
                default:
                    break;
            }
        }
    }
}
