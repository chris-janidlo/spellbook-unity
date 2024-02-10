using System;
using System.Collections.Generic;
using UnityEngine;

namespace crass
{
    // necessary in order to have a non-generic CrateMachineDriver class (since generic MonoBehaviours aren't supported)
    public abstract class ACrateMachine
    {
        internal abstract void Process(CrateMachineUpdateType updateType);
    }

    public enum CrateMachineUpdateType
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
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        void Update()
        {
            if (parent != null && parent.enabled)
                crateMachine.Process(CrateMachineUpdateType.Update);
        }

        void FixedUpdate()
        {
            if (parent != null && parent.enabled)
                crateMachine.Process(CrateMachineUpdateType.FixedUpdate);
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

        private readonly Dictionary<Type, Crate<TParent>> crates =
            new Dictionary<Type, Crate<TParent>>();

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

            Crate = GetCrateUnbounded(initialCrateType);
            driver = new GameObject($"{parentName} driver (for {machineType})");
            driver.AddComponent<CrateMachineDriver>().Initialize(this, parent);
            return this;
        }

        public Crate<TParent> GetCrate(Type crateType)
        {
            if (crateType.IsSubclassOf(typeof(Crate<TParent>)))
                return GetCrateUnbounded(crateType);
            else
            {
                var machineTypeName = $"CrateMachine<{typeof(TParent).Name}>";
                var parentName = parent.ToString();

                var message =
                    $"{machineTypeName} for {parentName}: type {crateType.Name} "
                    + "is not a valid crate type for this machine";
                throw new ArgumentException(message);
            }
        }

        public TCrate GetCrate<TCrate>()
            where TCrate : Crate<TParent>
        {
            return (TCrate)GetCrateUnbounded(typeof(TCrate));
        }

        private Crate<TParent> GetCrateUnbounded(Type crateType)
        {
            if (crates.TryGetValue(crateType, out var crate))
                return crate;
            else
            {
                var machineTypeName = $"CrateMachine<{typeof(TParent).Name}>";
                var crateTypeName = crateType.Name;
                var parentName = parent.ToString();

                var message =
                    $"{machineTypeName} for {parentName} has no crate for {crateTypeName} - "
                    + "did you forget to add one?";
                throw new KeyNotFoundException(message);
            }
        }

        public void SetCrate<TCrate>()
            where TCrate : Crate<TParent>
        {
            DoTransition(GetCrate<TCrate>());
        }

        public void SetCrate(Type crateType)
        {
            DoTransition(GetCrate(crateType));
        }

        internal override void Process(CrateMachineUpdateType updateType)
        {
            switch (updateType)
            {
                case CrateMachineUpdateType.Update:
                    Crate.OnUpdate();
                    break;

                case CrateMachineUpdateType.FixedUpdate:
                    Crate.OnFixedUpdate();
                    break;

                default:
                    throw new ArgumentException(updateType.ToString());
            }

            Type transition = Crate.GetTransition(updateType);
            if (transition is { } newCrateType)
                DoTransition(GetCrate(newCrateType));
        }

        private void DoTransition(Crate<TParent> newCrate)
        {
            var oldCrate = Crate;

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

        public virtual Type GetTransition(CrateMachineUpdateType updateType)
        {
            return null;
        }

        public virtual void OnUpdate() { }

        public virtual void OnFixedUpdate() { }
    }
}
