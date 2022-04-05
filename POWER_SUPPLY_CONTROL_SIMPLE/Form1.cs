using System;
using System.Linq;
using System.Windows.Forms;

namespace POWER_SUPPLY_CONTROL_SIMPLE
{
    public partial class Form1 : Form
    {
        ScriptParser parser;
        int countdownTimer;
        int unstableExceptionCounter = 0;
        bool isForm2Applied = false;
        Form2 form2;
        UDPServer server;

        public Form1()
        {
            InitializeComponent();
            portBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            CheckForIllegalCrossThreadCalls = false;
            server = new UDPServer();
            form2 = new Form2(server);
            server.SetupServer(form2.udpPort.Text);
            server.StartListening();
            server.UDPMessageReceived += UdpClient_UDPMessageReceived;
        }

        private void UdpClient_UDPMessageReceived(UdpStateEventArgs args)
        {
            if (form2 != null && form2.AllowCtrlCheckBox.Checked)
            {
               byte[] data = args.buffer;
                if (data == null || data?[0] != 0xE2 || data?[1] != 0x10) return;
                // 0x10 this PSC addr.
                bool outputLabel = false;
                if ((data?[3] & 0x10) != 0x10) return ;
                if ((data?[3] & 0x10) == 0x01) outputLabel = true ;
                byte[] sendData = PortIO.SendSetCode(data.Skip(4).Take(2).ToArray(), data.Skip(6).Take(2).ToArray(), outputLabel);
                
               serialPort1.Write(sendData, 0, sendData.Length);
            }
        }

        private void portBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    //串口已经处于打开状态
                    serialPort1.Close();    //关闭串口
                    portBtn.Text = "打开";
                    portBox.Enabled = true;
                    timer1.Enabled = false;
                    startBtn.Enabled = false;
                    infoLabel.Text = "串口已关闭。请打开串口。";
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    portBox.Enabled = false;
                    serialPort1.PortName = portBox.Text;
                    serialPort1.BaudRate = 19200;
                    serialPort1.DataBits = 8;

                    serialPort1.Parity = System.IO.Ports.Parity.None;

                    serialPort1.StopBits = System.IO.Ports.StopBits.One;


                    serialPort1.Open();     //打开串口
                    timer1.Enabled = true;
                    infoLabel.Text = "端口已打开。";
                    portBtn.Text = "关闭";
                    if (parser != null && parser.Length > 0)
                    {
                        startBtn.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                portBox.Items.Clear();
                portBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                portBtn.Text = "打开";
                infoLabel.Text = "端口状态异常。";
                MessageBox.Show(ex.Message);
                portBox.Enabled = true;
                timer1.Enabled = false;
                startBtn.Enabled = false;

            }

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("data recv.");
            if (isForm2Applied)
            {
                server.SetupServer(form2.udpPort.Text);
                isForm2Applied = false;
            }
            ScriptParser.Delay(30);
            byte[] received_buf = new byte[20];
            serialPort1.Read(received_buf, 0, 20);
            try
            {
                double[] result = PortIO.ParseRecvCode(received_buf);
                VLabel.Text = string.Format("{0:F2}", result[0]);
                ALabel.Text = string.Format("{0:F3}", result[1]);
                spVolt.Text = string.Format("{0:F2}", result[2]) + " V";
                spCurr.Text = string.Format("{0:F3}", result[3]) + " A";
                form2.chartStep(result[0], result[1]);
                byte[] result_buf = new byte[8];

                if (form2.UDPCheckbox.Checked)
                {
                    server.udpClient.Send(PortIO.UDPSendCode(received_buf), 8, server.locatePoint);
                }
            }
            catch (Exception)
            {
                unstableExceptionCounter++;
                if (unstableExceptionCounter >= 5)
                {
                    unstableExceptionCounter = 0;
                    infoLabel.Text = "数据接可能不稳定。";
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                byte[] codes = PortIO.SendReadCode();
                int length = codes.Length;
                serialPort1.Write(codes, 0, length);
                Console.WriteLine("query cmd sent.");
            }
            catch (Exception)
            {
                infoLabel.Text = "写入端口失败。";
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                portBox.Items.Clear();
                portBox.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                portBtn.Text = "打开";
                portBox.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            if (parser != null && parser.IsRunning)
            {
                parser.IsStop = true;
                infoLabel.Text += ", Stop(Plan)";
                startBtn.Enabled = false;
                openBtn.Enabled = false;
                return;
            }
            openFileDialog1.InitialDirectory = "./";
            openFileDialog1.Filter = "PSC脚本文件|*.txt|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                string script_name = System.IO.Path.GetFileNameWithoutExtension(fName);
                infoLabel.Text = "loading script: " + script_name;
                this.Text = "PSC: " + script_name;
                try
                {
                    parser = new ScriptParser(fName);
                }
                catch (InvalidProgramException exc)
                {
                    infoLabel.Text = "脚本错误 => " + exc.Message;
                    return;
                }
                if (parser.Length == -1)
                {
                    infoLabel.Text = "未找到标识符\"PSC SCRIPT\"";
                    startBtn.Enabled = false;
                }
                if (parser.Length > 0)
                {
                    infoLabel.Text = script_name + "已经打开。";
                    Text = "PSC: " + parser.Name;
                    infoLabel.Text = "FUNC: " + parser.Name;
                    countdownTimer = parser.Time / 60;
                    if (countdownTimer == 0)
                        timeLabel.Text = "< 1 min";
                    else
                        timeLabel.Text = countdownTimer + " min";
                    if (serialPort1.IsOpen)
                    {
                        startBtn.Enabled = true;
                    }
                }
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (parser != null && parser.IsRunning && !parser.IsPause)
            {
                parser.IsPause = true;
                infoLabel.Text += ", Pause(Plan)";
                openBtn.Enabled = false;
                startBtn.Text = "继续";
                return;

            }
            if (parser != null && parser.IsRunning && parser.IsPause)
            {
                parser.IsPause = false;
                openBtn.Enabled = true;
                startBtn.Text = "暂停";
                return;
            }
            timer2.Enabled = true;
            label4.Text = "结束程序";
            openBtn.Text = "停止";
            portBox.Enabled = false;
            portBtn.Enabled = false;
            startBtn.Text = "暂停";
            infoLabel.Text = "Exec: " + parser.Name;
            try
            {
                parser.Start(serialPort1, infoLabel);
                parser = null;
                infoLabel.Text = "Complete!";
            }
            catch (InvalidProgramException ex)
            {
                infoLabel.Text = "Error at:" + ex.Message;
            }
            catch (NullReferenceException nptr)
            {
                infoLabel.Text = "Cmd parser force deleted.";
            }
            finally
            {
                openBtn.Enabled = true;
                timer2.Enabled = false;
                openBtn.Text = "打开";
                label4.Text = "打开脚本";
                portBox.Enabled = true;
                portBtn.Enabled = true;
                startBtn.Text = "开始";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            countdownTimer--;
            if (countdownTimer <= 0)
                timeLabel.Text = "< 1 min";
            else
                timeLabel.Text = countdownTimer + " min";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                byte[] data_o = PortIO.SendSetCode(0.0, 0.0, false);
                serialPort1.Write(data_o, 0, data_o.Length);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                System.Environment.Exit(0);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            form2.Show();
        }
    }
}
