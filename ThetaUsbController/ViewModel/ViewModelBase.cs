using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ThetaUsbController.ViewModel
{
    /// <summary>
    /// ViewModelの基底クラス
    /// abstractなのでインスタンスを生成できない
    /// 継承して使うことが前提となる
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
            return;
        }

        #region Command
        protected class DelegateCommand : ICommand
        {
            private Action<object> command;
            private Func<object, bool> canExecute;

            // コンストラクタ
            public DelegateCommand(Action<object> command, Func<object, bool> canExecute)
            {
                this.command = command;
                this.canExecute = canExecute;
            }

            public void Execute(object parameter)
            {
                command(parameter);
            }

            public bool CanExecute(object parameter)
            {
                if (canExecute != null)
                {
                    return canExecute(parameter);
                }
                else
                {
                    return true;
                }
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
        #endregion
    }
}
