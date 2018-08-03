using System;
using System.Collections.Generic;

namespace HolmesMVC.Models
{
    public partial class Webpages_OAuthMembership
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public int UserId { get; set; }
    }
}
