using System.Collections.Generic;

namespace MikeRobbins.AdvancedEmailAction.Comparers
{
    public class EmailComparer: IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x == y;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
