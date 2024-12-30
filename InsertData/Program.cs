using Confluent.Kafka;
using InsertData;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

//for receving the message
var ConsumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "test-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

//for sending messages
var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092",
    Acks = Acks.All

};

using var consumer = new ConsumerBuilder<Ignore, string>(ConsumerConfig).Build();
using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();


consumer.Subscribe("Create-Student");
List<student> students = new List<student>();
CancellationTokenSource cts = new CancellationTokenSource();
try
{
   while(true)
    {
        //receive data
        var cr = consumer.Consume(cts.Token);
        var student = JsonSerializer.Deserialize<student>(cr.Message.Value);
        students.Add(student);
        Console.WriteLine($"Student name: {student.name}, Age: {student.Age}");

        // now send data
        var message = new Message<Null, string> { Value = JsonSerializer.Serialize(students) };
        var dr = await producer.ProduceAsync("read-Student", message);
        Console.WriteLine("studen data send to read micrservice "+ dr.Message.Value);
    }

}
catch (OperationCanceledException exc)
{
    Console.WriteLine(exc.Message);
	throw;
}