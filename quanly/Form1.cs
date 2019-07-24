using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data.SqlClient;  
using MySql.Data.MySqlClient;

namespace quanly
{
    

    public partial class Form1 : Form
    {
        string connectionString = "Server=sql12.freemysqlhosting.net;Port=3306;Database=sql12289053;Uid=sql12289053;Pwd=RIn2XtkGUX;charset=utf8";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string connectionString = "Server=localhost;Port=3306;Database=repair;Uid=root;Pwd=;";
            //string connectionString = "Server=sql12.freemysqlhosting.net;Port=3306;Database=sql12289053;Uid=sql12289053;Pwd=RIn2XtkGUX;";
            //MySqlConnection con = new MySqlConnection(connectionString);
            //MySqlCommand cmd = con.CreateCommand();
            //cmd.CommandText = "select * from user";
            //con.Open();


        }
        public MySqlConnection conntectToDatasbe()
        {
           // string connectionString = "Server=sql12.freemysqlhosting.net;Port=3306;Database=sql12289053;Uid=sql12289053;Pwd=RIn2XtkGUX;";
            // string connectionString = "Server=localhost;Port=3306;Database=repair;Uid=root;Pwd=;";
            MySqlConnection con = new MySqlConnection(connectionString);
            return con;
        }
        public void loadDatabase()
        {
            listView1.Clear();
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Tên Khách Hàng");
            listView1.Columns.Add("Địa Chỉ");
            listView1.Columns.Add("Số Điện Thoại");
            // connection to database 
            MySqlConnection con = conntectToDatasbe();
            String query = "select * from user";
            MySqlDataAdapter ad = new MySqlDataAdapter(query, con);
            con.Open();
            DataTable dt = new DataTable();
            ad.Fill(dt);
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                listView1.Items.Add(row["iduser"].ToString());
                listView1.Items[i].SubItems.Add(row["username"].ToString());
                listView1.Items[i].SubItems.Add(row["address"].ToString());
                listView1.Items[i].SubItems.Add(row["phone"].ToString());
                i++;
            }
            con.Close();
        }
        private void btLoad_Click(object sender, EventArgs e)
        {
            loadDatabase();
        }

        private void btInsert_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (txtUser.Text == "" && txtAddress.Text == "" && txtPhone.Text == "")
            {
                MessageBox.Show("Please type all the customer Information ","Insert check", MessageBoxButtons.OK, MessageBoxIcon.Information);
       
            }
            if (txtUser.Text.Length > 0 && txtAddress.Text.Length > 0 && txtPhone.Text.Length < 10)
            {
                MessageBox.Show("Please input at least 10 characters from Phone Text box", "Insert check", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            MySqlConnection con = conntectToDatasbe();
            String query = "INSERT INTO user (username,address,phone) values (@username,@address,@phone)";
            MySqlCommand cmd = new MySqlCommand(query,con);
            con.Open();
            cmd.Parameters.AddWithValue("@username", txtUser.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            String phoneData = txtPhone.Text.Replace(" ", "");
            if (phoneData.Length >= 10)
            {
                cmd.Parameters.AddWithValue("@phone", phoneData);
                i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    MessageBox.Show("Insert Successfully", "Insert Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadDatabase();
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Insert Failed. Please check your connection", "Insert Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            String ID = "";
            MySqlConnection con = conntectToDatasbe();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {
                      ID = listView1.Items[i].SubItems[0].Text;
                }
            }
            String query = "delete from user where iduser =" + ID;
            MySqlCommand cmd = new MySqlCommand(query,con);
            con.Open();
            i= cmd.ExecuteNonQuery();
            if (i > 0)
            {
                MessageBox.Show("Delete Successfuflly", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Delete Failed", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            loadDatabase();
            txtUser.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            con.Close();
            
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Do you confirm to close this ?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if(rs == DialogResult.OK)
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            String ID = "";
            MySqlConnection con = conntectToDatasbe();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {
                    ID = listView1.Items[i].SubItems[0].Text;
                }
            }
            String sql = "update user set username = @username, address=@address, phone=@phone where iduser=" + ID;
            MySqlCommand cmd = new MySqlCommand(sql, con);
            con.Open();
            cmd.Parameters.AddWithValue("@username", txtUser.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            String phoneData = txtPhone.Text.Replace(" ", "");
            cmd.Parameters.AddWithValue("@phone", phoneData);
            cmd.ExecuteNonQuery();
            loadDatabase();
            con.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {

                    txtUser.Text = listView1.Items[i].SubItems[1].Text;
                    txtAddress.Text = listView1.Items[i].SubItems[2].Text;
                    String phoneData = listView1.Items[i].SubItems[3].Text;         
                    txtPhone.Text = phoneData.Substring(0, 3) + " " + phoneData.Substring(3, 3) + " " + phoneData.Substring(6);
                    
                }
            }
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            txtUser.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
        }
    }
}