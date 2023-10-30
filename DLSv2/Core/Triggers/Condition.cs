﻿using System;

namespace DLSv2.Core.Triggers
{
    public class ConditionArgs : EventArgs
    {
        public bool ConditionMet;
        public ConditionArgs(bool conditionMet) => ConditionMet = conditionMet;
    }

    public abstract class BaseCondition
    {
        public event EventHandler<ConditionArgs> ConditionChangedEvent;

        public bool LastTriggered = false;

        public void ConditionResult(bool status)
        {
            if (LastTriggered == status) return;
            LastTriggered = status;
            OnRaiseCustomEvent(new ConditionArgs(status));
        }

        protected virtual void OnRaiseCustomEvent(ConditionArgs e)
        {
            EventHandler<ConditionArgs> raiseEvent = ConditionChangedEvent;
            if (raiseEvent != null) raiseEvent(this, e);
        }
    }

    public class VehicleCondition : BaseCondition
    {
        private Func<ManagedVehicle, bool> EvalFunc;

        public VehicleCondition(Func<ManagedVehicle, bool> func) => EvalFunc = func;

        public bool Evaluate(ManagedVehicle managedVehicle) => EvalFunc(managedVehicle);
    }

    public class GlobalCondition : BaseCondition
    {
        private Func<bool> EvalFunc;

        public GlobalCondition(Func<bool> func) => EvalFunc = func;

        public bool Evaluate() => EvalFunc();
    }
}