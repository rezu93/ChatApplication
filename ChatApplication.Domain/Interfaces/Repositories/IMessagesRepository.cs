using System.Collections.Generic;

namespace ChatApplication.Domain
{
    public interface IMessagesRepository
    {
        List<MessageEntity> GetAll();
        bool Add(MessageEntity message);
        bool Delete(MessageEntity message);
        
    }
}