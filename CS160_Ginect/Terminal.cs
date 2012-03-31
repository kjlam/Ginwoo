﻿using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// For password prompt
using System.Windows.Forms;
using System.Runtime.InteropServices;

public class Terminal
{
    static internal String workingDirectory = @"C:\Users\Jessica\Ginect";
    static internal String password = "password";

    public Terminal()
    {
    }

    /*
     * GitAddFilesToCommit()
     * 
     * This executes a 'git add <file1> <file2> .. <fileN>' command.
     * 
     */ 
    static internal String GitAddFilesToCommit(List<String> filesList)
    {
        String filesStr = "";

        filesList.ForEach(delegate(String file)
        {
            filesStr += " ";
            filesStr += file;
        });

        String stdout = ExecuteCommand(workingDirectory, "git add" + filesStr);

        // TODO: check stdout for success or failure

        return ParseStdOut(stdout);
    }


    static internal String GitTagLatestCommit(String tagName)
    {
        String latestCommitID = GetLatestCommitID();
        return GitTag(tagName, latestCommitID);
    }

    /*
     * GetLatestCommitID()
     * 
     * This returns the latest commit ID of the local commits
     * made by the registered user. These commits have not been
     * pushed to the remote master yet.
     * 
     * Returns a string of the latest commit ID.
     * 
     */ 
    static private String GetLatestCommitID()
    {
        char[] delimiterChars = {' '};
        String latestCommit = null;

        String unpushedCommits = ExecuteCommand(workingDirectory, "git log origin/master..HEAD");
        unpushedCommits = ParseStdOut(unpushedCommits);

        using (StringReader reader = new StringReader(unpushedCommits))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, "^commit .+$"))
                {
                    latestCommit = line;
                    break;
                }
            }
        }

        String[] splitLatestCommit = latestCommit.Split(delimiterChars);
        return splitLatestCommit[1];
    }

    /*
     * GitTag()
     * 
     * This executes a 'git tag -f <tagname> <commitID>' command.
     * A 'git commit' must be made before this method.
     * 
     */ 
    static private String GitTag(String tagName, String commitID)
    {
        String stdout = ExecuteCommand(workingDirectory, "git tag -f " + tagName + " " + commitID);

        // TODO: check stdout for success or failure

        return ParseStdOut(stdout);
    }

    /*
     * GitCommitWithMessage()
     * 
     * This executes a 'git commit -m "My message"' command.
     * 
     */ 
    static internal String GitCommitWithMessage(String message)
    {
        String stdout = ExecuteCommand(workingDirectory, "git commit -m \"" + message + "\"");

        // TODO: check stdout for success or failure

        return ParseStdOut(stdout);
    }

    /*
     * GitPush()
     * 
     * This executes a 'git push' command. A 'git commit' must be made
     * before calling this method.
     * 
     */ 
    static internal String GitPush()
    {
        String stdout = ExecuteCommand(workingDirectory, "git push --tags");

        // TODO: check stdout for success or failure

        return ParseStdOut(stdout);
    }

    // TODO: Complete this method. Return a list of files that have been modified
    // since the last commit
    static internal List<String> GitStatus()
    {
        List<String> modifiedFiles = new List<String>();
        String stdout = ExecuteCommand(workingDirectory, "git status");

        // parse stdout and add files to modifiedFiles

        return modifiedFiles;
    }

    /*
     * ExecuteCommand()
     * 
     * This executes a command in CMD. The CMD terminal is not displayed.
     * Returns the standard output of the command.
     * 
     */ 
    static private String ExecuteCommand(String directory, String command) 
    {
        Process process = new System.Diagnostics.Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();

        // Make it so the terminal isn't displayed on the screen when executing commands
        startInfo.CreateNoWindow = true;

        // The cmd terminal
        startInfo.FileName = "cmd.exe";
        
        Directory.SetCurrentDirectory(directory);

        startInfo.Arguments = "/C " + command;
        process.StartInfo = startInfo;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        String stdout = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        process.Close();

        return stdout;
    }

    static private int ExecuteCommandWithPassword(String directory, String command, String password)
    {
        Process process = new System.Diagnostics.Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();

        // Make it so the terminal isn't displayed on the screen when executing commands
        //startInfo.CreateNoWindow = true;

        //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        // The cmd terminal
        startInfo.FileName = "cmd.exe";

        Directory.SetCurrentDirectory(directory);

        startInfo.Arguments = "/C " + command;
        process.StartInfo = startInfo;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardInput = true;

        //process.EnableRaisingEvents = true;
        

        process.Start();
        //System.Threading.Thread.Sleep(10000);
        SendKeyTestCmdExe(process.MainWindowHandle);

        //process.StandardInput.WriteLine("/C dir");
        //Console.WriteLine("Done writing to standard input");
        
        //String stdout = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        int exitCode = process.ExitCode;
        //Debug.WriteLine(exitCode.ToString());
        process.Close();
        return exitCode;
        
        /*
        Debug.WriteLine("before end");
        if (process.HasExited)
        {
            Debug.WriteLine("process has exited");
            
        }
        else
        {
            Debug.WriteLine("process has not exited");
            return -10;
        }
        //return stdout;
         * */
    }

    /*
     * ParseStdOut()
     * 
     * Strips out the command prompt lines from stdout. The command prompt
     * starts with "C:". The rough format of standard output from executing Git
     * commands in CMD is:
     * 
     *      C:\current\directory>some stuff
     *      stdout line 1
     *      stdout line 2
     * 
     *      C:\current\directory>more stuff
     * 
     *      C:\current\directory>and more stuff
     * 
     * This method extracts and returns:
     * 
     *      stdout line 1
     *      stdout line 2
     *      
     */
    static private String ParseStdOut(String stdout)
    {
        String boundary = "^C:.*$";
        StringWriter writer = new StringWriter();
        int canWrite = 0;

        using (StringReader reader = new StringReader(stdout))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, boundary))
                {
                    if (++canWrite > 1)
                        break;
                }
                else
                    writer.WriteLine(line);
            }
        }
        return writer.ToString();
    }

    // Just for light testing
    static internal int TestModularTerminal()
    {
        return ExecuteCommandWithPassword(workingDirectory, "git push", password);

        // TODO: check stdout for success or failure
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

    static internal void SendKeyTestCmdExe(IntPtr cmdHandler)
    {
        /*
        IntPtr windowHandle = GetForegroundWindow();
        IntPtr childHandle;

        
        childHandle = FindWindow(null, "C:\\Windows\\system32\\cmd.exe");

        
        if (childHandle == IntPtr.Zero)
        {
            System.Windows.MessageBox.Show("cmd.exe is not running.");
        }
         * */
          
        //SetForegroundWindow(childHandle);

        SetForegroundWindow(cmdHandler);
        SendKeys.SendWait(password + "{ENTER}");

        Console.WriteLine("sent keys");

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
