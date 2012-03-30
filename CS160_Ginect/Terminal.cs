using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Terminal
{
    static internal String workingDirectory = @"C:\Users\Jessica\Ginect";

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

        String stdout = ExecuteCommand(workingDirectory, "git add" + filesStr);

        // TODO: check stdout for success or failure
    }

    static internal String GitTagLatestCommit(String tagName)
    {
        String latestCommitID = GetLatestCommitID();
        return GitTag(tagName, latestCommitID);
    }

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

    static private String GitTag(String tagName, String commitID)
    {
        String stdout = ExecuteCommand(workingDirectory, "git tag -f " + tagName + " " + commitID);

        // TODO: check stdout for success or failure

        return ParseStdOut(stdout);
    }

    static internal void GitPush()
    {
        String stdout = ExecuteCommand(workingDirectory, "git push --tags");

        // TODO: check stdout for success or failure
    }

    static private String ExecuteCommand(String directory, String command) 
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

        // Make it so the terminal isn't displayed on the screen when it's executing commands
        startInfo.CreateNoWindow = true;

        // The cmd terminal
        startInfo.FileName = "cmd.exe";
        
        Directory.SetCurrentDirectory(directory);

        startInfo.Arguments = "/C " + command;
        process.StartInfo = startInfo;

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        string stdout = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        process.Close();

        return stdout;
    }

    /*
     * ParseStdOut()
     * 
     * Strips out the command prompt from stdout. The command prompt starts
     * with "C:".
     */
    static internal String ParseStdOut(String stdout)
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
    static internal String TestModularTerminal()
    {
        return ExecuteCommand(workingDirectory, "git log");

        // TODO: check stdout for success or failure
    }
}
