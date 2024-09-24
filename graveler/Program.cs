using System.Diagnostics;

Random random = new Random();
int paralyzed = 1;

int maxParalyzedTurns = 0;

Stopwatch sw = Stopwatch.StartNew();
int taskBatch = 10000000;

int parallelCount = Math.Max(2, Environment.ProcessorCount - 4);


for (int i = 0; i < 100; i++) {
    Task<int>[] taskList = new Task<int>[taskBatch];
    Parallel.For(0, taskList.Length, new ParallelOptions { MaxDegreeOfParallelism = parallelCount }, i => {
        taskList[i] = Task.Run(async () => {
            int result = await AttemptTask();
            maxParalyzedTurns = result > maxParalyzedTurns ? result : maxParalyzedTurns;
            return result;
        });
    });

    await Task.WhenAll(taskList);

    Console.WriteLine(sw.Elapsed.ToString());
    Console.WriteLine($"Finished {((i+1) * taskBatch)} Attempts");
}

sw.Stop();
Console.WriteLine(sw.Elapsed.ToString());
Console.WriteLine(maxParalyzedTurns);

async Task<int> AttemptTask() {
    int paralyzedTurns = 0;

    for (int i = 0; i < 231; i++) {
        if (random.Next() % 4 == paralyzed)
            paralyzedTurns++;
    }
    return paralyzedTurns;
}

