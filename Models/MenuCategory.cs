using System.ComponentModel;

namespace RestautantMvc.Models
{
    public enum MenuCategory
    {
        [Description("Antipasti")]
        Antipasti = 1,

        [Description("Primi Piatti")]
        PrimiPiatti = 2,

        [Description("Secondi Piatti")]
        SecondiPiatti = 3,

        [Description("Pizza")]
        Pizza = 4,

        [Description("Dolci")]
        Dolci = 5
    }
}