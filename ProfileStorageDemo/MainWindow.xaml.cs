using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ProfileStorageDemo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            

            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.StorageFolder localFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;

            // Save a string        
            localSettings.Values["myString"] = "Hello, World!";
            myTextBlock.Text += "\nWrote local setting" + localSettings.Values["myString"].ToString();


            Windows.Globalization.DateTimeFormatting.DateTimeFormatter formatter =
                new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("longtime");

            // Corrected the usage of FileIO.WriteTextAsync to await the asynchronous method
            StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt",
                CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, formatter.Format(DateTimeOffset.Now));

            
            myTextBlock.Text += "\nWrote local file " + sampleFile.Path  ;


            Windows.Storage.ApplicationDataContainer roamingSettings =
        Windows.Storage.ApplicationData.Current.RoamingSettings;
            Windows.Storage.StorageFolder roamingFolder =
                Windows.Storage.ApplicationData.Current.RoamingFolder;

            roamingSettings.Values["exampleSetting"] = "Hello World";
            myTextBlock.Text += "\nWrote roaming setting " + roamingSettings.Values["exampleSetting"].ToString();



            StorageFile roamingFile = await roamingFolder.CreateFileAsync("dataFile.txt",
                CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(roamingFile, formatter.Format(DateTimeOffset.Now));

            myTextBlock.Text += "\nWrote roaming file " + roamingFile.Path;

            
        }

        private async void readButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            Windows.Storage.StorageFolder localFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;

            

            // Safely retrieve the value from localSettings.Values
            if (localSettings.Values.TryGetValue("myString", out object? myStringValue) && myStringValue is string localSetting)
            {
                myTextBlock.Text += $"\nRead local setting: {localSetting}";
            }
            else
            {
                myTextBlock.Text += "\nLocal setting 'myString' not found or is null.";
            }

            // Safely retrieve the value from roamingSettings.Values
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            if(roamingSettings.Values.TryGetValue("exampleSetting", out object? exampleSettingValue) && exampleSettingValue is string roamingSetting)
            {
                myTextBlock.Text += $"\nRead roaming setting: {roamingSetting}";
            }
            else
            {
                myTextBlock.Text += "\nRoaming setting 'exampleSetting' not found or is null.";
            }

            // Safely read the file content 
            StorageFile? sampleFile = await localFolder.TryGetItemAsync("dataFile.txt") as StorageFile;
            if (sampleFile != null)
            {
                string fileContent = await FileIO.ReadTextAsync(sampleFile);
                myTextBlock.Text += $"\nRead local file: {fileContent}";
            }
            else
            {
                myTextBlock.Text += "\nLocal file 'dataFile.txt' not found.";
            }
            // Safely read the roaming file content

            Windows.Storage.StorageFolder roamingFolder =
                Windows.Storage.ApplicationData.Current.RoamingFolder;

            StorageFile? roamingFile = await roamingFolder.TryGetItemAsync("dataFile.txt") as StorageFile;
            if (roamingFile != null)
            {
                string roamingFileContent = await FileIO.ReadTextAsync(roamingFile);
                myTextBlock.Text += $"\nRead roaming file: {roamingFileContent}";
            }
            else
            {
                myTextBlock.Text += "\nRoaming file 'dataFile.txt' not found.";
            }

        }
    }


}
