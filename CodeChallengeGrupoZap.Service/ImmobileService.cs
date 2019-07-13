using System.Collections.Generic;
using CodeChallengeGrupoZap.Domain.Entities;
using CodeChallengeGrupoZap.Repository;

namespace CodeChallengeGrupoZap.Service
{
    public class ImmobileService : IImmobileService
    {
        private readonly IImmobileRepository _immobileRepository;

        public ImmobileService(IImmobileRepository immobileRepository)
        {
            _immobileRepository = immobileRepository;
        }
        public IEnumerable<Immobile> FilterByZap()
        {
            IEnumerable<Immobile> properties = _immobileRepository.Properties;
            
            return null;
        }
    }
}