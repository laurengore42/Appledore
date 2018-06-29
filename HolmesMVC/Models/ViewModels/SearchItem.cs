namespace HolmesMVC.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public enum SearchItemType
    {
        Story,
        Actor,
        Character,
        Rename,
        Episode,
        Adapt
    }

    public class SearchData
    {
        public List<SearchStory> Stories { get; set; }

        public List<SearchActor> Actors { get; set; }

        public List<SearchChar> Characters { get; set; }

        public List<SearchRename> Renames { get; set; }

        public List<SearchEp> Episodes { get; set; }

        public List<SearchAdapt> Adaptations { get; set; }

        public string Query { get; set; }

        public bool NameListContainsQuery(string query, string[] names)
        {
            if (string.IsNullOrEmpty(query))
            {
                return false;
            }

            // turn %20 into ' '
            query = HttpUtility.HtmlDecode(query).ToLower().Replace("\"", string.Empty);

            // join names
            var nameString = string.Join(" ", names).ToLower();

            // process for accent marks
            nameString = Shared.NormaliseString(nameString);
            query = Shared.NormaliseString(query);

            return nameString.Contains(query);
        }

        public void DoSearch(string query)
        {
            Stories = (from s in Stories
                      where NameListContainsQuery(
                            query,
                            new[] {
                                    s.ID,
                                    s.Name
                                })
                      select s).ToList();

            Actors = (from a in Actors
                      orderby a.Surname
                      where NameListContainsQuery(
                           query,
                           new[] {
                                    // shortname
                                    Shared.BuildName(new[] { a.Forename, a.Surname }, ' '),

                                    // longname
                                    Shared.BuildName(new[] { a.Forename, a.Middlenames, a.Surname }, ' '),

                                    // displayname
                                    (a.Surname + 
                                    (!string.IsNullOrWhiteSpace(a.Forename)
                                        ? ", " + a.Forename
                                        : string.Empty))
                                })
                       select a).ToList();

            Characters = (from c in Characters
                          orderby c.Surname
                       where NameListContainsQuery(
                            query,
                            new[] {
                                    // shortname
                                    Shared.BuildName(new[] { string.IsNullOrWhiteSpace(c.Forename) ? c.Honorific : c.Forename, c.Surname }, ' '),
                                    
                                    // longname
                                    Shared.BuildName(new[] { c.Honorific, c.Forename, c.Surname }, ' '),

                                    // displayname
                                    ((string.IsNullOrWhiteSpace(c.Forename) && string.IsNullOrWhiteSpace(c.Honorific))
                                            ? c.Surname
                                            : c.Surname + ", " + c.Honorific + " " + c.Forename)
                                })
                       select c).ToList();

            Renames = (from c in Renames
                       orderby c.Surname
                       where NameListContainsQuery(
                            query,
                            new[] {
                                    // shortname
                                    Shared.BuildName(new[] { string.IsNullOrWhiteSpace(c.Forename) ? c.Honorific : c.Forename, c.Surname }, ' ').ToLower(),
                                    
                                    // longname
                                    Shared.BuildName(new[] { c.Honorific, c.Forename, c.Surname }, ' ').ToLower(),

                                    // displayname
                                    ((string.IsNullOrWhiteSpace(c.Forename) && string.IsNullOrWhiteSpace(c.Honorific))
                                            ? c.Surname
                                            : c.Surname + ", " + c.Honorific + " " + c.Forename).ToLower()
                                })
                       select c).ToList();

            Episodes = (from e in Episodes
                        orderby e.Year
                        where NameListContainsQuery(
                              query,
                              new[] {
                                    e.Name,
                                    e.Translation
                                })
                        select e).ToList();

            Adaptations = (from a in Adaptations
                           orderby a.Year
                      where NameListContainsQuery(
                            query,
                            new[] {
                                    a.Name,
                                    a.Translation
                                })
                      select a).ToList();
            
            Query = query;
        }

        // get only the necessary data from the database
        // in trying to cut down on the number of SQL statements linq runs, I have done horrible things
        public void Populate(HolmesDBEntities db)
        {
            var holmesId = Shared.GetHolmes();
            var watsonId = Shared.GetWatson();

            Stories = (from s in db.Stories
                       select new SearchStory
                                                       {
                                                           ID = s.ID, 
                                                           Name = s.Name.Replace("\"", string.Empty).Replace("\\" + "\"", string.Empty)
                                                       }).ToList();

            Actors = (from a in db.Actors
                      select new SearchActor

                                                     {
                                                         ID = a.ID,
                                                         Forename = a.Forename,
                                                         Middlenames = a.Middlenames,
                                                         Surname = a.Surname
                                                     }).ToList();

            Characters = (from c in db.Characters 
                          select new SearchChar
                                                             {
                                                                 ID = c.ID,
                                                                 Honorific = c.Honorific == null ? string.Empty : c.Honorific1.Name, 
                                                                 Forename = c.Forename,
                                                                 Surname = c.Surname
                                                             }).ToList();

            var renameQuery = from r in db.Renames 
                       select new SearchRename
                                                       {
                                                           Honorific = r.Honorific == null ? string.Empty : r.Honorific1.Name, 
                                                           Forename = r.Forename,
                                                           Surname = r.Surname, 
                                                           AdaptId = r.Adaptation, 
                                                           AdaptTranslation = r.Adaptation1.Translation,
                                                           AdaptName = (

                                                           // oh my god
                                                           !(null == r.Adaptation1.Name || string.Empty == r.Adaptation1.Name)
                            ? r.Adaptation1.Name.Replace("\"", string.Empty)
                            : (r.Adaptation1.Seasons.Count() == 1 && r.Adaptation1.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                ? (null == r.Adaptation1.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                   || string.Empty == r.Adaptation1.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? r.Adaptation1.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story1.Name
                                    : r.Adaptation1.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                : (r.Adaptation1.Company + " " + (r.Adaptation1.Medium1.Name == "Television" ? "TV" : r.Adaptation1.Medium1.Name))

                                                            ).Replace("\"", string.Empty),

                                                           Character = r.Character, 
                                                           OldName =
                                                                (r.Character1.Honorific == null ? string.Empty : r.Character1.Honorific1.Name + " ")
                                                                + (r.Character1.Forename == null || r.Character1.Forename == string.Empty ? string.Empty : r.Character1.Forename + " ")
                                                                + r.Character1.Surname
                                                       };
            var stringit = renameQuery.ToString();
            Renames = renameQuery.ToList();

            var episodeQuery = from e in db.Episodes 
                               where (e.Season1.Adaptation1.Name == null || e.Season1.Adaptation1.Name != "Canon") // these are 'Stories'
                                    && e.Season1.Adaptation1.Medium1.Name != "Stage" // to_do_theatre
                                    && e.Season1.Adaptation1.Seasons.SelectMany(s => s.Episodes).Count() > 1 // exclude single-film, put those in 'Adapts'
                               select new SearchEp
                                                         {
                                                             ID = e.ID, 
                                                             Year = e.Airdate.Year,
                                                             EpCount = e.Season1.Adaptation1.Seasons.SelectMany(s => s.Episodes).Count(),
                                                             Medium = e.Season1.Adaptation1.Medium1.Name,
                                                             Holmes = (from ap in e.Appearances
                                                                           where ap.Character == holmesId
                                                                           select ap).Any()
                                                                           ? (from ap in e.Appearances
                                                                              where ap.Character == holmesId
                                                                              group ap by ap.Actor into grp
                                                                              orderby grp.Count() descending 
                                                                              select grp.FirstOrDefault().Actor1.Surname).FirstOrDefault()
                                                                           : string.Empty,
                                                             Translation = (e.Translation == null || e.Translation == string.Empty || e.Translation.Length == 0)
                                                                            ? string.Empty
                                                                            : e.Translation.Replace("\"", string.Empty),
                                                             Name = (
                                                                     (e.Title == null || e.Title == string.Empty || e.Title.Length == 0)
                                                                      ? (e.Story == null || e.Story == string.Empty || e.Story.Length == 0)
                                                                         ? string.Empty
                                                                         : e.Story1.Name
                                                                      : e.Title
                                                                    ).Replace("\"", string.Empty).Replace("\\" + "\"", string.Empty)
                                                         };
            var stringit2 = episodeQuery.ToString();
            Episodes = episodeQuery.ToList();

            var adaptQuery = from a in db.Adaptations
                             where (a.Name == null || a.Name != "Canon") 
                            && a.Medium1.Name != "Stage" // to_do_theatre
                            && a.Seasons.Any()
                            && a.Seasons.FirstOrDefault().Episodes.Any()
                            select new SearchAdapt
                            {
                                ID = a.ID,
                                Year = a.Seasons.OrderBy(s => s.AirOrder).FirstOrDefault().Episodes.OrderBy(e => e.Airdate).FirstOrDefault().Airdate.Year,
                                EpCount = a.Seasons.SelectMany(s => s.Episodes).Count(),
                                Medium = a.Medium1.Name,
                                
                                                             Holmes = (from ap in a.Seasons.SelectMany(s => s.Episodes).SelectMany(e => e.Appearances)
                                                                           where ap.Character == holmesId
                                                                           select ap).Any()
                                                                           ? (from ap in a.Seasons.SelectMany(s => s.Episodes).SelectMany(e => e.Appearances)
                                                                              where ap.Character == holmesId
                                                                              group ap by ap.Actor into grp
                                                                              orderby grp.Count() descending
                                                                              select grp.FirstOrDefault().Actor1.Surname).FirstOrDefault()
                                                                           : string.Empty,
                                Translation = (a.Translation == null || a.Translation == string.Empty || a.Translation.Length == 0)
                                               ? string.Empty
                                               : a.Translation.Replace("\"", string.Empty),


                                // have to do all this here,
                                // can't call Shared.DisplayName() inside a linq to entities call
                                Name = (
                                !(null == a.Name || string.Empty == a.Name)
                            ? a.Name.Replace("\"", string.Empty)
                            : (a.Seasons.Count() == 1 && a.Seasons.FirstOrDefault().Episodes.Count() == 1)
                                ? (null == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                   || string.Empty == a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title)
                                    ? a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Story1.Name
                                    : a.Seasons.FirstOrDefault().Episodes.FirstOrDefault().Title
                                : (a.Company + " " + (a.Medium1.Name == "Television" ? "TV" : a.Medium1.Name))
                                ).Replace("\"", string.Empty)
                            };
            var stringit3 = adaptQuery.ToString();
            Adaptations = adaptQuery.ToList();
        }
    }
    
    public class SearchStory
    {
        public string ID { get; set; }

        public string Name { get; set; }
    }

    public class SearchActor
    {
        public int ID { get; set; }

        public string Forename { get; set; }

        public string Middlenames { get; set; }

        public string Surname { get; set; }
    }

    public class SearchChar
    {
        public int ID { get; set; }

        public string Honorific { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }
    }

    public class SearchRename
    {
        public int Character { get; set; }

        public string OldName { get; set; }

        public int AdaptId { get; set; }

        public string AdaptName { get; set; }

        public string AdaptTranslation { get; set; }

        public string Honorific { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }
    }

    public class SearchAdapt
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Translation { get; set; }

        public int Year { get; set; }

        public string Holmes { get; set; }

        public string Medium { get; set; }

        public int EpCount { get; set; }
    }
    
    public class SearchEp
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Translation { get; set; }

        public int Year { get; set; }

        public string Holmes { get; set; }

        public string Medium { get; set; }

        public int EpCount { get; set; }
    }
}