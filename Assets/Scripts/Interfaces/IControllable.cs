using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable 
{
    void SetControl(bool isActive);
    void SetMovementEnabled(bool isEnabled);
}
