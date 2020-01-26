using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class GoalsCollection
    {
        public List<singleGoal> Goals = new List<singleGoal>()
        {
            new singleGoal(){GoalID="AAA80904-BC5E-4FB2-BA07-CF6FA0166BC4",isAchived=false,GoalName="Goal-1"},
            new singleGoal(){GoalID="27A4764E-44B5-4ED9-BAE3-EC59D94D2D31",isAchived=false,GoalName="Goal-2"},
            new singleGoal(){GoalID="4437885A-3AD1-4E71-9C66-C24093199421",isAchived=false,GoalName="Goal-3"},
            new singleGoal(){GoalID="DF977C9C-E290-4EB9-B9FC-C52EF0C11B5F",isAchived=false,GoalName="Goal-4"},
            new singleGoal(){GoalID="321A0F2C-3AAA-4B8F-BEB9-B37EDB817AA5",isAchived=false,GoalName="Goal-5"},
            new singleGoal(){GoalID="40AE404F-ECA9-412E-8BEB-72F30E8AA26C",isAchived=false,GoalName="Goal-666"}
        };
    }
    public class singleGoal
    {
        public string GoalName { get; set; }
        public string GoalID { get; set; }
        public bool isAchived { get; set; }
    }
}
