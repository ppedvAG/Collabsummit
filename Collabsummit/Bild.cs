using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Collabsummit
{
    public class Bild:INotifyPropertyChanged
    {
        private string _Pfad;
        public string Pfad
        {
            get { return _Pfad; }
            set { _Pfad = value;
                RaisePropertyChanged();
            }
        }
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
          }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };


    }

}
