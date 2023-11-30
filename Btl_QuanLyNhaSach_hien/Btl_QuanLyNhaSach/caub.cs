using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// thiết kế thêm sách môn học dsex dùng combobox

namespace Btl_QuanLyNhaSach
{
    public partial class caub : Form
    {
        ModifyTaiKhoan modify = new ModifyTaiKhoan();
        public caub()
        {
            InitializeComponent();
        }

        private void caub_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
            LoadDataToComboBox();
        }
        private void LoadDataToDataGridView()
        {
            dataGridView1.DataSource = modify.Table("SELECT * FROM sach"); // Thay thế 'YourTableName' bằng tên bảng thực tế trong cơ sở dữ liệu của bạn
            // Đặt tên cho DataGridView của bạn là dataGridView_CaUB (tên có thể khác)
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            int masach;
            if (!int.TryParse(txtMS.Text, out masach))
            {
                MessageBox.Show("Vui lòng nhập số nguyên cho MaSach.", "Lỗi");
                return;
            }

            if (masach <= 0)
            {
                MessageBox.Show("MaSach phải là số nguyên dương.", "Lỗi");
                return;
            }
            string tieuDe = txtTieuDe.Text;
            string moTa = txtMieuTa.Text;
            int selectedMaMon = Convert.ToInt32(comboBox1.SelectedItem);

            string queryCheck = $"SELECT COUNT(*) FROM sach WHERE TieuDe = N'{tieuDe}' AND MaMon = {selectedMaMon}";
            int count = -1;

            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(queryCheck, sqlConnection))
                {
                    count = (int)sqlCommand.ExecuteScalar();
                }
            }

            if (count > 0)
            {
                MessageBox.Show("Tiêu đề sách đã tồn tại cho Mã Môn đã chọn.", "Thông báo");
            }
            else
            {
                string queryInsert = $"INSERT INTO sach(MaSach, MaMon, TieuDe, MoTa) VALUES ({masach}, {selectedMaMon}, N'{tieuDe}', N'{moTa}')";

                using (SqlConnection sqlConnection = Connection.GetSqlConnection())
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(queryInsert, sqlConnection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm sách thành công.", "Thông báo");
                LoadDataToDataGridView();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void LoadDataToComboBox()
        {
            using (SqlConnection connection = Connection.GetSqlConnection())
            {
                connection.Open();

                string query = "SELECT MaMon FROM subject";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Đổ dữ liệu vào ComboBox
                while (reader.Read())
                {
                    int MaMon = reader.GetInt32(0);
                    comboBox1.Items.Add(MaMon);
                }

                connection.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Lấy giá trị từ cột có index tương ứng (ví dụ: cột 0 là MaSach, cột 1 là MaMon, v.v.)
                string maSach = row.Cells["TenCotMaSach"].Value.ToString(); // Thay "TenCotMaSach" bằng tên cột MaSach trong DataGridView của bạn
               // string maMon = row.Cells["TenCotMaMon"].Value.ToString(); // Thay "TenCotMaMon" bằng tên cột MaMon trong DataGridView của bạn
                string tieuDe = row.Cells["TenCotTieuDe"].Value.ToString(); // Thay "TenCotTieuDe" bằng tên cột TieuDe trong DataGridView của bạn
                string moTa = row.Cells["TenCotMoTa"].Value.ToString(); // Thay "TenCotMoTa" bằng tên cột MoTa trong DataGridView của bạn

                // Hiển thị thông tin hoặc thực hiện các thao tác cần thiết với dữ liệu đã chọn
                // Ví dụ:
                txtMS.Text = maSach; // txtMaSach là TextBox để hiển thị MaSach
               // txt.Text = maMon; // txtMaMon là TextBox để hiển thị MaMon
                txtTieuDe.Text = tieuDe; // txtTieuDe là TextBox để hiển thị Tiêu Đề
                txtMieuTa.Text = moTa; // txtMoTa là TextBox để hiển thị Mô Tả
                string maMon = comboBox1.SelectedValue.ToString();
                comboBox1.Text = maMon;
            }*/
        }
    }
}
