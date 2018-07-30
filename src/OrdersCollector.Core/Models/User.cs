using System;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Models
{
    /// <summary>
    /// User of the app.
    /// </summary>
    public class User : EntityBase<Int64>
    {
        /// <summary>
        /// User name - skype handle.
        /// </summary>
        public string Name { get; set; }

        public string FullName { get; set; }

        public string DisplayName
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(FullName))
                {
                    return FullName;
                }

                return Name;
            }
        }

        /// <summary>
        /// If user has admin role.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// True if user can be selected as operator.
        /// </summary>
        public bool CanBeOperator { get; set; }

        /// <summary>
        /// If user is active user.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
