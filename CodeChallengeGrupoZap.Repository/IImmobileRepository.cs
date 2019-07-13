using System.Collections.Generic;
using CodeChallengeGrupoZap.Domain.Entities;

namespace CodeChallengeGrupoZap.Repository
{
    public interface IImmobileRepository
    {
        IEnumerable<Immobile> Properties { get; set; }
    }
}