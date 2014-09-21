param($installPath, $toolsPath, $package, $project)

$folder = $project.ProjectItems | where-object {$_.Name -eq "Model"}
$item = $folder.ProjectItems | where-object {$_.Name -eq "Definition.domain"}
$item.Properties.Item("CustomTool").Value = "MSBuild:Compile"