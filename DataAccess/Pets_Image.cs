//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pets_Image
    {
        public int PetsImage_Id { get; set; }
        public string PetsImage_Path { get; set; }
        public string PetsImage_Name { get; set; }
        public Nullable<int> PetsC3_Id { get; set; }
    
        public virtual Pets_C3 Pets_C3 { get; set; }
    }
}