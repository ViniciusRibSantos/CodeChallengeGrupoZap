namespace CodeChallengeGrupoZap.Domain.Entities
{
    public class Address
    {
        public string City { get; set; }
        public string Neighborhood { get; set; }
        public GeoLocation GeoLocation { get; set; }
    }
}