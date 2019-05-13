using Microsoft.Win32;
using System;
using System.Windows.Forms;
using Computer;
public class Library
{
    public Library()
    {

    }
    public static void enforceAdminPrivilegesWorkaround()
    {
        RegistryKey rk;
        string registryPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon\";

        try
        {
            if (Environment.Is64BitOperatingSystem)
            {
                rk = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            }
            else
            {
                rk = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry32);
            }

            rk = rk.OpenSubKey(registryPath, true);
        }
        catch (System.Security.SecurityException ex)
        {
            MessageBox.Show("Please run as administrator");
            System.Environment.Exit(1);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
    public static void ExecuteCommandSync(string command)
    {
        try
        {
            // create the ProcessStartInfo using "cmd" as the program to be run,
            // and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows,
            // and then exit.
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            if (ComputerInfo.DEBUG == true)
            {
                MessageBox.Show(command + " Executed");
            }
        }
        catch (Exception objException)
        {
            // Log the exception
            MessageBox.Show("Some text", "Some title",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}