//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Skidor4.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class persons
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public persons()
        {
            this.chiplink = new HashSet<chiplink>();
        }
    
        public int pk_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string adress { get; set; }
        public Nullable<int> zipcode { get; set; }
        public string password { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string alias { get; set; }
        public Nullable<bool> @public { get; set; }
        public int fk_permission_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<chiplink> chiplink { get; set; }
        public virtual permissions permissions { get; set; }
    }
}