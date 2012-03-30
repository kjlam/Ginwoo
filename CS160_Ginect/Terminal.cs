using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

public class Terminal
{
    public Terminal()
    {
    }

    static internal void GitAddFilesToCommit(List<String> filesList)
    {
        String filesStr = "";

        filesList.ForEach(delegate(String file)
        {
            filesStr += " ";
            filesStr += file;
        });

        Process process = InitializeCmdTerminal(@"C:\Users\Jessica", "git add" + filesStr);
        String stdout = ReadStdOut(ref process);

        // TODO: check stdout for success or failure
    }

    static internal void GitTag(String tagName, String commitID)
    {
        Process process = InitializeCmdTerminal(@"C:\Users\Jessica", "git tag " + tagName + " " + commitID);
        String stdout = ReadStdOut(ref process);

        // TODO: check stdout for success or failure
    }

    static internal void GitPush()
    {
        Process process = InitializeCmdTerminal(@"C:\Users\Jessica", "git push");
        String stdout = ReadStdOut(ref process);

        // TODO: check stdout for success or failure
    }

    static private Process InitializeCmdTerminal(String directory, String command) 
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

        // Make it so the terminal isn't displayed on the screen when it's executing commands
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        // The cmd terminal
        startInfo.FileName = "cmd.exe";
        
        Directory.SetCurrentDirectory(directory);

        startInfo.Arguments = "/C " + command;
        process.StartInfo = startInfo;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        return process;
    }

    static private String ReadStdOut(ref Process process)
    {
        process.Start();

        string stdout = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        process.Close();

        return stdout;
    }

    // Just preliminary code
    static internal String TestModularTerminal()
    {
        Process process = InitializeCmdTerminal(@"C:\Users\Jessica\Downloads", "dir");
        return ReadStdOut(ref process);
    }

    // Just preliminary code
    static internal String testTerminal()
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        //process.StartInfo.WorkingDirectory = @"c:\";

        //Set the current directory.
        string dir = @"C:\Users\Jessica\Documents";
		Directory.SetCurrentDirectory(dir);

        startInfo.Arguments = "/C dir";
        //startInfo.Arguments = "/C echo Hello World";
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

        Debug.WriteLine("Current directory: {0}", Directory.GetCurrentDirectory());
        Debug.WriteLine(output);
        //Console.ReadLine();
        return output;
        
        //return error;
    }

}
