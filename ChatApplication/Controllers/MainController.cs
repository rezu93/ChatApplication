using ChatApplication.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ChatApplication
{
    [ApiController]
    [Route("controller")]
    public class MainController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMessagesRepository _messagesRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IHubContext<MessageHubClient, IMessageHubClient> _messageHub;


        public MainController(IConfiguration configuration, 
                              IMessagesRepository messagesRepository,
                              IEmailService emailService,
                              ILogger logger,
                              IHubContext<MessageHubClient, IMessageHubClient> messageHub)
        {
            _configuration = configuration;
            _messagesRepository = messagesRepository;
            _emailService = emailService;
            _logger = logger;
            _messageHub = messageHub;
        }

        [Authorize("Administrator")]
        [Route("getSomeSecretData")]
        public IActionResult GetSomeSecretData()
        {
            return Ok("SomeSecretKey");
        }

        [HttpGet]
        [Route("getMessages")]
        public IActionResult GetMessages()
        {
            var messages = _messagesRepository.GetAll();

            var messagesDto = messages.Select(x => new MessageDto
            {
                Content = x.Content,
                Author = x.FirstNameAuthor + " " + x.LastNameAuthor
            });

            return Ok(messagesDto);
        }

        [HttpPost]
        [Route("sendMessage")]
        public IActionResult SendMessage([FromBody]MessageDto messageDto)
        {
            var messageEntity = new MessageEntity()
            {
                Content = messageDto.Content,
                FirstNameAuthor = messageDto.Author.Split(" ").First(),
                LastNameAuthor = messageDto.Author.Split(" ").Skip(1).First()
            };

            _emailService.SendEmail("saket.rock@alldrys.com",messageDto.Content);
            _logger.Log(messageDto.Content);

            var result = _messagesRepository.Add(messageEntity);

            if (result)
            {
                _messageHub.Clients.All.NewMessage().Wait();
                return Ok(messageDto);
            }

            return NotFound();
        }

    }
}
