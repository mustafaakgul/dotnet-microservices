using StackExchange.Redis;

namespace Microservices.Services.Basket.Services
{
    public class RedisService
    {
        //db ile haberlesecegnzde redis ile port ve host bilgisi lazım bunun constructorde depen inject ile veriyoruz
        private readonly string _host;

        private readonly int _port;

        private ConnectionMultiplexer _ConnectionMultiplexer;

        public RedisService(string host, int port)  //contructordan gelen bilgileri buradaki nesne ile esitlemek
        {
            _host = host;
            _port = port;
        }

        public void Connect() => _ConnectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");  //baglantı kurma metodu

        public IDatabase GetDb(int db = 1) => _ConnectionMultiplexer.GetDatabase(db);  //icindeki db lerdir
    }
}
