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
    
    public partial class Лечение
    {
        public int ID_Лечения { get; set; }
        public int НомерПриема { get; set; }
        public int ID_Услуги { get; set; }
        public int ID_Зуба { get; set; }
    
        public virtual Зуб Зуб { get; set; }
        public virtual Услуга Услуга { get; set; }
        public virtual Прием Прием { get; set; }
    }
}
