namespace EventBusRabbitMQ
{
    using System.Threading;
    using System;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Exceptions;

    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory 
                ?? throw new ArgumentNullException(nameof(connectionFactory));
            
            if(!IsConnected)
            {
                TryConnect();
            }
        }
        
        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException )
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }

            return IsConnected;
        }

        public bool IsConnected 
        {
            get => _connection != null && _connection.IsOpen && !_disposed;
        }

        public IModel CreateModel()
        {
            if(!IsConnected)
            {
                throw new InvalidOperationException("No Rabbit connection");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if(_disposed) return;

            try
            {
                _connection.Dispose();
            }
            catch (Exception)
            {                
                throw;
            }
        }    
    }
}