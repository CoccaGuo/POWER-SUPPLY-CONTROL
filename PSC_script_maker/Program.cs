using System;


namespace PSC_script_maker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("适用于Power Supply Control的脚本生成器。Wanptek WPS3010H(30V10A)+USB串口通讯。");
            Console.WriteLine("Author CoccaGuo, Script Ver 0.1, 2022.3.31");
            Console.WriteLine("---输入函数名：");
            string funcName = ReadLine().Replace(' ', '_');
            Console.WriteLine("---输入开始电压：");
            double beginVolt = double.Parse(ReadLine());
            Console.WriteLine("---输入结束电压：");
            double endVolt = double.Parse(ReadLine());
            Console.WriteLine("---输入电压每次增加步长：");
            double stepVolt = double.Parse(ReadLine());

            Console.WriteLine("---输入开始电流：");
            double beginCurr = double.Parse(ReadLine());
            Console.WriteLine("---输入结束电流：");
            double endCurr = double.Parse(ReadLine());
            Console.WriteLine("---输入电流每次增加步长：");
            double stepCurr = double.Parse(ReadLine());

            bool IMain = true;
            if (stepCurr != 0 && stepVolt != 0)
            {
                Console.WriteLine("---好的。总步长以电压为准还是电流为准？（V/I）");
                if (ReadLine().StartsWith("V")) IMain = false;

            }
            else
            {
                if (stepCurr == 0) IMain = false;
            }
            int steps;
            if (IMain)
            {
                steps = (int)((endCurr - beginCurr) / stepCurr);
            }
            else
            {
                steps = (int)((endVolt - beginVolt) / stepVolt);
            }

            Console.WriteLine("---总共要运行大约" + steps + "步。");

            Console.WriteLine("---输入每步之间等待时间（整数，秒）：");
            int stepTime = int.Parse(ReadLine());

            Console.WriteLine("---好的。运行时间约" + (stepTime * steps) + "秒，约等于" + (stepTime * steps / 60) + "分钟。");
            Console.WriteLine("---是否等待时间越来越快/慢？（Y/N）");
            int ssTime = 0;
            if (ReadLine().StartsWith("Y"))
            {
                Console.WriteLine("---数学提示：" + stepTime + "/" + steps + "=" + (stepTime / steps));
                Console.WriteLine("---目前每步之间要等待" + stepTime + "秒，在每进行一步之后，等待时间要减少多少秒（整数）：");
                ssTime = int.Parse(ReadLine());
            }
            Console.WriteLine("---好的。运行时间约" + ((stepTime + (stepTime - steps * ssTime)) * steps / 2) + "秒，约等于" + ((stepTime + (stepTime - steps * ssTime)) * steps / 120) + "分钟。");
            Console.WriteLine("---正在生成，请稍等...");
            System.IO.StreamWriter sw = new System.IO.StreamWriter("./PSC_" + funcName + ".txt");
            sw.WriteLine("# Automatic power supply script generated on " + DateTime.Now.ToString() + ". Check before use.");
            sw.WriteLine("PSC SCRIPT");
            sw.WriteLine();
            sw.WriteLine("func " + funcName);
            sw.WriteLine("# Functional test and preparation. DO NOT CHANGE.");
            sw.WriteLine("set volt 1");
            sw.WriteLine("set curr 0.05");
            sw.WriteLine("set output 1");
            sw.WriteLine("wait 5");
            sw.WriteLine("set output 1");
            sw.WriteLine("wait 5");
            sw.WriteLine("# test over.");
            sw.WriteLine();

            sw.WriteLine("# loop begin.");
            sw.WriteLine();
            for (int i = 0; i < steps; i++)
            {
                double nowVolt = (beginVolt + i * stepVolt);
                double nowCurr = (beginCurr + i * stepCurr);
                sw.WriteLine("#loop #" + i);
                if (stepVolt > 0 && nowVolt > endVolt) nowVolt = endVolt;
                if (stepVolt < 0 && nowVolt < endVolt) nowVolt = endVolt;
                if (stepCurr > 0 && nowCurr > endCurr) nowCurr = endCurr;
                if (stepCurr < 0 && nowCurr < endCurr) nowCurr = endCurr;

                sw.WriteLine("set volt " + nowVolt.ToString("0.000"));
                sw.WriteLine("set curr " + nowCurr.ToString("0.000"));
                sw.WriteLine("wait " + (stepTime - i * ssTime));
                sw.WriteLine();
            }

            sw.WriteLine("set volt " + endVolt);
            sw.WriteLine("set curr " + endCurr);
            sw.WriteLine("wait " + (stepTime - steps * ssTime));
            sw.WriteLine();
            sw.WriteLine("# End loop. Stopping power supply.");
            sw.WriteLine();
            sw.WriteLine("set output 0");
            sw.WriteLine("wait 5");
            sw.WriteLine("set volt 0");
            sw.WriteLine("set curr 0");
            sw.WriteLine("set output 0");
            sw.WriteLine("end");

            sw.WriteLine();
            sw.WriteLine("# End of the script.");
            sw.WriteLine("# Powered by Coccaguo. Version 0.1");

            sw.Flush();
            sw.Close();

            Console.WriteLine("---生成完成。按任意键退出。");
            Console.ReadKey();

        }

        public static string ReadLine()
        {
            string line;
            try
            {
                line = Console.ReadLine();
                if (line != null)
                {
                    return line;
                }
                else
                {
                    return " ";
                }
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
