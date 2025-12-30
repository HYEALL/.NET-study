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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DataGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            OrderInfoCollection orderInfoCollection = new OrderInfoCollection();
            sfDataGrid1.DataSource = orderInfoCollection.Orders;
            orderInfoCollection.Orders.CollectionChanged += Orders_CollectionChanged;
        }

        private void Orders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Add 처리
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (OrderInfo newItem in e.NewItems)
                {
                    Console.WriteLine($"새로운 주문 발생: Order ID: {newItem.OrderID}, Customer ID: {newItem.CustomerID}, Customer Name: {newItem.CustomerName}, Country: {newItem.Country}, ShipCity: {newItem.ShipCity}");
                }
            }

            // Remove/Replace 처리
        }
    }
}
