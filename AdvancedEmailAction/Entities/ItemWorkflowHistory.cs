using System;

namespace MikeRobbins.AdvancedEmailAction.Entities
{
    public class ItemWorkflowHistory
    {
        public DateTime ItemDateTime { get; set; }
        public string User { get; set; }
        public string PreviousState { get; set; }
        public string CurrentState { get; set; }
        public string Comment { get; set; }
    }
}
