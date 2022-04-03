using System;
using System.Windows.Forms;

namespace POWER_SUPPLY_CONTROL_SIMPLE
{
    public partial class Form1 : Form
    {   
        ScriptParser parser;
        int countdownTimer;
        int unstableExceptionCounter = 0;

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
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
            } catch (Exception) {
                unstableExceptionCounter++;
                if(unstableExceptionCounter >= 5) 
                {   
                    unstableExceptionCounter = 0;
                    infoLabel.Text = "数据接受可能不稳定。"; 
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
            openFileDialog1.InitialDirectory = "./";
            openFileDialog1.Filter = "PSC脚本文件|*.txt|所有文件|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                string script_name = System.IO.Path.GetFileNameWithoutExtension(fName);
                infoLabel.Text = "loading script: " + script_name;
                this.Text = "PSC: "+ script_name;
                try
                {
                    parser = new ScriptParser(fName);
                }catch (InvalidProgramException exc)
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
                    infoLabel.Text = "FUNC: "+parser.Name;
                    countdownTimer = parser.Time/60;
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
            timer2.Enabled = true;
            openBtn.Enabled = false;
            portBox.Enabled = false;
            portBtn.Enabled = false;
            startBtn.Enabled = false;
            infoLabel.Text = "Exec: " + parser.Name;
            try
            {
                parser.Start(serialPort1, infoLabel);
            } catch (InvalidProgramException ex)
            {
                infoLabel.Text = "Error at:" + ex.Message;
            }
            infoLabel.Text = parser.Name + " Complete!";
            timer2.Enabled = false;
            openBtn.Enabled = true;
            portBox.Enabled = true;
            portBtn.Enabled =true;
            startBtn.Enabled = true;
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
            System.Environment.Exit(0);
        }
    }
}
