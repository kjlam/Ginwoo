using System;
using System.Diagnostics;
using System.IO;

public class Terminal
{
    public Terminal()
    {
    }

    static internal String testTerminal()
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        //process.StartInfo.WorkingDirectory = @"c:\";
        //startInfo.Arguments = "/C cat buggy.txt";
        startInfo.Arguments = "/C echo Hello World";
        process.StartInfo = startInfo;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        //string error = process.StandardError.ReadLine();
        //StreamReader myStreamReader = process.StandardOutput;
        //string output = myStreamReader.ReadLine();
        //string output = process.StandardOutput.ReadLine();
        process.WaitForExit();
        process.Close();

        Debug.WriteLine(output);
        //Console.ReadLine();
        return output;
        
        //return error;
    }

}
