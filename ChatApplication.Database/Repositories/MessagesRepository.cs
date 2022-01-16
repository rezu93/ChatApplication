using ChatApplication.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChatApplication.Database
{
    public class MessagesRepository : IMessagesRepository
    {
        private ApplicationDbContext _dbContext;

        private DbSet<MessageEntity> Messages { get; set; }


        public MessagesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Messages = dbContext.Messages;
        }

        public List<MessageEntity> GetAll()
        {
            return Messages.ToList();
        }

        public bool Add(MessageEntity message)
        {
            Messages.Add(message);

            return _dbContext.SaveChanges() > 0;

        }

        public bool Delete(MessageEntity message)
        {
            Messages.Remove(message);

            return _dbContext.SaveChanges() > 0;
        }

    }
}
