using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;

namespace DataGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OrderInfoCollection orderInfoCollection = new OrderInfoCollection();
            sfDataGrid1.DataSource = orderInfoCollection.Orders;
        }
    }
}
