using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace HonyakuLens.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string credentialPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

            if (!File.Exists(credentialPath))
            {
                credentialPath = null;
            }

            if (credentialPath == null)
            {
                if (File.Exists(".\\google-cloud.json"))
                {
                    credentialPath = ".\\google-cloud.json";
                }
            }

            if (credentialPath == null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog()
                {
                    Filter = "JSON file|*.json",
                    Multiselect = false,
                    Title = "Select Google Cloud credential file"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    credentialPath = openFileDialog.FileName;
                }
            }

            if (credentialPath == null)
            {
                MessageBox.Show(
                    "Please either set environment variable \"GOOGLE_APPLICATION_CREDENTIALS\" or select a valid " +
                        "credential file",
                    "Missing credential file", MessageBoxButton.OK, MessageBoxImage.Error);

                Environment.Exit(1);
            }

            Environment.SetEnvironmentVariable(
                "GOOGLE_APPLICATION_CREDENTIALS", Path.GetFullPath(credentialPath), EnvironmentVariableTarget.Process);

            string[] lines = File.ReadAllLines(credentialPath);

            foreach (var line in lines)
            {
                if (!line.Contains("project_id"))
                {
                    continue;
                }

                int start = line.IndexOf(':');

                start = line.IndexOf('"', start) + 1;

                int end = line.IndexOf('"', start);

                Environment.SetEnvironmentVariable(
                    "GOOGLE_PROJECT_ID", line[start..end],EnvironmentVariableTarget.Process);
            }
        }
    }
}
