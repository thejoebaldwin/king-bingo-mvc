using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace TestConnectionString
{
    public partial class frmTestConnectionString : Form
    {
        public frmTestConnectionString()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(txtConnectionString.Text.Trim()))
                {

                    SqlCommand command = new SqlCommand(txtQuery.Text.Trim());
                    command.Connection = conn;
                    DataTable results = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    conn.Open();
                    adapter.Fill(results);
                    lblResult.Text ="Rows Returned:" + results.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
            }
            finally
            {

            }
        }
    }
}
