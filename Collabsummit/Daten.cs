using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Collabsummit
{
    public class Daten: INotifyPropertyChanged
    {
        public async Task SaveIDAsync(object sender, RoutedEventArgs e)
        {

            var sfo = await ApplicationData.Current.LocalFolder.CreateFileAsync("collabsummit.txt",
                CreationCollisionOption.OpenIfExists);
            await FileIO.AppendTextAsync(sfo, this.ID+System.Environment.NewLine);
            this.ID = "";

        }


        //public string ID { get; set
        //        ; }
        private string id;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string ID
        {
            get { return id; }
            set { id = value;
                RaisePropertyChanged();
            }
        }

        public string Name { get; set; }
        public string Firma { get; set; }
        public string Email { get; set; }
        public string Strasse { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }
        private Boolean _Transfered;
        public Boolean Transfered
        {
            get { return _Transfered; }
            set
            {
                _Transfered = value;
                RaisePropertyChanged();
            }
        }
    }
    
}
