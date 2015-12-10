//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MCX.Models.Tables
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Users
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Login()
        //{
        //    this.LeadsAandPotentials = new HashSet<LeadsAandPotential>();
        //    this.Tasks = new HashSet<Task>();
        //}


        [Key]
        public long LoginId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "EmailID")]
        public string EmailId { get; set; }

        [Required]
        [MaxLength(15)]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string UserType { get; set; }

        [DefaultValue("true")]
        public bool IsActive { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }

     
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<LeadsAandPotential> LeadsAandPotentials { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Task> Tasks { get; set; }
    }


}
