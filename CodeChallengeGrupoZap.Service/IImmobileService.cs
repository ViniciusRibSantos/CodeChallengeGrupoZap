using System.Collections.Generic;
using CodeChallengeGrupoZap.Domain.Entities;

namespace CodeChallengeGrupoZap.Service
{
    public interface IImmobileService
    {
        IList<Immobile> FilterByZap();
        IList<Immobile> FilterByVivareal();
    }
}