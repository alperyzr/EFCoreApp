using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.CodeFirst.DataAccessLayer
{
    public class DbContextInitializer
    {

        //Appsettingsten okuma yapabilmek için kullanılır.
        public static IConfigurationRoot Configuration;

        //Veri Tabanı için optionsları belirtmek için kullanılır.
        public static DbContextOptionsBuilder<AppDbContext> OptionsBuilder;

        public static void Build()
        {
            //GetCurrenctDirecktory uygulamanın çalışmış olduğu klasörü alması için kullanılır.
            //AddJsonJile Ana dizindeki appsettings dosyamızı eklemek için kullanılır.
            //Optional olup olmadığı belirtilir.
            //ReloadOnChange bu dosya üzerinde her değişiklik yapıldığında tekrardan yüklenmesi için kullanılır.
            //Aynı zamanda appsettings dosyasına sağ tıklayıp, propertilerinden Copy to Output Directory kısmından Copy always seçeneği seçilmelidir.
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            //Okuyabileceğimiz dosyayı hazır hale getirir
            Configuration = builder.Build();



        }
    }
}
