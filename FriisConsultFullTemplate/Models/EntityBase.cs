// // 
// // EntityBase.cs
// // Author: Per Friis per.friis@friisconsult.com
// // © 2018 Per Friis Consult ApS
// // Created: 8/21/2018 8:25 PM
using System;
namespace FriisConsultFullTemplate.Models
{
    public class EntityBase
    {
        public EntityBase()
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }
    }
}
