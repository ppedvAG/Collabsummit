using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZXing;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Collabsummit
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class QRCodeList : Page
    {
        public ObservableCollection<Daten> ScannedList { get; set; } = new ObservableCollection<Daten>();
        public QRCodeList()
        {

            this.InitializeComponent();
        }

        private async void Grid_LoadedAsync(object sender, RoutedEventArgs e)
        {
            var f = await ApplicationData.Current.LocalFolder.GetFileAsync("collabsummit.txt");

            var d = await Windows.Storage.FileIO.ReadLinesAsync(f);
            foreach (var item in d)
            {
                ScannedList.Add(new Daten() { ID = item, Transfered = false });

            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)

        {
            var d = (Daten)e.AddedItems.First();
            if (d.ID.Length > 0)
            {
                var write = new BarcodeWriter();
                write.Format = ZXing.BarcodeFormat.QR_CODE;
                var wb = write.Write(d.ID);
                this.qrcodeImg.Source = wb;
            }


        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
        private Daten daten;

        public Daten Aktuell
        {
            get { return daten; }
            set { daten = value; }
        }


        private async void Scanned_Click(object sender, RoutedEventArgs e)
        {
            if (ListView1.SelectedIndex==-1)
            {
                ListView1.SelectedIndex = 0;
                return;
            }
            Daten aktuell = (Daten)ListView1.SelectedItem;
            ScannedList.Where(x => x.ID == aktuell.ID & x.Transfered == false).First().Transfered = true;
            if (ListView1.SelectedIndex + 1 == ListView1.Items.Count())
            {
                ContentDialog dlg = new ContentDialog()
                {
                    Title = "Fertig?",
                    Content = "Ablegen der letzten Scans in Backup.txt",
                    PrimaryButtonText = "Ok"
                };

                ContentDialogResult result = await dlg.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    ListView1.SelectedItem = -1;
                    var unscanned = await ApplicationData.Current.LocalFolder.CreateFileAsync("unscanned.txt",
CreationCollisionOption.OpenIfExists);
                    foreach (var item in ScannedList)
                    {
                        if (item.Transfered == true)
                        {
                            var sfo = await ApplicationData.Current.LocalFolder.CreateFileAsync("backup.txt",
      CreationCollisionOption.OpenIfExists);
                            await FileIO.AppendTextAsync(sfo, item.ID + System.Environment.NewLine);

                           // ScannedList.Remove(item); //Crash
                        }
                        else
                        {
    
                            await FileIO.AppendTextAsync(unscanned, item.ID + System.Environment.NewLine);
                        }
                    }
                    //copy unscanned->collabsummit
                    try
                    {
                        StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("unscanned.txt");
                        StorageFile copyFile = await file.CopyAsync(ApplicationData.Current.LocalFolder,
                            "collabsummit.txt", NameCollisionOption.ReplaceExisting);
                        file.DeleteAsync();
                        //remove Items von Scanedlist
                     
                    }
                    catch (Exception)
                    {

                    }
                    ListView1.Visibility = Visibility.Collapsed;

                }
                return;
            }
            else
            {
                ListView1.SelectedIndex++;
            }
        }
    }
}
