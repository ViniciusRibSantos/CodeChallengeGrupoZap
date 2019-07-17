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
        private Func<Immobile, bool> _true = (Immobile i) => { return true; };
        private Func<Immobile, bool> _latAndLonAreZero = (Immobile i) => { return i.Address.GeoLocation.Location.Lat == 0 && i.Address.GeoLocation.Location.Lon == 0; };
        private Func<Immobile, bool> _usableAreasGreaterThanZero = (Immobile i) => { return i.UsableAreas > 0; };
        private Func<Immobile, bool> _saleOrRental = (Immobile i) => { return i.PricingInfos.BusinessType == "SALE"; };
        private Func<Immobile, bool> _isBoundingBoxGrupoZap = (Immobile i) => { return isBoundingBoxGrupoZap(i.Address.GeoLocation.Location); };
        private Func<Immobile, bool> _minimumValue = (Immobile i) => { return  i.UsableAreas <= 3500; };
        private Func<Immobile, bool> _minimumValueMinusTenPercent = (Immobile i) => { return  i.UsableAreas <= 3150; };
        private Func<Immobile, bool> _minimumRentalPriceZap = (Immobile i) => { return  i.PricingInfos.RentalTotalPrice >= 3500; };
        private Func<Immobile, bool> _minimumSalePriceZap = (Immobile i) => { return  i.PricingInfos.Price >= 600000; };

        public ImmobileService(IImmobileRepository immobileRepository)
        {
            _immobileRepository = immobileRepository;
        }

        public IEnumerable<Immobile> FilterByZap()
        {
            INode minimumRentalPriceZap = new Node(_minimumRentalPriceZap, null);
            INode minimumValue = new Node(_minimumValue, minimumRentalPriceZap);
            INode minimumValueMinusTenPercent = new Node(_minimumValueMinusTenPercent, minimumRentalPriceZap);
            INode isBoundingBoxGrupoZap = new DoubleNode(_isBoundingBoxGrupoZap, minimumValueMinusTenPercent, minimumValue);
            INode usableAreasGreaterThanZero = new Node(_usableAreasGreaterThanZero, isBoundingBoxGrupoZap);
            INode minimumSalePriceZap = new Node(_minimumSalePriceZap, null);
            INode saleOrRental = new DoubleNode(_saleOrRental, minimumSalePriceZap, usableAreasGreaterThanZero);
            INode latAndLonAreZero = new Node(_latAndLonAreZero, saleOrRental);
            INode head = new Node(_true, latAndLonAreZero);
            
            IList<Immobile> properties = _immobileRepository.Properties;
            IList<Immobile> filteredProperties = PerformFilter(properties, head);

            return filteredProperties;
        }

        public IEnumerable<Immobile> FilterByVivareal()
        {
            return null;
        }

        private IList<Immobile> PerformFilter(IList<Immobile> propperties, INode head)
        {
            IList<Immobile> filteredProperties = new List<Immobile>();

            foreach (Immobile immobile in propperties)
            {
                if(head.Filter(immobile))
                    filteredProperties.Add(immobile);
            }

            return filteredProperties;
        }

        private static bool isBoundingBoxGrupoZap(Location location)
        {
            double minlon = -46.693419;
            double minlat = -23.568704;
            double maxlon = -46.641146;
            double maxlat = -23.546686;

            return isInRange(location.Lat, maxlat, minlat) && isInRange(location.Lon, maxlon, minlon);
        }

        private static bool isInRange(double value, double max, double min)
        {
            return value >= min && value <= max;
        }
    }
}