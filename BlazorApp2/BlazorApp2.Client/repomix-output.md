This file is a merged representation of a subset of the codebase, containing specifically included files and files not matching ignore patterns, combined into a single document by Repomix.
The content has been processed where comments have been removed, empty lines have been removed.

# File Summary

## Purpose
This file contains a packed representation of the entire repository's contents.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.

## File Format
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Multiple file entries, each consisting of:
  a. A header with the file path (## File: path/to/file)
  b. The full contents of the file in a code block

## Usage Guidelines
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.

- Pay special attention to the Repository Instruction. These contain important context and guidelines specific to this project.

## Notes
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Only files matching these patterns are included: **/*.cs, **/*.razor
- Files matching these patterns are excluded: **/obj/**, **/debug/**, **/appsettings*.json
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Code comments have been removed from supported file types
- Empty lines have been removed from all files

## Additional Info

# Directory Structure
```
_Imports.razor
Layout/MainLayout.razor
Layout/NavMenu.razor
Pages/Counter.razor
Pages/Home.razor
Pages/Weather.razor
Program.cs
Routes.razor
```

# Files

## File: _Imports.razor
```
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
@using BlazorApp2.Client
```

## File: Layout/MainLayout.razor
```
@inherits LayoutComponentBase
<div class="relative flex flex-col md:flex-row">
    <div class="bg-gradient-to-b from-[rgb(5,39,103)] to-[#3a0647] md:w-[250px] md:h-screen md:sticky md:top-0">
        <NavMenu/>
    </div>
    <main class="flex-1">
        <div
            class="h-14 flex items-center justify-between bg-gray-100 border-b border-gray-300 px-4 md:px-8 md:pr-6 md:justify-end md:sticky md:top-0 md:z-10">
            <a href="https://learn.microsoft.com/aspnet/core/" target="_blank"
               class="whitespace-nowrap hover:underline overflow-hidden overflow-ellipsis">
                About
            </a>
        </div>
        <article class="px-4 md:px-8 md:pr-6">
            @Body
        </article>
    </main>
</div>
<div id="blazor-error-ui" data-nosnippet
     class="fixed bottom-0 left-0 w-full bg-yellow-200 p-3 box-border shadow-md z-50 hidden">
    An unhandled error has occurred.
    <a href="." class="reload">Reload</a>
    <span class="absolute right-3 top-2 cursor-pointer">🗙</span>
</div>
```

## File: Layout/NavMenu.razor
```
<div class="flex items-center min-h-[3.5rem] bg-black bg-opacity-40 pl-3">
    <div class="container-fluid">
        <a class="text-[1.1rem]" href="">BlazorApp2</a>
    </div>
</div>

<input type="checkbox" title="Navigation menu"
       class="peer absolute top-2 right-4 w-14 h-10 cursor-pointer border border-white/10 
              bg-[url('data:image/svg+xml,%3csvg xmlns=%27http://www.w3.org/2000/svg%27 viewBox=%270 0 30 30%27%3e%3cpath stroke=%27rgba(255,255,255,0.55)%27 stroke-linecap=%27round%27 stroke-miterlimit=%2710%27 stroke-width=%272%27 d=%27M4 7h22M4 15h22M4 23h22%27/%3e%3c/svg%3e')]
              bg-no-repeat bg-center bg-[length:1.75rem] bg-[rgba(255,255,255,0.1)] md:hidden"/>

<div class="hidden peer-checked:block md:block md:h-[calc(100vh-3.5rem)] md:overflow-y-auto"
     onclick="document.querySelector('.peer').click()">
    <nav class="flex flex-col">
        <div class="px-3 text-sm pb-2 first:pt-4 last:pb-4">
            <NavLink href="" Match="NavLinkMatch.All"
                     class="text-[#d7d7d7] bg-transparent rounded h-12 flex items-center w-full 
                            hover:bg-[rgba(255,255,255,0.1)] hover:text-white"
                     ActiveClass="bg-[rgba(255,255,255,0.37)] text-white">
                <span class="inline-block relative w-5 h-5 mr-3 bg-cover"
                      style="background-image: url('data:image/svg+xml,%3Csvg xmlns=%27http://www.w3.org/2000/svg%27 width=%2716%27 height=%2716%27 fill=%27white%27 class=%27bi bi-house-door-fill%27 viewBox=%270 0 16 16%27%3E%3Cpath d=%27M6.5 14.5v-3.505c0-.245.25-.495.5-.495h2c.25 0 .5.25.5.5v3.5a.5.5 0 0 0 .5.5h4a.5.5 0 0 0 .5-.5v-7a.5.5 0 0 0-.146-.354L13 5.793V2.5a.5.5 0 0 0-.5-.5h-1a.5.5 0 0 0-.5.5v1.293L8.354 1.146a.5.5 0 0 0-.708 0l-6 6A.5.5 0 0 0 1.5 7.5v7a.5.5 0 0 0 .5.5h4a.5.5 0 0 0 .5-.5Z%27/%3E%3C/svg%3E');"
                      aria-hidden="true"></span>
                Home
            </NavLink>
        </div>

        <div class="px-3 text-sm pb-2">
            <NavLink href="counter"
                     class="text-[#d7d7d7] bg-transparent rounded h-12 flex items-center w-full 
                            hover:bg-[rgba(255,255,255,0.1)] hover:text-white"
                     ActiveClass="bg-[rgba(255,255,255,0.37)] text-white">
                <span class="inline-block relative w-5 h-5 mr-3 bg-cover"
                      style="background-image: url('data:image/svg+xml,%3Csvg xmlns=%27http://www.w3.org/2000/svg%27 width=%2716%27 height=%2716%27 fill=%27white%27 class=%27bi bi-plus-square-fill%27 viewBox=%270 0 16 16%27%3E%3Cpath d=%27M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2zm6.5 4.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3a.5.5 0 0 1 1 0z%27/%3E%3C/svg%3E');"
                      aria-hidden="true"></span>
                Counter
            </NavLink>
        </div>

        <div class="px-3 text-sm pb-2">
            <NavLink href="weather"
                     class="text-[#d7d7d7] bg-transparent rounded h-12 flex items-center w-full 
                            hover:bg-[rgba(255,255,255,0.1)] hover:text-white"
                     ActiveClass="bg-[rgba(255,255,255,0.37)] text-white">
                <span class="inline-block relative w-5 h-5 mr-3 bg-cover"
                      style="background-image: url('data:image/svg+xml,%3Csvg xmlns=%27http://www.w3.org/2000/svg%27 width=%2716%27 height=%2716%27 fill=%27white%27 class=%27bi bi-list-nested%27 viewBox=%270 0 16 16%27%3E%3Cpath fill-rule=%27evenodd%27 d=%27M4.5 11.5A.5.5 0 0 1 5 11h10a.5.5 0 0 1 0 1H5a.5.5 0 0 1-.5-.5zm-2-4A.5.5 0 0 1 3 7h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5zm-2-4A.5.5 0 0 1 1 3h10a.5.5 0 0 1 0 1H1a.5.5 0 0 1-.5-.5z%27/%3E%3C/svg%3E');"
                      aria-hidden="true"></span>
                Weather
            </NavLink>
        </div>
    </nav>
</div>
```

## File: Pages/Counter.razor
```
@page "/counter"

<PageTitle>Counter</PageTitle>

<h1>Counter</h1>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }

}
```

## File: Pages/Home.razor
```
@page "/"

<PageTitle>Home</PageTitle>

<h1>Hello, world 22!</h1>

<div class="">test3</div>

Welcome to your new app.
```

## File: Pages/Weather.razor
```
@page "/weather"

<PageTitle>Weather</PageTitle>

<h1>Weather</h1>

<p>This component demonstrates showing data.</p>

@if (forecasts == null)
{
    <p>
        <em>Loading...</em>
    </p>
}
else
{
    <table class="table">
        <thead>
        <tr>
            <th>Date</th>
            <th aria-label="Temperature in Celsius">Temp. (C)</th>
            <th aria-label="Temperature in Farenheit">Temp. (F)</th>
            <th>Summary</th>
        </tr>
        </thead>
        <tbody>
        @foreach (var forecast in forecasts)
        {
            <tr>
                <td>@forecast.Date.ToShortDateString()</td>
                <td>@forecast.TemperatureC</td>
                <td>@forecast.TemperatureF</td>
                <td>@forecast.Summary</td>
            </tr>
        }
        </tbody>
    </table>
}

@code {
    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        // Simulate asynchronous loading to demonstrate a loading indicator
        await Task.Delay(500);

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        }).ToArray();
    }

    private class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

}
```

## File: Program.cs
```csharp
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
await builder.Build().RunAsync();
```

## File: Routes.razor
```
<Router AppAssembly="typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)"/>
        <FocusOnNavigate RouteData="routeData" Selector="h1"/>
    </Found>
</Router>
```


# Instruction
﻿# Coding Guidelines

- Follow the Airbnb JavaScript Style Guide
- Suggest splitting files into smaller, focused units when appropriate
- Add comments for non-obvious logic. Keep all text in English
- All new features should have corresponding unit tests

# Generate Comprehensive Output

- Include all content without abbreviation, unless specified otherwise
- Optimize for handling large codebases while maintaining output quality
