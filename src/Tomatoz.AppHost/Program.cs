var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Tomatoz>("tomatoz");

builder.Build().Run();
