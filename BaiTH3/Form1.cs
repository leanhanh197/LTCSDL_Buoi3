using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace BaiTH3
{
    public partial class Form1 : Form
    {
        private SqlConnection cn = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string cnStr = "Server = .; Database = QLBanHang; Integrated security = true;";
            cn = new SqlConnection(cnStr);

            dGVLoaiSanPham.DataSource = GetData();
        }
        private void Connect()
        {
            try
            {
                if (cn != null && cn.State != ConnectionState.Open)
                {
                    cn.Open();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Cannot open a connection without specifying a data source or server \n\n" + ex.Message);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("A connection-level error occurred while opening the connection \n\n" + ex.Message);
            }
            catch (ConfigurationException ex)
            {
                MessageBox.Show("There are two entries with the same name in the <localdbinstances> section \n\n" + ex.Message);
            }

        }

        private void DisConnect()
        {
            if (cn != null && cn.State != ConnectionState.Closed)
            {
                cn.Close();
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            string strsql = "DELETE FROM LoaiSP WHERE MaLoaiSP = " + txtMaLoai.Text ;
            SqlCommand cmd = new SqlCommand(strsql, cn);
            Connect();
            try
            {
                int SS = cmd.ExecuteNonQuery();
                dGVLoaiSanPham.DataSource = GetData();
                MessageBox.Show("Đã xóa xong");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi nhập loại sản phẩm không tồn tại\n" + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Kết nối đã đóng \n" + ex.Message);
            }
            finally
            {                
                DisConnect();
            }
        }//end Xoa_Click
        private List<object> GetData()
        {
            Connect();

            string sql = "SELECT * FROM LoaiSP";

            List<object> list = new List<object>();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                SqlDataReader dr = cmd.ExecuteReader();

                string tenLoai;
                int maLoai;
                while (dr.Read())
                {
                    maLoai = dr.GetInt32(0);
                    tenLoai = dr.GetString(1);
                    var prod = new
                    {
                        MaLoai = maLoai,
                        TenLoai =tenLoai
                    };
                    list.Add(prod);
                }
                dr.Close();
            }
            catch (SqlException ex) // Internet => Exception
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DisConnect();
            }
            return list;
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            Connect();
            string sql = "INSERT INTO LoaiSP values(" + txtMaLoai.Text + ", N'" + txtTenLoai.Text + "')";
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.ExecuteNonQuery();
                dGVLoaiSanPham.DataSource = GetData();
                MessageBox.Show("Đã thêm xong");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi nhập loại sản phẩm\n" + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Kết nối đã đóng \n" + ex.Message);
            }
            finally
            {
                DisConnect();
            }
        }
    }
}
