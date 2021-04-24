using ClientWPFInterface.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ClientWPFInterface.Models
{
    public class WindowModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private UserValidator _validator;

        public WindowModel()
        {
            _validator = new UserValidator();
        }
        public string this[string columnName] 
        {
            get 
            {
                string message = null;
                message = 
                    _validator.Validate(this).Errors
                    .FirstOrDefault(x => x.PropertyName == columnName).ErrorMessage;
                return message;
            }
        }

        private string _nameUser;
        public string NameUser
        {
            get { return _nameUser; }
            set { _nameUser = value; }
        }


        public string Error 
        {
            get 
            {
                string message = null;
                var messages = _validator.Validate(this).Errors.Select(x => x).ToArray();
                message = string.Join(Environment.NewLine, messages.Select(x => x.ErrorMessage));
                return message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
