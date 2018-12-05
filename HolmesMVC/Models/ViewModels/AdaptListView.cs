using System.Collections.Generic;
using HolmesMVC.Extensions;

namespace HolmesMVC.Models.ViewModels
{
    public class AdaptListView
    {
        public AdaptListView(HolmesDBEntities Db)
        {
            AdaptsFilm = Db.Adaptations.AdaptsFilm().ToAdaptListAdapts();

            AdaptsSingleFilm = Db.Adaptations.AdaptsSingleFilm().ToAdaptListAdapts();

            AdaptsRadio = Db.Adaptations.AdaptsRadio().ToAdaptListAdapts();

            AdaptsSingleRadio = Db.Adaptations.AdaptsSingleRadio().ToAdaptListAdapts();

            AdaptsTV = Db.Adaptations.AdaptsTV().ToAdaptListAdapts();

            AdaptsOther = Db.Adaptations.AdaptsOther().ToAdaptListAdapts();
        }

        public List<AdaptListAdapt> AdaptsFilm { get; private set; }

        public List<AdaptListAdapt> AdaptsSingleFilm { get; private set; }

        public List<AdaptListAdapt> AdaptsRadio { get; private set; }

        public List<AdaptListAdapt> AdaptsSingleRadio { get; private set; }

        public List<AdaptListAdapt> AdaptsTV { get; private set; }

        public List<AdaptListAdapt> AdaptsOther { get; private set; }
    }

    public class AdaptListAdapt
    {
        public int ID;

        public string Name;

        public string Translation;

        public string Holmes;

        public int Year;

        public int EpCount;

        public string Medium;

        public string UrlName;
    }
}