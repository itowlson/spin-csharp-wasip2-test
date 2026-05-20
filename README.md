# C# WASI P2 with Spin

1. Install .NET SDK (https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
2. `spin up --build`
3. `curl` it with different paths and it will print the history

More background: https://github.com/bytecodealliance/componentize-dotnet/tree/b132a2a88f5a45229a16c8be8e1cb7228cf0732e#getting-started but I don't _think_ you need anything except the SDK.

NOTE: WASI P2 (Spin 3.4) because I don't think `componentize-dotnet` supports WASI P3.
