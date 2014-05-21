using System;

namespace MikeRobbins.AdvancedEmailAction.Entities
{
    public class ItemWorkflowHistory
    {
        public DateTime ItemDateTime { get; set; }
        public String User { get; set; }
        public String PreviousState { get; set; }
        public String CurrentState { get; set; }
        public String Comment { get; set; }
    }
}
