using System;
using System.Collections.Generic;

namespace POWER_SUPPLY_CONTROL_SIMPLE
{
    class ScriptParser
    {
        string name = "undefined";
        double lastVolt = 0.0;
        double lastCurr = 0.0;
        bool isOutput = false;
        bool isRunning = false;  // controlled by this class
        bool isPause = false; // controlled by form1
        bool isStop = false; // controlled by form1
        int length = 0;
        int time = 0;

        System.IO.Ports.SerialPort port = null;

        List<string> commands = new List<string>();

        ~ScriptParser()
        {
            if (port != null)
            {
                byte[] data_o = PortIO.SendSetCode(0.0, 0.0, false);
                port.Write(data_o, 0, data_o.Length);
            }
        }

        public ScriptParser(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                if (line.Trim().StartsWith("#")) continue;
                if (line.Trim().Length == 0) continue;
                if (line.Trim().ToUpper().StartsWith("FUNC"))
                {
                    Name = line.Trim().ToUpper().Split(' ')[1];
                }
                if (line.Trim().ToUpper().StartsWith("WAIT"))
                {
                    try
                    {
                        Time += int.Parse(line.Trim().ToUpper().Split(' ')[1]);
                    }
                    catch (Exception)
                    {
                        throw new InvalidProgramException(line + ", 时间应为整数。");
                    }
                }
                Commands.Add(line.Trim().ToUpper());
                Length++;
            }
            if (Commands[0] != "PSC SCRIPT")
            {
                Length = -1;
                Time = 0;
            }
            else
            {
                Commands.RemoveAt(0);
                Length--;
            }
        }

        public int Step(System.IO.Ports.SerialPort port, string cmd)
        {
            // SET
            if (cmd.StartsWith("SET")) return SetCmd(port, cmd);
            // WAIT
            if (cmd.StartsWith("WAIT")) return Wait(cmd);
            // END FUNC
            if (cmd.StartsWith("END")) return -1;

            return 0;
        }

        // timer 用于计算等待时间
        public void Start(System.IO.Ports.SerialPort port, System.Windows.Forms.Label info)
        {
            // TODO
            IsRunning = true;
            string lastCmd = "";
            foreach (string cmd in Commands)
            {
                if (IsPause)
                {
                    info.Text = "Paused";
                    while (IsPause)
                    {
                        Delay(1000);
                    }
                }

                if (IsStop)
                {
                    IsRunning = false;
                    IsPause = false;
                    return;
                }
                info.Text = "=> " + lastCmd + ", " + cmd;
                lastCmd = cmd;
                try
                {
                    int answer = Step(port, cmd);
                    // Delay for a while
                    if (answer == -1)
                    {
                        return;
                    }
                    Delay(10);
                }
                catch (Exception)
                {
                    throw new InvalidProgramException(cmd + ", 命令解析错误。");
                }
            }

        }

        private int Wait(string cmd)
        {
            string[] cmds = cmd.Split(' ');
            int waitTime = int.Parse(cmds[1]);
            Delay(waitTime * 1000);
            return 0;
        }

        private int SetCmd(System.IO.Ports.SerialPort port, string cmd)
        {
            string[] cmds = cmd.Split(' ');
            switch (cmds[1])
            {
                case "OUTPUT":
                    IsOutput = int.Parse(cmds[2]) == 1;
                    byte[] data_o = PortIO.SendSetCode(LastVolt, LastCurr, IsOutput);
                    port.Write(data_o, 0, data_o.Length);
                    break;
                case "VOLT":
                    LastVolt = double.Parse(cmds[2]);
                    byte[] data_v = PortIO.SendSetCode(LastVolt, LastCurr, IsOutput);
                    port.Write(data_v, 0, data_v.Length);
                    break;
                case "CURR":
                    Console.WriteLine(cmd);
                    LastCurr = double.Parse(cmds[2]);
                    byte[] data_c = PortIO.SendSetCode(LastVolt, LastCurr, IsOutput);
                    port.Write(data_c, 0, data_c.Length);
                    break;
            }
            return 0;
        }


        public static void Delay(int mm)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(mm) > DateTime.Now)
            {
                System.Windows.Forms.Application.DoEvents();
            }
            return;
        }


        public int Time { get => time; set => time = value; }
        public int Length { get => length; set => length = value; }
        public List<string> Commands { get => commands; set => commands = value; }
        public double LastCurr { get => lastCurr; set => lastCurr = value; }
        public double LastVolt { get => lastVolt; set => lastVolt = value; }
        public bool IsOutput { get => isOutput; set => isOutput = value; }
        public string Name { get => name; set => name = value; }
        public bool IsPause { get => isPause; set => isPause = value; }
        public bool IsStop { get => isStop; set => isStop = value; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }
    }
}
