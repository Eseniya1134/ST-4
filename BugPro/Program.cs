// See https://aka.ms/new-console-template for more information
using System;
using Stateless;

namespace BugPro
{
    public enum BugState
    {
        New,
        Triaged,
        NeedMoreInfo,
        Assigned,
        InProgress,
        Fixed,
        Testing,
        Reopened,
        Closed,
        Rejected
    }

    public enum BugTrigger
    {
        Triage,
        NeedInfo,
        ProvideInfo,
        Assign,
        StartProgress,
        Fix,
        SendToTesting,
        FailTesting,
        PassTesting,
        Reopen,
        Reject
    }

    public class Bug
    {
        private readonly StateMachine<BugState, BugTrigger> _machine;

        public BugState State => _machine.State;

        public Bug()
        {
            _machine = new StateMachine<BugState, BugTrigger>(BugState.New);
            Configure();
        }

        private void Configure()
        {
            _machine.Configure(BugState.New)
                .Permit(BugTrigger.Triage, BugState.Triaged);

            _machine.Configure(BugState.Triaged)
                .Permit(BugTrigger.NeedInfo, BugState.NeedMoreInfo)
                .Permit(BugTrigger.Assign, BugState.Assigned)
                .Permit(BugTrigger.Reject, BugState.Rejected);

            _machine.Configure(BugState.NeedMoreInfo)
                .Permit(BugTrigger.ProvideInfo, BugState.Triaged);

            _machine.Configure(BugState.Assigned)
                .Permit(BugTrigger.StartProgress, BugState.InProgress);

            _machine.Configure(BugState.InProgress)
                .Permit(BugTrigger.Fix, BugState.Fixed);

            _machine.Configure(BugState.Fixed)
                .Permit(BugTrigger.SendToTesting, BugState.Testing);

            _machine.Configure(BugState.Testing)
                .Permit(BugTrigger.PassTesting, BugState.Closed)
                .Permit(BugTrigger.FailTesting, BugState.Reopened);

            _machine.Configure(BugState.Reopened)
                .Permit(BugTrigger.Assign, BugState.Assigned);

            _machine.Configure(BugState.Closed)
                .Permit(BugTrigger.Reopen, BugState.Reopened);

            _machine.Configure(BugState.Rejected)
                .Permit(BugTrigger.Reopen, BugState.Triaged);
        }

        public void Fire(BugTrigger trigger)
        {
            _machine.Fire(trigger);
        }
    }

    class Program
    {
        static void Main()
        {
            var bug = new Bug();

            Console.WriteLine($"Start: {bug.State}");

            bug.Fire(BugTrigger.Triage);
            bug.Fire(BugTrigger.Assign);
            bug.Fire(BugTrigger.StartProgress);
            bug.Fire(BugTrigger.Fix);
            bug.Fire(BugTrigger.SendToTesting);
            bug.Fire(BugTrigger.PassTesting);

            Console.WriteLine($"End: {bug.State}");
        }
    }
}