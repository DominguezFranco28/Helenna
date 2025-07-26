using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFixedUpdate 
{
    //las maquina de estado no heredan de monobehaviour, asi que no cuentan perse con este metodo de unity
    void FixedUpdate();

}
