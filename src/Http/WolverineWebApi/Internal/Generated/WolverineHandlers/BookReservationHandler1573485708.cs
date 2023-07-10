// <auto-generated/>
#pragma warning disable
using Microsoft.Extensions.Logging;
using Wolverine.Marten.Publishing;

namespace Internal.Generated.WolverineHandlers
{
    // START: BookReservationHandler1573485708
    public class BookReservationHandler1573485708 : Wolverine.Runtime.Handlers.MessageHandler
    {
        private readonly Wolverine.Marten.Publishing.OutboxedSessionFactory _outboxedSessionFactory;
        private readonly Microsoft.Extensions.Logging.ILogger<WolverineWebApi.Reservation> _logger;

        public BookReservationHandler1573485708(Wolverine.Marten.Publishing.OutboxedSessionFactory outboxedSessionFactory, Microsoft.Extensions.Logging.ILogger<WolverineWebApi.Reservation> logger)
        {
            _outboxedSessionFactory = outboxedSessionFactory;
            _logger = logger;
        }



        public override async System.Threading.Tasks.Task HandleAsync(Wolverine.Runtime.MessageContext context, System.Threading.CancellationToken cancellation)
        {
            var bookReservation = (WolverineWebApi.BookReservation)context.Envelope.Message;
            await using var documentSession = _outboxedSessionFactory.OpenSession(context);
            string sagaId = context.Envelope.SagaId ?? bookReservation.Id;
            if (string.IsNullOrEmpty(sagaId)) throw new Wolverine.Persistence.Sagas.IndeterminateSagaStateIdException(context.Envelope);
            
            // Try to load the existing saga document
            var reservation = await documentSession.LoadAsync<WolverineWebApi.Reservation>(sagaId, cancellation).ConfigureAwait(false);
            if (reservation == null)
            {
                throw new Wolverine.Persistence.Sagas.UnknownSagaException(typeof(WolverineWebApi.Reservation), sagaId);
            }

            else
            {
                reservation.Handle(bookReservation, _logger);
                if (reservation.IsCompleted())
                {
                    
                    // Register the document operation with the current session
                    documentSession.Delete(reservation);
                }

                else
                {
                    
                    // Register the document operation with the current session
                    documentSession.Update(reservation);
                }

                
                // Commit all pending changes
                await documentSession.SaveChangesAsync(cancellation).ConfigureAwait(false);

            }

        }

    }

    // END: BookReservationHandler1573485708
    
    
}
