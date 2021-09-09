using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Events.Data.Extension
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StarDateTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Author { get; set; }
        public string Location { get; set; }

        public static Expression<Func<Event, EventViewModel>> ViewModel
        {
            get
            {
                return e => new EventViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StarDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Author = e.Author.FullName,
                    Location = e.Location
                };
            }

        }
    }
    public class UpPaEventsViewModel
    {
        public IEnumerable<EventViewModel> UpcomingEvents { get; set; }
        public IEnumerable<EventViewModel> PassedEvents { get; set; }
    }
}
