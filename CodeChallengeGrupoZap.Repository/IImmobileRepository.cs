using System.Collections.Generic;
using CodeChallengeGrupoZap.Domain.Entities;

namespace CodeChallengeGrupoZap.Repository
{
    public interface IImmobileRepository
    {
        IList<Immobile> Properties { get; set; }
    }
}