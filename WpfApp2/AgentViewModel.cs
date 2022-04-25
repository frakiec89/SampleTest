using System.ComponentModel.DataAnnotations;


namespace WpfApp2
{
    public class AgentViewModel
    {
        [Key]
        public int AgentViewModelId { get; set; }

        public string ImagePath { get; set; }
        public string TypeName { get; set; }
        public string NameCompany { get; set; }
        public int Sale { get; set; }
        public string Telefone { get; set; }
        public double Priority { get; set; }

        public double Discount { get; set; }
    }
}
