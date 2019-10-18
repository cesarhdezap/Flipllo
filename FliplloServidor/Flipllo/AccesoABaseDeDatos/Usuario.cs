//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccesoABaseDeDatos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            this.Estado = 0;
            this.PartidasJugadas = 0;
            this.ExperienciaTotal = 0D;
            this.ObjetoEnInventario = new HashSet<ObjetoEnInventario>();
            this.Cofre = new HashSet<Cofre>();
        }
    
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Contraseña { get; set; }
        public string CorreoElectronico { get; set; }
        public short Estado { get; set; }
        public short PartidasJugadas { get; set; }
        public double ExperienciaTotal { get; set; }
        public string CodigoDeVerificacion { get; set; }
        public short Victorias { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ObjetoEnInventario> ObjetoEnInventario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cofre> Cofre { get; set; }
    }
}
