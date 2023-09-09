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

    public enum CrateMachineProcessType
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
        public Crate<TParent> Crate { get; private set; }

        public delegate void TransitionHandler(Crate<TParent> oldCrate, Crate<TParent> newCrate);

        /// <summary>
        /// Invoked between the completion of <c>oldCrate.OnExit</c> and the start of
        /// <c>newCrate.OnEnter</c>, and after <c>Crate</c> is updated.
        /// </summary>
        public event TransitionHandler OnTransition;

        private readonly TParent parent;
        private readonly Dictionary<Type, Crate<TParent>> crates = new();
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

            Crate = crates[initialCrateType];
            driver = new GameObject($"{parentName} driver (for {machineType})");
            driver.AddComponent<CrateMachineDriver>().Initialize(this, parent);
            return this;
        }

        internal override void Process(CrateMachineProcessType processType)
        {
            switch (processType)
            {
                case CrateMachineProcessType.Update:
                    Crate.OnUpdate();
                    break;

                case CrateMachineProcessType.FixedUpdate:
                    Crate.OnFixedUpdate();
                    break;

                default:
                    throw new ArgumentException(processType.ToString());
            }

            Type transition = Crate.GetTransition(processType);
            if (transition is { } newCrateType)
                DoTransition(newCrateType);
        }

        private void DoTransition(Type newCrateType)
        {
            var oldCrate = Crate;
            var newCrate = crates[newCrateType];

            oldCrate.OnExit();
            Crate = newCrate;
            OnTransition?.Invoke(oldCrate, newCrate);
            newCrate.OnEnter();
        }
    }

    // implementation note: could make the virtual methods here protected so that the
    // parent class and outside classes can't break things, but have chosen not to for
    // the sake of exposing them to tests. might revisit this decision later.
    public abstract class Crate<TParent>
        where TParent : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        protected TParent C;

        internal void SetParent(TParent parent)
        {
            C = parent;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual Type GetTransition(CrateMachineProcessType processType)
        {
            return null;
        }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }
    }
}
