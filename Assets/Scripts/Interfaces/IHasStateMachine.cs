using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public interface IHasStateMachine
{
    //las maquina de estado no heredan de monobehaviour, asi que no cuentan perse con este metodo de unity
    IState CurrentState { get; }

}
