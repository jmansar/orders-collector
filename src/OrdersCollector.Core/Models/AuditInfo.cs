using System;

namespace OrdersCollector.Core.Models
{
    /// <summary>
    /// Holds information about source and user that made changes.
    /// </summary>
    public class AuditInfo
    {
        /// <summary>
        /// Source. For example: Skype
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Sub source. For example skype chat name.
        /// </summary>
        public string SubSource { get; set; }

        /// <summary>
        /// SubSubSource. For example skype message id.
        /// </summary>
        public string SubSubSource { get; set; }

        /// <summary>
        /// Identifier of the user that caused the change to occur.
        /// </summary>
        public string InvokedBy { get; set; }

        /// <summary>
        /// Name of the user that caused the change to occur.
        /// </summary>
        public string InvokedByName { get; set; }

        /// <summary>
        /// Operation date.
        /// </summary>
        public DateTime? Date { get; set; }
    }
}
