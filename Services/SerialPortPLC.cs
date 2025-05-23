using SerialPortFX.Profinet;
using SerialPortFX;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Services
{
    public class SerialPortPLC
    {
        MelsecFxSerial serialPort;
        string portName;
        public SerialPortPLC()
        {
            serialPort = new MelsecFxSerial();
            using (StreamReader reader = new StreamReader("config.ini"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("Port="))
                    {
                        portName = line.Substring("Port=".Length);
                    }
                }
            }
            serialPort = new MelsecFxSerial();
            int baudRate = 38400; // Tốc độ truyền
            Parity parity = Parity.Even;
            int dataBits = 7;
            StopBits stopBits = StopBits.One;
            serialPort.SerialPortInni(portName, baudRate, dataBits, stopBits, parity);
        }
        public void ConnectPLC()
        {
            try
            {
                serialPort.Open();
            }
            catch
            {
            }
        }
        public bool CheckConnectPLC()
        {
            return serialPort.IsOpen();
        }
        public string PortName()
        {
            return serialPort.PortName;
        }
        public void DisconnectPLC()
        {
            try
            {
                if (serialPort.IsOpen())
                {
                    serialPort.Close();
                    serialPort.Dispose();
                }
            }
            catch { }

        }
        bool convertResult;
        double convertResultDB;
        public bool ReadDataFromPLC(string bitget)
        {

            if (serialPort.IsOpen())
            {
                OperateResult<bool> result = serialPort.ReadBool(bitget);
                if (result.IsSuccess)
                {
                    convertResult = result.Content;
                }
            }
            return convertResult;
        }
        public double ReadDataFromPLC2(string bitget)
        {
            OperateResult<Int32> result = serialPort.ReadInt32(bitget);
            if (result.IsSuccess)
            {
                convertResultDB = result.Content;
            }
            return convertResultDB;
        }
        public void WriteDataToPLC2(string bitget, Int32 valueDB)
        {
            serialPort.Write(bitget, valueDB);
        }

        public void WriteDataToPLC(string bitset, bool valueset)
        {
            serialPort.Write(bitset, valueset);
        }
    }
}
