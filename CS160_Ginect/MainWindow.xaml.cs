using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// For password prompt
using System.Windows.Forms;
using System.Runtime.InteropServices;

// For terminal backend
using System.Diagnostics;
using System.IO;

namespace CS160_Ginect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // For simple testing
            /*
            String output = Terminal.TestModularTerminal();
            MessageBox.Show(output);
            
            List<String> filesList = new List<string>();
            filesList.Add("jessica.txt");
            filesList.Add("jessica2.txt");
            output = Terminal.GitAddFilesToCommit(filesList);
            MessageBox.Show(output);
             * */
            
            
            //String output = Terminal.GetLatestCommitID();
            //String output = Terminal.GitTagLatestCommit("lastCommit");
            //String output = Terminal.GitPush();

            int output = Terminal.TestModularTerminal();
            System.Windows.MessageBox.Show(output.ToString());

            SendKeyTestCmdExe();
        }

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        [DllImport("USER32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        // Send a series of key presses to the Calculator application.
        private void SendKeyTest()
        {
            // Get a handle to the Calculator application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr calculatorHandle = FindWindow("CalcFrame", "Calculator");

            // Verify that Calculator is a running process.
            if (calculatorHandle == IntPtr.Zero)
            {
                System.Windows.MessageBox.Show("Calculator is not running.");
                return;
            }

            // Make Calculator the foreground application and send it 
            // a set of calculations.
            SetForegroundWindow(calculatorHandle);
            SendKeys.SendWait("111");
            SendKeys.SendWait("*");
            SendKeys.SendWait("11");
            SendKeys.SendWait("=");
        }

        internal void SendKeyTestCmdExe()
        {
            IntPtr windowHandle = GetForegroundWindow();
            IntPtr childHandle;

            //try to get a handle to IE's toolbar container
            childHandle = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");

            if (childHandle == IntPtr.Zero)
            {
                System.Windows.MessageBox.Show("cmd.exe is not running.");
            }
            SetForegroundWindow(childHandle);
            SendKeys.SendWait("chewie#3{ENTER}");

            /*
            // Get a handle to the Calculator application. The window class
            // and window name were obtained using the Spy++ tool.
            IntPtr h = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");

            // Verify that Calculator is a running process.
            if (h == IntPtr.Zero)
            {
                System.Windows.MessageBox.Show("cmd.exe is not running.");
                return;
            }

            // Make Calculator the foreground application and send it 
            // a set of calculations.
            SetForegroundWindow(h);
             * */
            
        }
    }


}
