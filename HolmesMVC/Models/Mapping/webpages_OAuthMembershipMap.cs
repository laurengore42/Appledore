using System.Data.Entity.ModelConfiguration;

namespace HolmesMVC.Models.Mapping
{
    public class Webpages_OAuthMembershipMap : EntityTypeConfiguration<Webpages_OAuthMembership>
    {
        public Webpages_OAuthMembershipMap()
        {
            // Primary Key
            HasKey(t => new { t.Provider, t.ProviderUserId });

            // Properties
            Property(t => t.Provider)
                .IsRequired()
                .HasMaxLength(30);

            Property(t => t.ProviderUserId)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("webpages_OAuthMembership");
            Property(t => t.Provider).HasColumnName("Provider");
            Property(t => t.ProviderUserId).HasColumnName("ProviderUserId");
            Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
