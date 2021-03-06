//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedCenter
{
    using System;
    using System.Collections.Generic;
    
    public partial class Прием
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Прием()
        {
            this.Лечение = new HashSet<Лечение>();
        }
    
        public int НомерПриема { get; set; }
        public int ID_Клиента { get; set; }
        public int ID_Врача { get; set; }
        public System.DateTime Дата { get; set; }
        public string Заключение { get; set; }
    
        public virtual Врач Врач { get; set; }
        public virtual Клиент Клиент { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Лечение> Лечение { get; set; }
    }
}
