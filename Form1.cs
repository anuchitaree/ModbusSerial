using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyModbus;
using System.IO.Ports;
using System.Management;

namespace ModbusSerial
{
    public partial class Form1 : Form
    {
        //ModbusClient modbusClient =new ModbusClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Replace with your actual serial port name and Modbus slave address
            string serialPortName = comboBoxComPort.Text; // "COM6";
            int slaveAddress = 1;

            // Create a Modbus RTU client
            ModbusClient modbusClient = new ModbusClient(serialPortName);
            modbusClient.UnitIdentifier = (byte)slaveAddress;
            modbusClient.Baudrate = int.Parse(comboBaudrate.Text); // 9600;
            modbusClient.Parity = System.IO.Ports.Parity.None;
            modbusClient.StopBits = System.IO.Ports.StopBits.One;
            modbusClient.ConnectionTimeout = 1000;

            try
            {
                // Connect to the Modbus server
                modbusClient.Connect();

                // Now you can perform Modbus operations
                // For example, read holding registers
                int startAddress = 24;
                int length = 16;
                int[] values = modbusClient.ReadHoldingRegisters(startAddress, length);

                // Print the values of the holding registers
                Console.WriteLine("Holding Registers: " + string.Join(", ", values));

                // Display the values of the holding registers
                //Console.WriteLine($"Holding Registers (Address {startAddress} to {startAddress + length - 1}):");
                for (int i = 0; i < length; i += 2)
                {
                    long val = values[i + 1];
                    Console.WriteLine(val << 16);

                    Console.WriteLine($"Register {startAddress + i}: {values[i]} {values[i + 1]} {Convert.ToString(val << 16, 2)}");
                }
                label3.Text = "Counter 1 : " + values[1].ToString();
                label4.Text = "Counter 2 : " + values[3].ToString();
                label5.Text = "Counter 3 : " + values[5].ToString();
                label6.Text = "Counter 4 : " + values[7].ToString();
                label7.Text = "Counter 5 : " + values[9].ToString();
                label8.Text = "Counter 6 : " + values[11].ToString();
                label9.Text = "Counter 7 : " + values[13].ToString();
                label10.Text = "Counter 8 : " + values[15].ToString();

            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during Modbus operations
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Close the connection when done
                modbusClient.Disconnect();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            List<String> tList = new List<String>();

            comboBoxComPort.Items.Clear();

            foreach (string s in SerialPort.GetPortNames())
            {
                tList.Add(s);
            }

            tList.Sort();
            comboBoxComPort.Items.Add("Select COM port...");
            comboBoxComPort.Items.AddRange(tList.ToArray());
            comboBoxComPort.SelectedIndex = 0;
        }
    }
}
