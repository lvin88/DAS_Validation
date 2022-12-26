using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace DAS_Validation.Models.Dto
{
    public class BatchValidationRequestDTO
    {
        public string UserID { get; set; }
        public List<string> BarcodeList { get; set; }
    }
}
