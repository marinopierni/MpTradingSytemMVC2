//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MpTradingSytemMVC2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Titoli
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Titoli()
        {
            this.Prezzi = new HashSet<Prezzi>();
        }
        [Key]
        public long IdTitolo { get; set; }
        public string NomeTitolo { get; set; }
        public string Simbolo { get; set; }
        public string UltimoScambio { get; set; }
        public string Variazione { get; set; }
        public Nullable<decimal> Volume { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prezzi> Prezzi { get; set; }
    }
}
