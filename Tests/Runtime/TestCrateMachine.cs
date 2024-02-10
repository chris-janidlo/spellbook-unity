using System.Collections;
using System.Collections.Generic;
using crass;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class TestCrateMachine
{
    private class ScriptWithCrateMachine : MonoBehaviour
    {
        public CrateMachine<ScriptWithCrateMachine> CrateMachine;

        public class StateOne : Crate<ScriptWithCrateMachine> { }

        public class StateTwo : Crate<ScriptWithCrateMachine> { }

        public void Awake()
        {
            CrateMachine = new CrateMachine<ScriptWithCrateMachine>(this)
                .AddCrate(new StateOne(), initial: true)
                .AddCrate(new StateTwo())
                .Build();
        }
    }

    private ScriptWithCrateMachine crateMachineOwner;

    [SetUp]
    public void Setup()
    {
        crateMachineOwner = new GameObject(
            "test stub",
            typeof(ScriptWithCrateMachine)
        ).GetComponent<ScriptWithCrateMachine>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(crateMachineOwner.gameObject);
    }

    [Test]
    public void TestSetCrateCallsOnTransition()
    {
        bool wasCalled = false;
        crateMachineOwner.CrateMachine.OnTransition += (o, n) => wasCalled = true;

        crateMachineOwner.CrateMachine.SetCrate<ScriptWithCrateMachine.StateTwo>();
        Assert.That(wasCalled, Is.True);

        wasCalled = false;
        crateMachineOwner.CrateMachine.SetCrate(typeof(ScriptWithCrateMachine.StateOne));
        Assert.That(wasCalled, Is.True);
    }
}
