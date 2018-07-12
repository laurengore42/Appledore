namespace HolmesMVC.Models.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            // Primary Key
            HasKey(t => t.UserId);

            // Properties
            // Table & Column Mappings
            ToTable("UserProfile");
            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.UserName).HasColumnName("UserName");
            Property(t => t.PreferredCanonOrder).HasColumnName("PreferredCanonOrder");
        }
    }
}
