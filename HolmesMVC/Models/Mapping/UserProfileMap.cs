namespace HolmesMVC.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserProfile");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.PreferredCanonOrder).HasColumnName("PreferredCanonOrder");
        }
    }
}
