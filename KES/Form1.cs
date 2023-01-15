using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z.Dapper.Plus;

namespace KES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbSheet_SelectedIndexChanged(object sender, EventArgs e)//указание имени таблицы для связи данных с DataGridView
        {
            DataTable dt = tableCollection[cmbSheet.SelectedItem.ToString()];
           // dataGridView1.DataSource = dt;
           if(dt != null)
            {
                List < customer > customers= new List < customer >();
                for(int i=0;i<dt.Rows.Count; i++)
                {
                    //
                    customer customer= new customer();
                    customer.id = dt.Rows[i]["id"].ToString();
                    customer.name_of= dt.Rows[i]["Наименование процесса"].ToString();
                    customer.podrazd_of = dt.Rows[ i] ["Подразделение"].ToString();
                    customers.Add(customer);
                }
                customerBindingSource.DataSource = customers;
            }
        }
        DataTableCollection tableCollection;
        //чтение эксель файлов
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx" })//выбор формата
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtName.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                                { UseHeaderRow = true }

                            });
                            //чтение всех имен в таблиц и добавление имя таблицы в поле со списком 
                            tableCollection = result.Tables;
                            cmbSheet.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                cmbSheet.Items.Add(table.TableName);
                        }

                    }
                }
            }
        }

        private void brnImport_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server=;Database=db_test;user id=sa; Password=1";//подключение к серверу
                DapperPlusManager.Entity<customer>().Table("Customers");
                List<customer> customers = customerBindingSource.DataSource as List<customer>;
                if(customers != null)
                {
                    using(IDbConnection db =new SqlConnection(connectionString))
                    {
                        db.BulkInsert(customers);//для использования больших данных в таблицах
                    }
                }
                MessageBox.Show("ок");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}