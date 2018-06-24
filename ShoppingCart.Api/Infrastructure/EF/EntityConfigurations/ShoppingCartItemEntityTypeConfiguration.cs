//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace ShoppingCart.Api.Infrastructure.EF.EntityConfigurations
//{
//    public class ShoppingCartItemEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingCart.Api.Models.ShoppingCartItem>
//    {
//        public void Configure(EntityTypeBuilder<ShoppingCart.Api.Models.ShoppingCartItem> builder)
//        {
//            builder.ToTable("ShoppingCartItem");

//            builder.HasKey(ci => ci.Id);


//            builder.Property(cb => cb.ProductName)
//                .IsRequired()
//                .HasMaxLength(30);

//            builder.Property(cb => cb.Desscription)
//                .IsRequired()
//                .HasMaxLength(100);
//        }
//    }
//}
