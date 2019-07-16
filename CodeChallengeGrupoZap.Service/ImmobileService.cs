using System;
using System.Collections.Generic;
using System.Linq;
using CodeChallengeGrupoZap.Domain.Entities;
using CodeChallengeGrupoZap.Repository;

namespace CodeChallengeGrupoZap.Service
{
    public class ImmobileService : IImmobileService
    {
        private readonly IImmobileRepository _immobileRepository;
        private IDictionary<int, Func<Immobile, bool>> filters;
        private IDictionary<int, Func<Immobile, bool>> salesFilters;
        private IDictionary<int, Func<Immobile, bool>> rentalFilters;
        private Func<Immobile, bool> latAndLonAreZero = 
            (Immobile i) => { return i.Address.GeoLocation.Location.Lat == 0 && i.Address.GeoLocation.Location.Lon == 0; };
        private Func<Immobile, bool> usableAreasGreaterThanZero = (Immobile i) => { return i.UsableAreas > 0; };

        public ImmobileService(IImmobileRepository immobileRepository)
        {
            _immobileRepository = immobileRepository;
        }
        public IEnumerable<Immobile> FilterByZap()
        {
            filters = new Dictionary<int, Func<Immobile, bool>>();
            salesFilters = new Dictionary<int, Func<Immobile, bool>>();
            rentalFilters = new Dictionary<int, Func<Immobile, bool>>();

            filters.Add(1, latAndLonAreZero);

            IList<Immobile> properties = _immobileRepository.Properties;
            IList<Immobile> filteredProperties = PerformFilter(properties);

            return filteredProperties;
        }

        public IEnumerable<Immobile> FilterByVivareal()
        {
            filters = new Dictionary<int, Func<Immobile, bool>>();
            salesFilters = new Dictionary<int, Func<Immobile, bool>>();
            rentalFilters = new Dictionary<int, Func<Immobile, bool>>();

            filters.Add(1, latAndLonAreZero);

            IList<Immobile> properties = _immobileRepository.Properties;
            IList<Immobile> filteredProperties = PerformFilter(properties);

            return filteredProperties;
        }

        private IList<Immobile> PerformFilter(IList<Immobile> propperties)
        {
            IList<Immobile> filteredProperties = new List<Immobile>();

            foreach (Immobile immobile in propperties)
            {
                if(PerformFilter(immobile, filters, 0))
                {
                   if(FilterByBusinessType(immobile, immobile.PricingInfos.BusinessType))
                   {
                       filteredProperties.Add(immobile);
                   }
                }
            }

            return filteredProperties;
        }

        private bool FilterByBusinessType(Immobile immobile, string businessType)
        {
            if(businessType.Equals("SALES"))
            {
                return PerformFilter(immobile, salesFilters, 0);
            }
            else if (businessType.Equals("RENTAL"))
            {
                return PerformFilter(immobile, rentalFilters, 0);
            }
            else
            {
                return false;
            }
        }

        private bool PerformFilter(Immobile immobile, IDictionary<int, Func<Immobile, bool>> filters, int counter)
        {
            if(filters[counter](immobile))
            {
                return counter < filters.Count ? PerformFilter(immobile, filters, counter + 1) : true;
            }
            else
            {
                return false;
            }
        }
    }
}