var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Build")
    .Does(() =>
{
    MSBuild("./Source/Obsession.sln", settings =>
        settings.SetConfiguration(configuration));
});

Task("Default")
    .IsDependentOn("Build");
	
RunTarget(target);