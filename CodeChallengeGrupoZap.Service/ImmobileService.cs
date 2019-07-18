using System;
using System.Collections.Generic;
using CodeChallengeGrupoZap.Domain.Entities;
using CodeChallengeGrupoZap.Repository;

namespace CodeChallengeGrupoZap.Service
{
    public class ImmobileService : IImmobileService
    {
        private readonly IImmobileRepository _immobileRepository;
        private Func<Immobile, bool> _true = (Immobile i) => { return true; };
        private Func<Immobile, bool> _latAndLonDifferentZero = (Immobile i) => { return i.Address.GeoLocation.Location.Lat != 0 && i.Address.GeoLocation.Location.Lon != 0; };
        private Func<Immobile, bool> _usableAreasGreaterThanZero = (Immobile i) => { return i.UsableAreas > 0; };
        private Func<Immobile, bool> _saleOrRental = (Immobile i) => { return i.PricingInfos.BusinessType == "SALE"; };
        private Func<Immobile, bool> _isBoundingBoxGrupoZap = (Immobile i) => { return isBoundingBoxGrupoZap(i.Address.GeoLocation.Location); };
        private Func<Immobile, bool> _minimumValue = (Immobile i) => { return  i.UsableAreas > 3500; };
        private Func<Immobile, bool> _minimumValueMinusTenPercent = (Immobile i) => { return  i.UsableAreas > 3150; };
        private Func<Immobile, bool> _minimumRentalPriceZap = (Immobile i) => { return  i.PricingInfos.RentalTotalPrice >= 3500; };
        private Func<Immobile, bool> _minimumSalePriceZap = (Immobile i) => { return  i.PricingInfos.Price >= 600000; };
        private Func<Immobile, bool> _monthlyFeeCondominiumIsValid = (Immobile i) => { return  i.PricingInfos.MonthlyCondoFee is int; };
        private Func<Immobile, bool> _condoThirtyLessthanRent = (Immobile i) => { return  i.PricingInfos.MonthlyCondoFee < i.PricingInfos.RentalTotalPrice * 0.3; };
        private Func<Immobile, bool> _condoThirtyLessthanRentPlusFifty = (Immobile i) => { return  i.PricingInfos.MonthlyCondoFee < (i.PricingInfos.RentalTotalPrice * 0.3) * 1.5; };
        private Func<Immobile, bool> _maximumRentalPriceVivareal = (Immobile i) => { return  i.PricingInfos.RentalTotalPrice <= 4000; };
        private Func<Immobile, bool> _maximumSalePriceVivareal = (Immobile i) => { return  i.PricingInfos.Price <= 700000; };
        
        public ImmobileService(IImmobileRepository immobileRepository)
        {
            _immobileRepository = immobileRepository;
        }

        public IList<Immobile> FilterByZap()
        {
            INode leaf = new Node(_true, null);
            INode minimumRentalPriceZap = new Node(_minimumRentalPriceZap, leaf);
            INode minimumSalePriceZap = new Node(_minimumSalePriceZap, leaf);
            INode minimumValue = new Node(_minimumValue, minimumSalePriceZap);
            INode minimumValueMinusTenPercent = new Node(_minimumValueMinusTenPercent, minimumSalePriceZap);
            INode isBoundingBoxGrupoZap = new DoubleNode(_isBoundingBoxGrupoZap, minimumValueMinusTenPercent, minimumValue);
            INode usableAreasGreaterThanZero = new Node(_usableAreasGreaterThanZero, isBoundingBoxGrupoZap);
            INode saleOrRental = new DoubleNode(_saleOrRental, usableAreasGreaterThanZero, minimumRentalPriceZap);
            INode latAndLonDifferentZero = new Node(_latAndLonDifferentZero, saleOrRental);
            INode root = new Node(_true, latAndLonDifferentZero);

            IList<Immobile> properties = _immobileRepository.Properties;
            IList<Immobile> filteredProperties = PerformFilter(properties, root);

            return filteredProperties;
        }

        public IList<Immobile> FilterByVivareal()
        {
            INode leaf = new Node(_true, null);
            INode maximumRentalPriceVivareal = new Node(_maximumRentalPriceVivareal, leaf);
            INode condoThirtyLessthanRent = new Node(_condoThirtyLessthanRent, maximumRentalPriceVivareal);
            INode condoThirtyLessthanRentPlusFifty = new Node(_condoThirtyLessthanRentPlusFifty, maximumRentalPriceVivareal);
            INode isBoundingBoxGrupoZap = new DoubleNode(_isBoundingBoxGrupoZap, condoThirtyLessthanRentPlusFifty, condoThirtyLessthanRent);
            INode monthlyFeeCondominiumIsValid = new Node(_monthlyFeeCondominiumIsValid, isBoundingBoxGrupoZap);
            INode maximumSalePriceVivareal = new Node(_maximumSalePriceVivareal, leaf);
            INode saleOrRental = new DoubleNode(_saleOrRental, maximumSalePriceVivareal, monthlyFeeCondominiumIsValid);
            INode latAndLonDifferentZero = new Node(_latAndLonDifferentZero, saleOrRental);
            INode root = new Node(_true, latAndLonDifferentZero);

            IList<Immobile> properties = _immobileRepository.Properties;
            IList<Immobile> filteredProperties = PerformFilter(properties, root);

            return filteredProperties;
        }

        private IList<Immobile> PerformFilter(IList<Immobile> propperties, INode root)
        {
            IList<Immobile> filteredProperties = new List<Immobile>();

            foreach (Immobile immobile in propperties)
            {
                if(root.Filter(immobile))
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