using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models;
namespace Web.Data
{
    public class HomeIndexViewModel
    {
        public UserProfile CurrentUser { get; set; }
        public List<UserProfile> Suggestions { get; set; }
        public List<FeedItem> FeedItems { get; set; }
    }
  
}