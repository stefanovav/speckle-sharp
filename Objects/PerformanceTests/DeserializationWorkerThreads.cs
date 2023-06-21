#pragma warning disable CA2007
using System.Diagnostics;
using Speckle.Core.Credentials;
using Speckle.Core.Kits;
using Speckle.Core.Serialisation;
using Speckle.Core.Transports;

public static class DeserialiationWorkerThreads_Tests
{
  private const int numberOfIterations = 5;
  private static readonly IEnumerable<int> threads = Enumerable.Range(0, Environment.ProcessorCount + 1);

  private static MemoryTransport transport;

  private static async Task<StreamWrapper[]> TestData() => new[]
  {
    await NewTestCase("https://latest.speckle.dev/streams/efd2c6a31d/objects/62de853962036fc3408b681777c597d3"), //1 object
    await NewTestCase("https://latest.speckle.dev/streams/efd2c6a31d/objects/9df81f53498fdae9f698dfe3bc407c53"), //1 model
    await NewTestCase("https://latest.speckle.dev/streams/efd2c6a31d/objects/27267610fb44bd50c5df846cb0bb21c6"), //10 models
    //await NewTestCase("https://latest.speckle.dev/streams/efd2c6a31d/objects/f49468d800041d35b6390ac844ab8c89"), //20 models
  };
  
  public static async Task Main()
  {
    _ = KitManager.Kits; //to force kit manager to initialise so we don't get console logs in our table...
    
    transport = new();

    foreach (var testData in await TestData())
    {
      RunBenchmark(testData);
    }
  }

  static void RunBenchmark(StreamWrapper testData)
  {
    Console.WriteLine($"Starting benchmark for {testData.ObjectId}");
    Console.WriteLine("");
    Console.WriteLine("| NUM THREADS | MEAN | STDDEV |");
    Console.WriteLine("| ----------- | ---- | ------ |");
    
    foreach(int i in threads)
    {
      var results = new long[numberOfIterations];
      for (int t = 0; t < numberOfIterations; t++)
      {
        var sw = Stopwatch.StartNew();
        RunTest(i, testData);
        sw.Stop();

        results[t] = sw.ElapsedMilliseconds;
      }

      var mean = results.Average();
      var std = Math.Sqrt(results.Average(v=>Math.Pow(v-mean,2)));
      Console.WriteLine($"| {i} | {mean}ms | {std:0.00} |");
    }
  }



  static void RunTest(int numThreads, StreamWrapper testData)
  {
    BaseObjectDeserializerV2 sut = new()
    {
      WorkerThreadCount = numThreads,
      ReadTransport = transport,
    };

    var stopWatch = Stopwatch.StartNew();
    
    sut.Deserialize(transport.GetObject(testData.ObjectId));
    
    stopWatch.Stop();
  }

  static async Task<StreamWrapper> NewTestCase(string streamWrapperInput)
  {
    StreamWrapper stream = new(streamWrapperInput);
    var acc = await stream.GetAccount();
    using ServerTransport remote = new(acc, stream.StreamId);


    await remote.CopyObjectAndChildren(stream.ObjectId, transport);
    return stream;
  }
}
