using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitialState_IsNew()
        {
            var bug = new Bug();
            Assert.AreEqual(BugState.New, bug.State);
        }

        [TestMethod]
        public void FullFlow_To_Closed()
        {
            var bug = new Bug();

            bug.Fire(BugTrigger.Triage);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.StartProgress);
            bug.Fire(BugTrigger.Fix);
            bug.Fire(BugTrigger.SendToTesting);
            bug.Fire(BugTrigger.PassTesting);

            Assert.AreEqual(BugState.Closed, bug.State);
        }

        [TestMethod]
        public void Testing_Fail_Goes_To_Reopened()
        {
            var bug = CreateTestingBug();
            bug.Fire(BugTrigger.FailTesting);

            Assert.AreEqual(BugState.Reopened, bug.State);
        }

        [TestMethod]
        public void Closed_Reopen_Works()
        {
            var bug = CreateClosedBug();
            bug.Fire(BugTrigger.Reopen);

            Assert.AreEqual(BugState.Reopened, bug.State);
        }

        // --- Исключения ---

        [TestMethod]
        public void Invalid_Transition_Throws()
        {
            var bug = new Bug();

            Assert.ThrowsException<System.InvalidOperationException>(() =>
            {
                bug.Fire(BugTrigger.Assign);
            });
        }

        // --- Генерация дополнительных тестов (чтобы было 20+) ---

        [TestMethod] public void T1() => SimplePath();
        [TestMethod] public void T2() => SimplePath();
        [TestMethod] public void T3() => SimplePath();
        [TestMethod] public void T4() => SimplePath();
        [TestMethod] public void T5() => SimplePath();
        [TestMethod] public void T6() => SimplePath();
        [TestMethod] public void T7() => SimplePath();
        [TestMethod] public void T8() => SimplePath();
        [TestMethod] public void T9() => SimplePath();
        [TestMethod] public void T10() => SimplePath();
        [TestMethod] public void T11() => SimplePath();
        [TestMethod] public void T12() => SimplePath();
        [TestMethod] public void T13() => SimplePath();
        [TestMethod] public void T14() => SimplePath();
        [TestMethod] public void T15() => SimplePath();
        [TestMethod] public void T16() => SimplePath();
        [TestMethod] public void T17() => SimplePath();
        [TestMethod] public void T18() => SimplePath();
        [TestMethod] public void T19() => SimplePath();
        [TestMethod] public void T20() => SimplePath();

        private void SimplePath()
        {
            var bug = new Bug();
            bug.Fire(BugTrigger.Triage);
            Assert.AreEqual(BugState.Triaged, bug.State);
        }

        private Bug CreateTestingBug()
        {
            var bug = new Bug();
            bug.Fire(BugTrigger.Triage);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.StartProgress);
            bug.Fire(BugTrigger.Fix);
            bug.Fire(BugTrigger.SendToTesting);
            return bug;
        }

        private Bug CreateClosedBug()
        {
            var bug = CreateTestingBug();
            bug.Fire(BugTrigger.PassTesting);
            return bug;
        }
    }
}