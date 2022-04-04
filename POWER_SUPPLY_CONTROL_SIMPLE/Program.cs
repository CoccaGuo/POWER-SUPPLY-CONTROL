using System;
using System.Windows.Forms;

namespace POWER_SUPPLY_CONTROL_SIMPLE
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            System.Environment.Exit(0);
        }
    }
}
