using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace TicketBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger<TicketController> _logger;
    private List<Ticket> _tickets = new List<Ticket>();

    public TicketController(ILogger<TicketController> logger)
    {
        _logger = logger;
    }


    [HttpGet("GetTicket", Name = "GetTicket")]
    public async Task<string> GetTicket()
    {
        return System.IO.File.ReadAllText("ticketDB.json");
    }


    [HttpGet("CompleteTicket", Name = "CompleteTicket")]
    public async Task CompleteTicket(string? guid)
    {
        var jsonString = System.IO.File.ReadAllText("ticketDB.json");
        List<Ticket> jsonObject = JsonConvert.DeserializeObject<List<Ticket>>(jsonString);

        jsonObject.RemoveAll(obj => obj.Guid == guid);
        jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

        System.IO.File.WriteAllText("ticketDB.json", jsonString);

        Console.WriteLine(jsonObject);
    }


    [HttpPost("MakeTicket", Name = "MakeTicket")]
    public async Task MakeTicket(string? from, string? to, string? subject, string? description, string? priority)
    {
        Ticket newTicket = new Ticket
        {
            Guid = Guid.NewGuid().ToString(),
            From = from,
            To = to,
            Subject = subject,
            Description = description,
            Priority = priority
        };
        string json = JsonConvert.SerializeObject(newTicket, Formatting.Indented);

        var fileValue = System.IO.File.ReadAllText("ticketDB.json");

        if (!String.IsNullOrEmpty(fileValue) && fileValue != "[]")
        {
            fileValue = fileValue.Replace("]", ",");
            fileValue = fileValue + json;
            System.IO.File.WriteAllText("ticketDB.json", fileValue + "]");
        }
        else
        {
            System.IO.File.WriteAllText("ticketDB.json", "[" + json + "]");
        }
    }
}

public class Ticket
{
    public string Guid { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
}