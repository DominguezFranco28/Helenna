using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable 
{
    bool GetControl();
    void SetControl(bool isActive);
    void SetMovementEnabled(bool isEnabled);
}
