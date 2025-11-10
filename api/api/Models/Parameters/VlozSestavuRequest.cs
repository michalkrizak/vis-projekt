namespace api.Models.Parameters
{
    public class VlozSestavuRequest
    {
        public int IdZapas { get; set; }
        public int IdTymDomaci { get; set; }
        public int IdTymHost { get; set; }
        public List<SestavaInput> Sestava { get; set; }
    }
}
