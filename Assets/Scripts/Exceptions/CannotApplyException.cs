using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    public class CannotApplyException : Exception
    {
        public string itemName;
        public string applicableName;
        public CannotApplyException(string itemName, string applicableName) : base($"Cannot apply {itemName} in {applicableName}")
        {
            this.itemName = itemName;
            this.applicableName = applicableName;

        }

    }
}
