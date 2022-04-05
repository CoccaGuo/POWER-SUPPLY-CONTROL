using System;
using System.Linq;

namespace POWER_SUPPLY_CONTROL_SIMPLE
{
    internal class PortIO
    {

        public static byte[] SendReadCode()
        {
            byte[] data = new byte[8];
            data[0] = 0x00;
            data[1] = 0x03;
            data[2] = 0x00;
            data[3] = 0x00;
            data[4] = 0x00;
            data[5] = 0x0F;
            data[6] = 0x04;
            data[7] = 0x1F;
            return data;
        }

        public static byte[] UDPSendCode(byte[] recv)
        {
            byte[] data = new byte[8];
            byte[] volt_part_real = recv.Skip(6).Take(2).ToArray();
            byte[] curr_part_real = recv.Skip(8).Take(2).ToArray();
            data[0] = 0xE2; //check
            data[1] = 0x00; //to main
            data[2] = 0x10; // power supply addr
            data[3] = 0x00;
            data[4] = volt_part_real[0];
            data[5] = volt_part_real[1];
            data[6] = curr_part_real[0];
            data[7] = curr_part_real[1];
            return data;
        }

        public static double[] ParseRecvCode(byte[] recv)
        {
            try
            {
                byte[] volt_part_real = recv.Skip(6).Take(2).ToArray();
                byte[] curr_part_real = recv.Skip(8).Take(2).ToArray();
                byte[] volt_part_set = recv.Skip(10).Take(2).ToArray();
                byte[] curr_part_set = recv.Skip(12).Take(2).ToArray();
                byte[] info_code = recv.Take(18).ToArray();
                byte[] checked_code = ToModbus(info_code);
                byte[] given_check_code = recv.Skip(18).Take(2).ToArray();
                if (checked_code[0] != given_check_code[0] || checked_code[1] != given_check_code[1]) throw new FormatException("数据传输不稳定");
                double voltReal = BitConverter.ToInt16(volt_part_real, 0) / 100.0;
                double currReal = BitConverter.ToInt16(curr_part_real, 0) / 1000.0;
                double voltSet = BitConverter.ToInt16(volt_part_set, 0) / 100.0;
                double currSet = BitConverter.ToInt16(curr_part_set, 0) / 1000.0;
                double[] data = { voltReal, currReal, voltSet, currSet };
                return data;
            }
            catch (Exception)
            {
                return new double[] { 0, 0, 0, 0 };
            }
        }
        public static byte[] SendSetCode(byte[] b_v, byte[] b_c, bool isOutput)
        {
            byte[] data = new byte[13];
            data[0] = 0x00;
            data[1] = 0x10;
            data[2] = 0x00;
            data[3] = 0x00;
            data[4] = 0x00;
            data[5] = 0x05;
            if (isOutput) data[6] = 0x01;
            else data[6] = 0x00;
            data[7] = b_v[0];
            data[8] = b_v[1];
            data[9] = b_c[0];
            data[10] = b_c[1];
            byte[] crc = ToModbus(data.Take(11).ToArray());
            data[11] = crc[0];
            data[12] = crc[1];
            return data;
        }

        public static byte[] SendSetCode(double volt, double curr, bool isOutput)
        {
            byte[] data = new byte[13];
            data[0] = 0x00;
            data[1] = 0x10;
            data[2] = 0x00;
            data[3] = 0x00;
            data[4] = 0x00;
            data[5] = 0x05;
            if (isOutput) data[6] = 0x01;
            else data[6] = 0x00;
            int volt_set = (int)(volt * 100);
            int curr_set = (int)(curr * 1000);
            byte[] b_v = BitConverter.GetBytes(volt_set);
            byte[] b_c = BitConverter.GetBytes(curr_set);
            data[7] = b_v[0];
            data[8] = b_v[1];
            data[9] = b_c[0];
            data[10] = b_c[1];
            byte[] crc = ToModbus(data.Take(11).ToArray());
            data[11] = crc[0];
            data[12] = crc[1];
            return data;
        }

        /// <summary>
        /// CRC16_Modbus效验
        /// </summary>
        /// <param name="byteData">要进行计算的字节数组</param>
        /// <returns>计算后的数组</returns>
        public static byte[] ToModbus(byte[] byteData)
        {
            byte[] CRC = new byte[2];

            UInt16 wCrc = 0xFFFF;
            for (int i = 0; i < byteData.Length; i++)
            {
                wCrc ^= Convert.ToUInt16(byteData[i]);
                for (int j = 0; j < 8; j++)
                {
                    if ((wCrc & 0x0001) == 1)
                    {
                        wCrc >>= 1;
                        wCrc ^= 0xA001;//异或多项式
                    }
                    else
                    {
                        wCrc >>= 1;
                    }
                }
            }

            CRC[1] = (byte)((wCrc & 0xFF00) >> 8);//高位在后
            CRC[0] = (byte)(wCrc & 0x00FF);       //低位在前
            return CRC;

        }

    }

}
