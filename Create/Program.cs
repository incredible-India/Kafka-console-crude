//create
using Confluent.Kafka;
using Create;
using System.Text.Json;

var config = new ProducerConfig { BootstrapServers = "localhost:9092",Acks=
Acks.All};

using var producer = new ProducerBuilder<Null, string>(config).Build();

var topic = "Create-Student";


try
{
    while (true)
    {
        Console.WriteLine("\n Enter Student name");
        string name = Console.ReadLine();
        Console.WriteLine("\n Enter Student Age");
        int age = Convert.ToInt32(Console.ReadLine());

        var student = new student { name = name, Age = age };

        var message = new Message<Null, string> { Value = JsonSerializer.Serialize(new student { name = name, Age = age }) };

        var dr = await producer.ProduceAsync(topic, message);

        Console.WriteLine("Student data sent to producer.. " + dr.Message);
    }
    }
catch (ProduceException<Null,string> exc)
{
	Console.WriteLine(exc.Message);
	throw;
}









