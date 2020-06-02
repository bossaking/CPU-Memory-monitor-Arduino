using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sensors_WPF
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Initialize()
        {

            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter memoryCounter = new PerformanceCounter("Memory", "Available MBytes");

            SerialPort serialPort = new SerialPort("COM3", 9600);

            OpenPort(serialPort);

            Sendmessage(serialPort, cpuCounter, memoryCounter);

        }

        private void OpenPort(SerialPort serialPort)
        {
            while (true)
            {
                try
                {
                    serialPort.Open();
                    break;
                }
                catch
                {
                    Thread.Sleep(5000);
                }
            }
        }

        private string GetCpuUsage(PerformanceCounter cpuCounter)
        {

            return $"{cpuCounter.NextValue().ToString("N2")}";

        }

        private string GetMemoryUsage(PerformanceCounter memoryCounter)
        {

            return $"{memoryCounter.NextValue().ToString()}";


        }

        private void Sendmessage(SerialPort serialPort, PerformanceCounter cpuCounter, PerformanceCounter memoryCounter)
        {
            while (true)
            {
                try
                {
                    string output = GetCpuUsage(cpuCounter) + GetMemoryUsage(memoryCounter);
                    serialPort.Write(output);
                    Thread.Sleep(2000);
                }
                catch
                {
                    OpenPort(serialPort);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            NotifyIcon trayIcon = new NotifyIcon()
            {
                Icon = new System.Drawing.Icon("Icons/icon.ico"),
                ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[] {
                new System.Windows.Forms.MenuItem("Exit", Exit)
            }),
                Visible = true
            };

            Thread thread = new Thread(() => Initialize());
            thread.Start();


        }

        private void Exit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
