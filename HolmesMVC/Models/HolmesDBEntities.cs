using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using HolmesMVC.Models.Mapping;

namespace HolmesMVC.Models
{
    public partial class HolmesDBEntities : DbContext
    {
        static HolmesDBEntities()
        {
            Database.SetInitializer<HolmesDBEntities>(null);
        }

        public HolmesDBEntities()
            : base("Name=HolmesDB")
        {
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Adaptation> Adaptations { get; set; }
        public DbSet<Appearance> Appearances { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Honorific> Honorifics { get; set; }
        public DbSet<Medium> Media { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Rename> Renames { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<HolmesLink> HolmesLinks { get; set; }
        public DbSet<HolmesLinkAppearance> HolmesLinkAppearances { get; set; }
        public DbSet<HolmesLinkActor> HolmesLinkActors { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public DbSet<webpages_Roles> webpages_Roles { get; set; }
        public DbSet<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ActorMap());
            modelBuilder.Configurations.Add(new AdaptationMap());
            modelBuilder.Configurations.Add(new AppearanceMap());
            modelBuilder.Configurations.Add(new CharacterMap());
            modelBuilder.Configurations.Add(new DateMap());
            modelBuilder.Configurations.Add(new EpisodeMap());
            modelBuilder.Configurations.Add(new HonorificMap());
            modelBuilder.Configurations.Add(new MediumMap());
            modelBuilder.Configurations.Add(new ReferenceMap());
            modelBuilder.Configurations.Add(new RenameMap());
            modelBuilder.Configurations.Add(new SeasonMap());
            modelBuilder.Configurations.Add(new SpeciesMap());
            modelBuilder.Configurations.Add(new StoryMap());
            modelBuilder.Configurations.Add(new HolmesLinkMap());
            modelBuilder.Configurations.Add(new HolmesLinkAppearanceMap());
            modelBuilder.Configurations.Add(new HolmesLinkActorMap());
            modelBuilder.Configurations.Add(new UserProfileMap());
            modelBuilder.Configurations.Add(new webpages_MembershipMap());
            modelBuilder.Configurations.Add(new webpages_OAuthMembershipMap());
            modelBuilder.Configurations.Add(new webpages_RolesMap());
            modelBuilder.Configurations.Add(new webpages_UsersInRolesMap());
        }
    }
}
