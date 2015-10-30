﻿using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ManuelsCouchTisch
{
	public class ViewModel : INotifyPropertyChanged
	{
		public void NotifyChanged(string memberName)
		{
			var pc = PropertyChanged;
			if (pc != null)
			{
				pc(this, new PropertyChangedEventArgs(memberName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class Command : ICommand
	{
		Action<object> _action;
		public Command(Action<object> action)
		{
			_action = action;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_action(parameter);
		}
	}
}
