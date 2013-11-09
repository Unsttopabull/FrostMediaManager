using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common;

namespace Frost.UI
{
	public partial class Reload : INotifyPropertyChanged
	{
	    private DBSystem _sistemKnjiznice;

	    public Reload()
		{
			InitializeComponent();
            SistemKnjiznice = DBSystem.Xtreamer;
		}

        private DBSystem SistemKnjiznice {
	        get { return _sistemKnjiznice; }
	        set {
                if (value == _sistemKnjiznice) {
                    return;
                }
                _sistemKnjiznice = value; 

                OnPropertyChanged();
            }
	    }

	    private IEnumerable<string> SistemNames {
            get { return Enum.GetNames(typeof(DBSystem)); }
	    }

	    public event PropertyChangedEventHandler PropertyChanged;

	    public void Connect(int connectionId, object target) {
	        throw new NotImplementedException();
	    }

	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
	        if (PropertyChanged != null) {
	            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
	        }
	    }
	}
}