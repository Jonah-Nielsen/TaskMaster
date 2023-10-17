using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Taskmaster
{
    
}

public class New : ICommand
{
    private Action executeAction;

    public New(Action executeAction)
    {
        this.executeAction = executeAction;
    }


   

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        //Add Task
        throw new NotImplementedException();
    }

    public event EventHandler CanExecuteChanged;
}
