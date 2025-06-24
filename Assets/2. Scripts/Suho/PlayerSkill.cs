using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
   BaseController<PlayerUnitController, PlayerUnitState> controller;
   SelectTargetType targetType;
   private TargetSelect selectedTarget;
   

   private void Awake()
   {
      controller = GetComponent<BaseController<PlayerUnitController, PlayerUnitState>>();
   }
   
   
   
   
}
