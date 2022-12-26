using DAS_Validation.Data;
using DAS_Validation.Models;
using DAS_Validation.Models.Dto;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using DAS_Validation.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace DAS_Validation.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _db;
        string key = "test123";

        public TicketRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ValidationResponseDTO> Validate(ValidationRequestDTO validationRequestDTO)
        {
            string url = "";
            string userId = validationRequestDTO.UserID;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-DynamicsApiKey", key);
            client.DefaultRequestHeaders.Add("X-User-Id", userId);

            var parameters = new Dictionary<string, string> { { "barcode", validationRequestDTO.Barcode } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            var response = await client.PostAsync(url, encodedContent);

            string responseContent = await response.Content.ReadAsStringAsync();

            dynamic jsonData = JObject.Parse(responseContent);

            ValidationResponseDTO validationResponseDTO = new();

            if (jsonData.success == "true")
            {
                if (jsonData.data.error.ToString() == "")
                {
                    Ticket ticketInfo = new()
                    {
                        UserID = userId,
                        Barcode = jsonData.data.validationItems[0].barcode,
                        PrizeAmount = jsonData.data.validationItems[0].prizeAmount / 100,
                        TaxAmount = jsonData.data.validationItems[0].taxAmount / 100,
                        ValidationResult = jsonData.data.validationItems[0].validationResult,
                        ValidationDate = DateTime.Now,
                    };

                    await _db.AddAsync(ticketInfo);
                    await _db.SaveChangesAsync();

                    TicketDTO ticketDTO = new()
                    {
                        UserID = ticketInfo.UserID,
                        Barcode = ticketInfo.Barcode,
                        PrizeAmount = ticketInfo.PrizeAmount,
                        TaxAmount = ticketInfo.TaxAmount,
                        ValidationResult = ticketInfo.ValidationResult
                    };

                    validationResponseDTO.TicketDTO = ticketDTO;

                    return validationResponseDTO;
                }

                validationResponseDTO.Error = new
                {
                    code = jsonData.data.error.code.ToString(),
                    message = jsonData.data.error.message.ToString()
                };

                return validationResponseDTO;
            }

            validationResponseDTO.Error = new
            {
                code = jsonData.error.code.ToString(),
                message = jsonData.error.message.ToString()
            };

            return validationResponseDTO;
        }

        public async Task<BatchValidationResponseDTO> ValidateBatch(BatchValidationRequestDTO batchValidationRequestDTO)
        {
            string url = "";

            string userId = batchValidationRequestDTO.UserID;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-DynamicsApiKey", key);
            client.DefaultRequestHeaders.Add("X-User-Id", userId);

            string bList = String.Join(",", batchValidationRequestDTO.BarcodeList);

            var parameters = new Dictionary<string, string> { { "barcodeList", bList } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            var response = await client.PostAsync(url, encodedContent);

            string responseContent = await response.Content.ReadAsStringAsync();

            dynamic jsonData = JObject.Parse(responseContent);

            BatchValidationResponseDTO batchValidationResponseDTO = new();

            if (jsonData.success == "true")
            {
                if (jsonData.data.error.ToString() == "")
                {
                    List<Ticket> ticketInfoList = new();

                    for (int j = 0; j < jsonData.data.validationItems.Count; j++)
                    {
                        Ticket ticketInfo = new()
                        {
                            UserID = userId.ToString(),
                            Barcode = jsonData.data.validationItems[j].barcode.ToString(),
                            PrizeAmount = jsonData.data.validationItems[j].prizeAmount / 100,
                            TaxAmount = jsonData.data.validationItems[j].taxAmount / 100,
                            ValidationResult = jsonData.data.validationItems[j].validationResult.ToString(),
                            ValidationDate = DateTime.Now
                        };

                        ticketInfoList.Add(ticketInfo);

                        await _db.AddAsync(ticketInfo);
                    }
                    await _db.SaveChangesAsync();

                    List<TicketDTO> ticketListDTO = new();

                    for (int i = 0; i < ticketInfoList.Count; i++)
                    {
                        TicketDTO ticketDTO = new();

                        ticketDTO.UserID = ticketInfoList[i].UserID;
                        ticketDTO.Barcode = ticketInfoList[i].Barcode;
                        ticketDTO.PrizeAmount = ticketInfoList[i].PrizeAmount;
                        ticketDTO.TaxAmount = ticketInfoList[i].TaxAmount;
                        ticketDTO.ValidationResult = ticketInfoList[i].ValidationResult;

                        ticketListDTO.Add(ticketDTO);
                    }

                    batchValidationResponseDTO.TicketDTO = ticketListDTO;

                    return batchValidationResponseDTO;
                }

                batchValidationResponseDTO.Error = new
                {
                    code = jsonData.data.error.code.ToString(),
                    message = jsonData.data.error.message.ToString()
                };

                return batchValidationResponseDTO;
            }

            batchValidationResponseDTO.Error = new
            {
                code = jsonData.error.code.ToString(),
                message = jsonData.error.message.ToString()
            };

            return batchValidationResponseDTO;
        }
    }
}
