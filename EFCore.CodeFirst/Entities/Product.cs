using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.Entities
{
    //Name ve Price Propertysinin EfCore tarafında Indexlenip, sql tarafında çok daha hızlı sonuç vermesi için kullanılır
    //Birden fazla property tanımlanırsa bunun adına ComposedIndex denir
    //Aşağıdaki gibi ComposedIndex üzerinde iki veya daha fazla property verilmişse sorguda ikiside aynı anda kullanıldığı zaman hızlı sonuç verir.
    //sadece ikinci propertye göre sorgu yaparsak indexte ikinci parametre olarak belirttiğimiz için biraz daha yavaş sonuç döner.
    //Bununda önüne geçmek için örneğin sadece price için Index atacaksat [Index(nameof(Price))] attribütüne arttırmamız gerekir
    [Index(nameof(Name), nameof(Price))]
    [Index(nameof(Name))]
    [Index(nameof(Price))]
    public class Product:_BaseEntity
    {
        public int Id { get; set; }

        
        public string Name { get; set; }

        //[Unicode(false)] attribute ü sadece ASCII karakterleri içermesini istediğimiz, türkçe karakter barındırmayacak proppertyler için kullanılır
        //Sql tarafında nvarchar olarak değil, varchar olarak belirtilir ve her karakteri 2 bytre yerine 1 byte olarak saklanır
        //Böylece Sql tarafının daha verimli ve daha az yer maliyetli çalışması sağlanır
        //Aynı zamanda [Column(TypeName="")] ile birlikte tirpini ve max alacağı karakter sayısınıda belirtebilir
        [Unicode(false)]
        [Column(TypeName = "nvarchar(max)")]
        public string Url { get; set; }

        //Decimal değerler için kaç karakter olup, virgülden sonra kaç karakter alacağını belirtmek için kullanılır
        //################.## gibi
        [Precision(18,2)]
        public decimal Price { get; set; }

        //İndirimli Fiyat
        [Precision(18, 2)]
        public decimal DiscountPrice { get; set; }

        public int Stock { get; set; }

        //MotMapped attribute ü ilgili property nin db de oluşmamasını sağlamak için kullanılır
        [NotMapped]
        public int Test { get; set; }
      
        public int Barcode { get; set; }

        //Her products ın bir kategorisi olacak
        public int? CategoryId { get; set; }

        //Navigation property
        //Category Parent property
        //LazyLoading kullanılması durumunda virtual eklenmelidir.
        //public virtual Category Category { get; set; }
        public Category Category { get; set; }

        //ProductFeature bire-bir ilişki kurmak için kullanılır
        //LazyLoading kullanılması durumunda virtual eklenmelidir.
        //public virtual ProductFeature ProductFeature { get; set; }
        public ProductFeature ProductFeature { get; set; }
    }
}
