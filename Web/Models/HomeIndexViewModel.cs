
using System.Collections.Generic;
using Web.Models;

namespace Web.Data
{
    public class HomeIndexViewModel
    {
        public Profile CurrentUser { get; set; }
        public List<UserProfile> Suggestions { get; set; }
        public List<FeedItem> FeedItems { get; set; }
        
    }
  
}