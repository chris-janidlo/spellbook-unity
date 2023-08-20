using System;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
    // necessary in order to have a non-generic CrateMachineDriver class (since generic MonoBehaviours aren't supported)
    public abstract class ACrateMachine
    {
        internal abstract void Process(CrateMachineProcessType processType);
    }

    internal enum CrateMachineProcessType
    {
        Update,
        FixedUpdate
    }

    internal class CrateMachineDriver : MonoBehaviour
    {
        private ACrateMachine crateMachine;
        private MonoBehaviour parent;

        public void Initialize(ACrateMachine crateMachine, MonoBehaviour parent)
        {
            this.crateMachine = crateMachine;
            this.parent = parent;

            transform.parent = parent.transform;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        void Update()
        {
            if (parent != null && parent.enabled)
                crateMachine.Process(CrateMachineProcessType.Update);
        }

        void FixedUpdate()
        {
            if (parent != null && parent.enabled)
                crateMachine.Process(CrateMachineProcessType.FixedUpdate);
        }
    }

    public class CrateMachine<TParent> : ACrateMachine
        where TParent : MonoBehaviour
    {
        private readonly TParent parent;
        private readonly Dictionary<Type, Crate<TParent>> crates = new();
        private Crate<TParent> currentCrate;
        private Type initialCrateType;
        private GameObject driver;

        public CrateMachine(TParent parent)
        {
            this.parent = parent;
        }

        public CrateMachine<TParent> AddCrate<TCrate>(TCrate crate, bool initial = false)
            where TCrate : Crate<TParent>
        {
            crates[typeof(TCrate)] = crate;
            crate.SetParent(parent);
            if (initial)
                SetInitialCrate<TCrate>();
            return this;
        }

        public CrateMachine<TParent> SetInitialCrate<TCrate>()
            where TCrate : Crate<TParent>
        {
            initialCrateType = typeof(TCrate);
            return this;
        }

        public CrateMachine<TParent> Build()
        {
            var machineType = $"CrateMachine<{typeof(TParent).Name}>";
            var parentName = parent.ToString();

            if (crates.Count == 0)
                throw new InvalidOperationException(
                    $"{machineType} for {parentName} has no crates"
                );

            if (initialCrateType == null)
                throw new InvalidOperationException(
                    $"{machineType} for {parentName} has no initial crate set"
                );

            if (driver != null)
                throw new InvalidOperationException(
                    $"{machineType} is already built for {parentName}"
                );

            currentCrate = crates[initialCrateType];
            driver = new GameObject($"{parentName} driver (for {machineType})");
            driver.AddComponent<CrateMachineDriver>().Initialize(this, parent);
            return this;
        }

        public void CustomCall<TInterface>(Action<TInterface> call)
        {
            if (currentCrate is TInterface @interface)
                call(@interface);
        }

        internal override void Process(CrateMachineProcessType processType)
        {
            switch (processType)
            {
                case CrateMachineProcessType.Update:
                    currentCrate.OnUpdate();
                    break;

                case CrateMachineProcessType.FixedUpdate:
                    currentCrate.OnFixedUpdate();
                    break;

                default:
                    throw new ArgumentException(processType.ToString());
            }

            DoTransition(currentCrate.GetTransition());
        }

        private void DoTransition(Type newCrateType)
        {
            if (newCrateType is null)
                return;

            currentCrate.OnExit();
            currentCrate = crates[newCrateType];
            currentCrate.OnEnter();
        }
    }

    public abstract class Crate<TParent>
        where TParent : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        protected TParent C;

        public void SetParent(TParent parent)
        {
            C = parent;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual Type GetTransition()
        {
            return null;
        }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }
    }
}
