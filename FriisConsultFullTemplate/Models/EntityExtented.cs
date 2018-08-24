// // 
// // EntityExtented.cs
// // Author: Per Friis per.friis@friisconsult.com
// // © 2018 Per Friis Consult ApS
// // Created: 8/21/2018 8:26 PM
using System;
namespace FriisConsultFullTemplate.Models
{
    public class EntityExtented : EntityBase
    {
        public EntityExtented()
        {
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>The detail.</value>
        public string Detail { get; set; }

    }
}
