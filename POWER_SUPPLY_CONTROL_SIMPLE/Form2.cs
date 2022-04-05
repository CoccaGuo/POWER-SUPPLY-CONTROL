using System;
using System.Windows.Forms;

namespace POWER_SUPPLY_CONTROL_SIMPLE
{
    public partial class Form2 : Form
    {
        public UDPServer server;

        public Form2(UDPServer server)
        {
            InitializeComponent();
            this.server = server;

        }

        public void chartStep(double volt, double curr)
        {
            this.chart1.Series[0].Points.AddXY(DateTime.Now.ToShortTimeString().ToString(), curr);
            this.chart1.Series[1].Points.AddXY(DateTime.Now.ToShortTimeString().ToString(), volt);
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            if (udpPort.Text != server.port)
            {
                server.SetupServer(udpPort.Text);
            }
        }


    }
}
