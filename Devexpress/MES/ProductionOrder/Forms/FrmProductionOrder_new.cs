using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using MES.DAL;
using MES.Models;
using MES.ProductionOrder;

namespace ProductionOrder.Forms
{
    public partial class FrmProductionOrder_new : XtraForm
    {
        public FrmProductionOrder_new()
        {
            InitializeComponent();
        }
    }
}
