using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Collections;
using Sitecore.Workflows;

namespace MikeRobbins.AdvancedEmailAction.Entities
{
    public class WorkflowHistoryItem
    {
        public string ItemPath { get; set; }

        public string ItemLanguage { get; set; }

        public int Version { get; set; }

        public string DisplayName { get; set; }

        public DateTime Updated { get; set; }

        public WorkflowState WorkflowState { get; set; }

        public string PreviousState { get; set; }

        public string WorkflowName { get; set; }

        public string Username { get; set; }

        public StringDictionary Comments { get; set; }

    }
}
