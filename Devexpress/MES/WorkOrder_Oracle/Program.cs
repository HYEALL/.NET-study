using MES.Forms;
using MES.ProductionOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProductionOrder
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                AppSession.UserId = "test_user";
                //Application.Run(new FrmProductionOrder());
                Application.Run(new FrmWorkOrder());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message + "\n\n" + ex.StackTrace,
                    "시작 오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }
    }
}
