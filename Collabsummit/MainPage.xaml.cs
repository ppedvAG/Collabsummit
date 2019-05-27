using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace Collabsummit
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Daten VM { get; set; } = new Daten();
        DispatcherTimer dp = new DispatcherTimer();
        List<Bilder> liste = new List<Bilder>();
        Random rnd = new Random(5);
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        public MainPage()
        {

            this.InitializeComponent();




            
         
         }
        public Bild MyBild { get; set; }
        private async void Dp_TickAsync(object sender, object e)
        {
            
            var pfad = itemsList.Skip(counter).First().Path;

            var file = await StorageFile.GetFileFromPathAsync(pfad);
            var stream = await file.OpenReadAsync();
            var imageSource = new BitmapImage();
            await imageSource.SetSourceAsync(stream);
            bild1.Source = imageSource;
            counter++;
            if (counter == itemsList.Count()) counter = 0;

        }
        int counter = 0;
        IReadOnlyList<IStorageItem> itemsList;
        private async void Grid_LoadedAsync(object sender, RoutedEventArgs e)
        {
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            StorageFolder sf = await picturesFolder.GetFolderAsync("collabsummit");


            itemsList = await sf.GetItemsAsync();
            dp.Tick += Dp_TickAsync;
            dp.Interval = new TimeSpan(0, 0, 5);

            dp.Start();

        }

        public async void SaveIDAsync(object sender, RoutedEventArgs e)
        {
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dp.Stop();
            this.Frame.Navigate(typeof(QRCodeList));

        }

       

        private void Text1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length>10 && popup.Visibility==Visibility.Collapsed)
            {

             
                var zahl = rnd.Next(5);
                if (zahl==4)
                {
                    popup.Background = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    popup.Background = new SolidColorBrush(Colors.Silver);
                }
                popup.Visibility = Visibility.Visible;

            }
        }

        private async void SubmitScan_Click(object sender, RoutedEventArgs e)
        {
            if (VM.ID.Length > 10)
            {
                var sfo = await ApplicationData.Current.LocalFolder.CreateFileAsync("collabsummit.txt",
            CreationCollisionOption.OpenIfExists);
                await FileIO.AppendTextAsync(sfo, VM.ID + System.Environment.NewLine);
                
            }
            VM.ID = "";
            text1.Focus(FocusState.Programmatic);

            popup.Visibility = Visibility.Collapsed;

        }
    }
}
