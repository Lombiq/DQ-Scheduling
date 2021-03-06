﻿using DQ.Scheduling.Models;
using DQ.Scheduling.ViewModels;
using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Html;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DQ.Scheduling.CalendarProviders {
    [OrchardFeature("DQ.SchedulingFullCalendar")]
    public class FullCalendarProvider : ICalendarProvider {
        private readonly IContentManager _contentManager;
        private readonly UrlHelper _urlHelper;

        public FullCalendarProvider(IContentManager contentManager, UrlHelper urlHelper) {
            _contentManager = contentManager;
            _urlHelper = urlHelper;
        }

        public string Name { get { return "Fullcalendar"; } }
        public bool SupportsAjax { get { return true; } }

        public IEnumerable<FormattedEvent> FormatCalendarEvents(IEnumerable<IContent> events) {
            var viewModels = new List<FullCalendarDisplayViewModel>();

            events.ToList().ForEach(ev => {
                var eventPart = ev.As<SchedulingPart>();

                var viewModel = new FullCalendarDisplayViewModel {
                    Id = ev.Id,
                    Title = _contentManager.GetItemMetadata(ev).DisplayText,
                    Start = eventPart.StartDateTime.GetValueOrDefault(),
                    End = eventPart.EndDateTime.GetValueOrDefault(),
                    Url = string.IsNullOrWhiteSpace(eventPart.DisplayUrlOverride) ? _urlHelper.ItemDisplayUrl(ev) : eventPart.DisplayUrlOverride,
                    AllDay = eventPart.IsAllDay,
                    Recurring = eventPart.IsRecurring // TODO: do something with the 'dow' of fullcalendar (for recurring events)
                };

                viewModels.Add(viewModel);
            });

            return viewModels;
        }
    }
}