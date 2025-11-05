using MediatR;
using PAIFGAMES.FCG.Domain.Mediator.Notifications;

namespace PAIFGAMES.FCG.Domain.Mediator
{
    public abstract class CommandHandler
    {
        protected readonly IMediatorHandler _mediator;
        protected readonly DomainNotificationHandler _notifications;

        protected CommandHandler(IMediatorHandler mediator, INotificationHandler<DomainNotification> notifications)
        {
            _mediator = mediator;
            _notifications = (DomainNotificationHandler)notifications;
        }
    }
}
