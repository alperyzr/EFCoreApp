using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    public class _BaseEntity
    {
        //Identity Sadece insert edilirken bu değeri alır, update ederken almaz.
        //Computed enum ı ise insert ve update de de db ye o propertyi göndermiyor
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
      
        public DateTime? UpdatedDate { get; set; }
     
        public bool? IsActive { get; set; } = true;
     
        public bool? IsDeleted { get; set; } = false;
      
        public int? CreatedBy { get; set; }
     
        public int? UpdatedBy { get; set; }

    }
}
